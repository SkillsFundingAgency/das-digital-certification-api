using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenUnlockingUser
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<UsersController>> _loggerMock = null!;
        private UsersController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _sut = new UsersController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_UserNotFound_Then_ReturnsBadRequestWithUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UnlockUserCommandResponse { NotFound = true });

            // Act
            var result = await _sut.UnlockUser(userId);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { userId });
        }

        [Test]
        public async Task And_UserUpdated_Then_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UnlockUserCommandResponse { Updated = true });

            // Act
            var result = await _sut.UnlockUser(userId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task And_UserAlreadyUnlocked_Then_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UnlockUserCommandResponse { Updated = false, NotFound = false });

            // Act
            var result = await _sut.UnlockUser(userId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.UnlockUser(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected"));

            // Act
            var result = await _sut.UnlockUser(userId);

            // Assert
            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
