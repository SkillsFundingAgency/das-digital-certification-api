using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenDeletingSharing
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
        public async Task And_ValidRequest_Then_ReturnsNoContent()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var response = new DeleteSharingCommandResponse { SharingId = sharingId };

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.DeleteSharing(sharingId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task And_ResultIsNull_Then_ReturnsBadRequest()
        {
            // Arrange
            var sharingId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeleteSharingCommandResponse?)null);

            // Act
            var result = await _sut.DeleteSharing(sharingId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnsInternalServerError()
        {
            // Arrange
            var sharingId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSharingCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _sut.DeleteSharing(sharingId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<StatusCodeResult>();
            var status = (StatusCodeResult)result;
            status.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var sharingId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSharingCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.DeleteSharing(sharingId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
