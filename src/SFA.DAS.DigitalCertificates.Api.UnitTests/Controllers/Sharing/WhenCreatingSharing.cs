using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenCreatingSharing
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
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var linkCode = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var expiry = now.AddDays(28);

            var command = new CreateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            var response = new CreateSharingCommandResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber = 1,
                CreatedAt = now,
                LinkCode = linkCode,
                ExpiryTime = expiry
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.CreateSharing(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returned = okResult!.Value as CreateSharingCommandResponse;
            returned.Should().NotBeNull();

            returned!.UserId.Should().Be(userId);
            returned.CertificateId.Should().Be(certificateId);
            returned.CertificateType.Should().Be(CertificateType.Standard);
            returned.CourseName.Should().Be("Test Course");
            returned.SharingId.Should().Be(sharingId);
            returned.SharingNumber.Should().Be(1);
            returned.CreatedAt.Should().Be(now);
            returned.LinkCode.Should().Be(linkCode);
            returned.ExpiryTime.Should().Be(expiry);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Framework,
                CourseName = "Test Course"
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateSharing(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateSharing(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}