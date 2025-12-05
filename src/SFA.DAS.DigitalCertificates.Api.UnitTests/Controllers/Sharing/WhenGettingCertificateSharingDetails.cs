using System;
using System.Collections.Generic;
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
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingCertificateSharingDetails
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
        public async Task And_ValidRequest_Then_ReturnOk()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var details = new CertificateSharingDetails { UserId = userId, CertificateId = certId, CertificateType = "TypeA", CourseName = "CourseName" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCertificateSharingDetailsQueryResult { SharingDetails = details });

            var result = await _controller.GetCertificateSharingDetails(userId, certId);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(details);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_NoSharingDetailsFound_Then_ReturnOkWithEmptyDetails()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var emptyDetails = new CertificateSharingDetails
            {
                UserId = userId,
                CertificateId = certId,
                CertificateType = string.Empty,
                CourseName = string.Empty,
                Sharings = new List<SharingDetail>()
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCertificateSharingDetailsQueryResult { SharingDetails = emptyDetails });

            var result = await _controller.GetCertificateSharingDetails(userId, certId);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(emptyDetails);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnBadRequestWithErrors()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var validationException = new ValidationException("Validation failed");
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            var result = await _controller.GetCertificateSharingDetails(userId, certId);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.GetCertificateSharingDetails(userId, certId);

            result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}