using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UnlockUser
{
    public class WhenHandlingUnlockUserCommandHandler
    {
        private Mock<IUserEntityContext> _userContextMock = null!;
        private UnlockUserCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userContextMock = new Mock<IUserEntityContext>();
            _sut = new UnlockUserCommandHandler(_userContextMock.Object);
        }

        [Test]
        public async Task And_UserDoesNotExist_Then_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync((User?)null);

            var command = new UnlockUserCommand { UserId = userId };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.NotFound.Should().BeTrue();
            result.Updated.Should().BeFalse();
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_UserAlreadyUnlocked_Then_ReturnsUpdatedFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com", IsLocked = false };
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync(user);

            var command = new UnlockUserCommand { UserId = userId };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.NotFound.Should().BeFalse();
            result.Updated.Should().BeFalse();
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_UserIsLocked_Then_SetsIsLockedFalse_And_Saves()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com", IsLocked = true };
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync(user);
            _userContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new UnlockUserCommand { UserId = userId };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.NotFound.Should().BeFalse();
            result.Updated.Should().BeTrue();
            user.IsLocked.Should().BeFalse();
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
