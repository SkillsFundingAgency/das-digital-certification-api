using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateOrUpdateUser
{
    public class WhenHandlingCreateOrUpdateUserCommand
    {
        [OneTimeSetUp]
        public void GlobalFixtureSetup()
        {
            Fixture fixture = new();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test, MoqAutoData]
        public async Task And_UserDoesNotExist_Then_AddsUserAndSavesChanges(
            CreateOrUpdateUserCommand command,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IUserEntityContext> userEntityContext,
            [Frozen] Mock<IUserIdentityEntityContext> userIdentityEntityContext)
        {
            // Arrange
            var now = System.DateTime.UtcNow;
            dateTimeProvider.Setup(x => x.Now).Returns(now);
            userEntityContext.Setup(x => x.GetWithIdentities(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            userEntityContext
                .Setup(x => x.Add(It.IsAny<User>()))
                .Returns((EntityEntry<User>)null!);

            var _sut = new CreateOrUpdateUserCommandHandler(dateTimeProvider.Object, userEntityContext.Object);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            userEntityContext.Verify(x => x.Add(It.Is<User>(u =>
                u.GovUkIdentifier == command.GovUkIdentifier &&
                u.EmailAddress == command.EmailAddress &&
                u.PhoneNumber == command.PhoneNumber &&
                u.LastLoginAt == now
            )), Times.Once);

            userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task And_UserExists_Then_UpdatesEmailPhoneAndLastLogin(
            CreateOrUpdateUserCommand command,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IUserEntityContext> userEntityContext,
            [Frozen] Mock<IUserIdentityEntityContext> userIdentityEntityContext)
        {
            // Arrange
            var utcNow = DateTime.UtcNow;
            var now = DateTime.Now;

            dateTimeProvider.Setup(x => x.Now).Returns(now);
            dateTimeProvider.Setup(x => x.UtcNow).Returns(utcNow);

            var existingUser = new User { Id = Guid.NewGuid(), GovUkIdentifier = command.GovUkIdentifier, EmailAddress = "current@email.com", CreatedAt = utcNow.AddMonths(-1) };
            userEntityContext.Setup(x => x.GetWithIdentities(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            var _sut = new CreateOrUpdateUserCommandHandler(dateTimeProvider.Object, userEntityContext.Object);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            userEntityContext.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
            userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            existingUser.EmailAddress.Should().Be(command.EmailAddress);
            existingUser.PhoneNumber.Should().Be(command.PhoneNumber);
            existingUser.LastLoginAt.Should().Be(now);
            existingUser.CreatedAt.Should().Be(utcNow.AddMonths(-1));

            result.Should().NotBeNull();
            result.UserId.Should().Be(existingUser.Id);
        }

        [Test, MoqAutoData]
        public async Task And_NamesProvided_Then_ReplacesUserIdentities(
            CreateOrUpdateUserCommand command,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IUserEntityContext> userEntityContext)
        {
            // Arrange
            var utcNow = DateTime.UtcNow;
            var now = DateTime.Now;

            dateTimeProvider.Setup(x => x.Now).Returns(now);
            dateTimeProvider.Setup(x => x.UtcNow).Returns(utcNow);

            var existingIdentities = new List<UserIdentity>
            {
                new UserIdentity { Id = Guid.NewGuid(), FamilyName = "Old", GivenNames = "OldName", DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) }
            };

            var existingUser = new User { Id = Guid.NewGuid(), GovUkIdentifier = command.GovUkIdentifier, EmailAddress = "current@email.com", CreatedAt = utcNow.AddMonths(-1), UserIdentities = existingIdentities };
            userEntityContext.Setup(x => x.GetWithIdentities(It.IsAny<string>())).ReturnsAsync(existingUser);

            userEntityContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var sut = new CreateOrUpdateUserCommandHandler(dateTimeProvider.Object, userEntityContext.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            userEntityContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
