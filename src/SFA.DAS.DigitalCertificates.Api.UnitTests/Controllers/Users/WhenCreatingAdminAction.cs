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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;
using SFA.DAS.DigitalCertificates.Api.Models;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingAdminAction
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<UserActionsController>> _loggerMock = null!;
        private UserActionsController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UserActionsController>>();
            _sut = new UserActionsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_MediatorIsCalledAndReturns204()
        {
            // Arrange
            var command = new CreateAdminActionRequest
            {
                Username = "admin",
                Action = AdminActionType.Viewed
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAdminActionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _sut.CreateAdminAction(1, command);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateAdminActionCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var status = result.Should().BeOfType<NoContentResult>().Which;
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var command = new CreateAdminActionRequest { Username = "", Action = AdminActionType.Viewed };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAdminActionCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateAdminAction(0, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var command = new CreateAdminActionRequest { Username = "admin", Action = AdminActionType.Viewed };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAdminActionCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateAdminAction(1, command);

            // Assert
            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
