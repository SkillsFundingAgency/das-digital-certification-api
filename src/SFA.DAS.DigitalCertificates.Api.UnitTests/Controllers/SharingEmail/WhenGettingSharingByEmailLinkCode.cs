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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.SharingEmail
{
    [TestFixture]
    public class WhenGettingSharingByEmailLinkCode
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<SharingEmailController>> _loggerMock = null!;
        private SharingEmailController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SharingEmailController>>();
            _sut = new SharingEmailController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_ReturnOkWithSharingEmail()
        {
            // Arrange
            var emailLinkCode = Guid.NewGuid();

            var summary = new CertificateSharingEmailLinkSummary
            {
                SharingEmailId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = Enums.CertificateType.Standard,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var queryResult = new GetSharingByEmailLinkCodeQueryResult { SharingEmail = summary };

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetSharingByEmailLinkCode(emailLinkCode);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var ok = (OkObjectResult)result;
            ok.Value.Should().Be(summary);

            _mediatorMock.Verify(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_NotFound_Then_ReturnsNotFound()
        {
            // Arrange
            var emailLinkCode = Guid.NewGuid();
            var queryResult = new GetSharingByEmailLinkCodeQueryResult { SharingEmail = null };

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetSharingByEmailLinkCode(emailLinkCode);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _mediatorMock.Verify(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var emailLinkCode = Guid.NewGuid();

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetSharingByEmailLinkCodeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.GetSharingByEmailLinkCode(emailLinkCode);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            _mediatorMock.Verify(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_Exception_Then_ReturnsInternalServerError()
        {
            // Arrange
            var emailLinkCode = Guid.NewGuid();

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetSharingByEmailLinkCodeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected"));

            // Act
            var result = await _sut.GetSharingByEmailLinkCode(emailLinkCode);

            // Assert
            result.Should().BeOfType<StatusCodeResult>();
            var status = (StatusCodeResult)result;
            status.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _mediatorMock.Verify(x => x.Send(It.Is<GetSharingByEmailLinkCodeQuery>(q => q.EmailLinkCode == emailLinkCode), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
