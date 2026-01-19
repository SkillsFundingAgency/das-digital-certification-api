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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenCreatingSharingEmail
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<SharingController>> _loggerMock = null!;
        private SharingController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SharingController>>();
            _sut = new SharingController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_MediatorIsCalledAndReturns200()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };
            var response = new CreateSharingEmailCommandResponse { Id = Guid.NewGuid(), EmailLinkCode = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(It.Is<CreateSharingEmailCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // Act
            var result = await _sut.CreateSharingEmail(sharingId, command);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<CreateSharingEmailCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returned = okResult!.Value as CreateSharingEmailCommandResponse;
            returned.Should().NotBeNull();
            returned!.Id.Should().Be(response.Id);
            returned.EmailLinkCode.Should().Be(response.EmailLinkCode);
        }

        [Test]
        public async Task And_NotFound_Then_ReturnsNotFound()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingEmailCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync((CreateSharingEmailCommandResponse?)null);

            // Act
            var result = await _sut.CreateSharingEmail(sharingId, command);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingEmailCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateSharingEmail(sharingId, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnsInternalServerError()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingEmailCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateSharingEmail(sharingId, command);

            // Assert
            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
