using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingUserAuthorisation
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
        public async Task And_ValidRequest_Then_MediatorIsCalledAndReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new Models.CreateUserAuthorisationRequest
            {
                Uln = 1234567890
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserAuthorisationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _sut.CreateUserAuthorisation(userId, request);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.Is<CreateUserAuthorisationCommand>(c => c.UserId == userId && c.Uln == request.Uln), It.IsAny<CancellationToken>()),
                Times.Once);

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new Models.CreateUserAuthorisationRequest
            {
                Uln = 1234567890
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserAuthorisationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateUserAuthorisation(userId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new Models.CreateUserAuthorisationRequest
            {
                Uln = 1234567890
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserAuthorisationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateUserAuthorisation(userId, request);

            // Assert
            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
