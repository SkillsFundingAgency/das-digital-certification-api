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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenCreatingCertificateSharing
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<SharingController>> _loggerMock = null!;
        private SharingController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SharingController>>();
            _controller = new SharingController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_MediatorIsCalledAndReturns200()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var linkCode = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var expiry = now.AddDays(28);
            var command = new CreateCertificateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = "Standard",
                CourseName = "Test Course"
            };
            var response = new CreateCertificateSharingCommandResponse
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = "Standard",
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber =1,
                CreatedAt = now,
                LinkCode = linkCode,
                ExpiryTime = expiry,
                SharingAccess = [],
                SharingEmails = []
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.CreateCertificateSharing(command);

            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returned = okResult!.Value as CreateCertificateSharingCommandResponse;
            returned.Should().NotBeNull();
            returned!.UserId.Should().Be(userId);
            returned.CertificateId.Should().Be(certificateId);
            returned.CertificateType.Should().Be("Standard");
            returned.CourseName.Should().Be("Test Course");
            returned.SharingId.Should().Be(sharingId);
            returned.SharingNumber.Should().Be(1);
            returned.CreatedAt.Should().Be(now);
            returned.LinkCode.Should().Be(linkCode);
            returned.ExpiryTime.Should().Be(expiry);
            returned.SharingAccess.Should().BeEmpty();
            returned.SharingEmails.Should().BeEmpty();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

            var result = await _controller.CreateCertificateSharing(command);

            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.CreateCertificateSharing(command);

            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
