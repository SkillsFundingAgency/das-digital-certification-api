using System;
using System.Threading;
using System.Threading.Tasks;
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
        private Mock<IUserEntityContext> _userEntityContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateOrUpdateUserCommandHandler _sut = null!;
        
        private readonly DateTime _utcNow = DateTime.UtcNow;
        private readonly DateTime _now = DateTime.Now;

        [SetUp]
        public void SetUp()
        {
            _userEntityContextMock = new Mock<IUserEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _dateTimeProviderMock.SetupGet(d => d.UtcNow).Returns(_utcNow);
            _dateTimeProviderMock.SetupGet(d => d.Now).Returns(_now);

            _sut = new CreateOrUpdateUserCommandHandler(_dateTimeProviderMock.Object, _userEntityContextMock.Object);
        }

        [Test, MoqAutoData]
        public async Task And_UserDoesNotExist_Then_AddsUserAndSavesChanges(
            CreateOrUpdateUserCommand command)
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            User? addedUser = null;

            _userEntityContextMock
                .Setup(x => x.Get(command.GovUkIdentifier))
                .ReturnsAsync((User?)null);

            _userEntityContextMock
                .Setup(x => x.Add(It.IsAny<User>()))
                .Callback<User>(user => addedUser = user)
                .Returns((EntityEntry<User>)null!);

            // Act
            var result = await _sut.Handle(command, cancellationToken);

            // Assert
            addedUser.Should().NotBeNull();
            addedUser!.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
            addedUser.EmailAddress.Should().Be(command.EmailAddress);
            addedUser.PhoneNumber.Should().Be(command.PhoneNumber);
            addedUser.CreatedAt.Should().Be(_utcNow);
            addedUser.LastLoginAt.Should().Be(_now);

            result.UserId.Should().Be(addedUser.Id);

            _userEntityContextMock.Verify(
                x => x.Get(command.GovUkIdentifier),
                Times.Once);

            _userEntityContextMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Once);

            _userEntityContextMock.Verify(
                x => x.SaveChangesAsync(cancellationToken),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_UserExists_Then_UpdatesUserAndSavesChanges(
            CreateOrUpdateUserCommand command)
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var originalCreatedAt = _utcNow.AddMonths(-1);

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                GovUkIdentifier = command.GovUkIdentifier,
                EmailAddress = "current@email.com",
                PhoneNumber = "0123456789",
                CreatedAt = originalCreatedAt
            };

            _userEntityContextMock
                .Setup(x => x.Get(command.GovUkIdentifier))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _sut.Handle(command, cancellationToken);

            // Assert
            existingUser.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
            existingUser.EmailAddress.Should().Be(command.EmailAddress);
            existingUser.PhoneNumber.Should().Be(command.PhoneNumber);
            existingUser.LastLoginAt.Should().Be(_now);
            existingUser.CreatedAt.Should().Be(originalCreatedAt);

            result.UserId.Should().Be(existingUser.Id);

            _userEntityContextMock.Verify(
                x => x.Get(command.GovUkIdentifier),
                Times.Once);

            _userEntityContextMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _userEntityContextMock.Verify(
                x => x.SaveChangesAsync(cancellationToken),
                Times.Once);
        }
    }
}
