using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UpdateUserIdentity
{
    public class WhenHandlingUpdateUserIdentityCommand
    {
        private Mock<IUserEntityContext> _userEntityContext = null!;
        private Mock<IUserIdentityEntityContext> _userIdentityEntityContext = null!;
        private UpdateUserIdentityCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userEntityContext = new Mock<IUserEntityContext>();
            _userIdentityEntityContext = new Mock<IUserIdentityEntityContext>();

            _sut = new UpdateUserIdentityCommandHandler(
                _userEntityContext.Object,
                _userIdentityEntityContext.Object);
        }

        [Test]
        public async Task And_UserDoesNotExist_Then_ThrowsValidationException()
        {
            var userId = Guid.NewGuid();

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { FamilyName = "Smith", GivenNames = "John" }
                },
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            }, userId);

            _userEntityContext
                .Setup(x => x.GetWithIdentitiesByUserId(userId))
                .ReturnsAsync((User?)null);

            var act = async () => await _sut.Handle(command, CancellationToken.None);

            var exception = await act.Should().ThrowAsync<ValidationException>();

            exception.Which.Errors.Should().ContainSingle(x =>
                x.PropertyName == nameof(UpdateUserIdentityCommand.UserId) &&
                x.ErrorMessage == "UserId not found");

            _userIdentityEntityContext.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<UserIdentity>>()), Times.Never);
            _userIdentityEntityContext.Verify(x => x.Add(It.IsAny<UserIdentity>()), Times.Never);
            _userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_NamesProvided_Then_ReplacesUserIdentities()
        {
            var userId = Guid.NewGuid();

            var existingIdentities = new List<UserIdentity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FamilyName = "Old",
                    GivenNames = "OldName",
                    DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            };

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "gov-123",
                EmailAddress = "current@email.com",
                UserIdentities = existingIdentities
            };

            var dateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var validSince = new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var validUntil = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Unspecified);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new()
                    {
                        FamilyName = "Smith",
                        GivenNames = "John|Paul",
                        ValidSince = validSince,
                        ValidUntil = validUntil
                    }
                },
                DateOfBirth = dateOfBirth
            }, userId);

            _userEntityContext
                .Setup(x => x.GetWithIdentitiesByUserId(userId))
                .ReturnsAsync(user);

            _userIdentityEntityContext
                .Setup(x => x.Add(It.IsAny<UserIdentity>()))
                .Returns((EntityEntry<UserIdentity>)null!);

            _userEntityContext
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _sut.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);

            _userIdentityEntityContext.Verify(x =>
                x.RemoveRange(It.Is<IEnumerable<UserIdentity>>(r => r == existingIdentities)),
                Times.Once);

            _userIdentityEntityContext.Verify(x =>
                x.Add(It.Is<UserIdentity>(identity =>
                    identity.User == user &&
                    identity.FamilyName == "Smith" &&
                    identity.GivenNames == "John|Paul" &&
                    identity.DateOfBirth == dateOfBirth &&
                    identity.ValidSince == validSince &&
                    identity.ValidUntil == validUntil)),
                Times.Once);

            _userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_NamesProvided_And_UserHasNoExistingIdentities_Then_AddsNewIdentities()
        {
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "gov-123",
                EmailAddress = "current@email.com",
                UserIdentities = null
            };

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { FamilyName = "Smith", GivenNames = "John" }
                },
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            }, userId);

            _userEntityContext
                .Setup(x => x.GetWithIdentitiesByUserId(userId))
                .ReturnsAsync(user);

            _userIdentityEntityContext
                .Setup(x => x.Add(It.IsAny<UserIdentity>()))
                .Returns((EntityEntry<UserIdentity>)null!);

            await _sut.Handle(command, CancellationToken.None);

            _userIdentityEntityContext.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<UserIdentity>>()), Times.Never);
            _userIdentityEntityContext.Verify(x => x.Add(It.IsAny<UserIdentity>()), Times.Once);
            _userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_MultipleNamesProvided_Then_AddsAllUserIdentities()
        {
            var userId = Guid.NewGuid();

            var existingIdentities = new List<UserIdentity>
            {
                new() { Id = Guid.NewGuid(), FamilyName = "Old", GivenNames = "OldName" }
            };

            var user = new User
            {
                Id = userId,
                GovUkIdentifier = "gov-123",
                EmailAddress = "current@email.com",
                UserIdentities = existingIdentities
            };

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { FamilyName = "Smith", GivenNames = "John" },
                    new() { FamilyName = "Jones", GivenNames = "Jonathan" }
                },
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            }, userId);

            _userEntityContext
                .Setup(x => x.GetWithIdentitiesByUserId(userId))
                .ReturnsAsync(user);

            _userIdentityEntityContext
                .Setup(x => x.Add(It.IsAny<UserIdentity>()))
                .Returns((EntityEntry<UserIdentity>)null!);

            await _sut.Handle(command, CancellationToken.None);

            _userIdentityEntityContext.Verify(x => x.RemoveRange(existingIdentities), Times.Once);
            _userIdentityEntityContext.Verify(x => x.Add(It.IsAny<UserIdentity>()), Times.Exactly(2));
            _userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}