using System;
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

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands
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
            [Frozen] Mock<IUserEntityContext> userEntityContext)
        {
            // Arrange
            var now = System.DateTime.UtcNow;
            dateTimeProvider.Setup(x => x.Now).Returns(now);
            userEntityContext.Setup(x => x.Get(command.GovUkIdentifier))
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
            [Frozen] Mock<IUserEntityContext> userEntityContext)
        {
            // Arrange
            var now = DateTime.UtcNow;
            dateTimeProvider.Setup(x => x.Now).Returns(now);
            
            var existingUser = new User { Id = Guid.NewGuid(), GovUkIdentifier = command.GovUkIdentifier, EmailAddress = "current@email.com" };
            userEntityContext.Setup(x => x.Get(command.GovUkIdentifier))
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

            result.Should().NotBeNull();
            result.UserId.Should().Be(existingUser.Id);
        }
    }
}
