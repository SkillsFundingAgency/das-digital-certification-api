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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenCreatingSharingEmailAccess
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<SharingEmailAccessController>> _loggerMock = null!;
        private SharingEmailAccessController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SharingEmailAccessController>>();
            _sut = new SharingEmailAccessController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_MediatorReturnsId_Then_ReturnsNoContent()
        {
            // Arrange
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };
            var createdId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(createdId);

            // Act
            var result = await _sut.CreateSharingEmailAccess(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var noContent = result as NoContentResult;
            noContent.Should().NotBeNull();
        }

        [Test]
        public async Task And_MediatorReturnsNull_Then_ReturnsBadRequest()
        {
            // Arrange
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync((Guid?)null);

            // Act
            var result = await _sut.CreateSharingEmailAccess(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequestObject()
        {
            // Arrange
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateSharingEmailAccess(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateSharingEmailAccess(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
