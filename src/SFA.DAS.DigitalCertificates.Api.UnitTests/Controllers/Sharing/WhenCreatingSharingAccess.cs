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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenCreatingSharingAccess
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
        public async Task And_MediatorReturnsId_Then_ReturnsNoContent()
        {
            // Arrange
            var request = new Models.CreateSharingAccessRequest { SharingId = Guid.NewGuid() };
            var createdId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.Is<CreateSharingAccessCommand>(c => c.SharingId == request.SharingId), It.IsAny<CancellationToken>())).ReturnsAsync(createdId);

            // Act
            var result = await _sut.CreateSharingAccess(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<CreateSharingAccessCommand>(c => c.SharingId == request.SharingId), It.IsAny<CancellationToken>()), Times.Once);

            var noContent = result as NoContentResult;
            noContent.Should().NotBeNull();
        }

        [Test]
        public async Task And_MediatorReturnsNull_Then_ReturnsBadRequest()
        {
            // Arrange
            var request = new Models.CreateSharingAccessRequest { SharingId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync((Guid?)null);

            // Act
            var result = await _sut.CreateSharingAccess(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequestObject()
        {
            // Arrange
            var request = new Models.CreateSharingAccessRequest { SharingId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateSharingAccess(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var request = new Models.CreateSharingAccessRequest { SharingId = Guid.NewGuid() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateSharingAccess(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
