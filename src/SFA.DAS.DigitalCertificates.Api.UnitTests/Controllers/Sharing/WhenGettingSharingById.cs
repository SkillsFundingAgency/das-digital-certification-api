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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.Domain.Models;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    [TestFixture]
    public class WhenGettingSharingById
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
        public async Task And_ValidRequest_Then_ReturnOkWithSharing()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var sharing = new CertificateSharing
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber = 1,
                CreatedAt = DateTime.UtcNow,
                LinkCode = Guid.NewGuid(),
                ExpiryTime = DateTime.UtcNow.AddDays(30)
            };

            var queryResult = new GetSharingByIdQueryResult { Sharing = sharing };

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetSharingById(sharingId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().Be(sharing);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId && q.Limit == null), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task And_ValidRequestWithLimit_Then_ReturnOkWithSharing()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var limit = 10;
            var sharing = new CertificateSharing
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course",
                SharingId = sharingId,
                SharingNumber = 1,
                CreatedAt = DateTime.UtcNow,
                LinkCode = Guid.NewGuid(),
                ExpiryTime = DateTime.UtcNow.AddDays(30)
            };

            var queryResult = new GetSharingByIdQueryResult { Sharing = sharing };

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId && q.Limit == limit), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetSharingById(sharingId, limit);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().Be(sharing);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId && q.Limit == limit), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnNotFound()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var queryResult = new GetSharingByIdQueryResult { Sharing = null };

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetSharingById(sharingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnBadRequestWithErrors()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var validationException = new ValidationException("Test validation error");

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await _sut.GetSharingById(sharingId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var exception = new Exception("Test error");

            _mediatorMock
              .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _sut.GetSharingById(sharingId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>();
            var statusCodeResult = (StatusCodeResult)result;
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetSharingByIdQuery>(q => q.SharingId == sharingId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}