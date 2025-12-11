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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Domain.Models;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingSharings
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<UsersController>> _loggerMock = null!;
        private UsersController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _sut = new UsersController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_ReturnOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var details = new CertificateSharings
            {
                UserId = userId,
                CertificateId = certId,
                CertificateType = CertificateType.Standard,
                CourseName = "CourseName"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetSharingsQueryResult { SharingDetails = details });

            // Act
            var result = await _sut.GetSharings(userId, certId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(details);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_SharingDetailsWithEmptyList_Then_ReturnOkWithEmptyDetails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var emptyDetails = new CertificateSharings
            {
                UserId = userId,
                CertificateId = certId,
                CertificateType = CertificateType.Unknown,
                CourseName = string.Empty,
                Sharings = new List<SharingDetail>()
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetSharingsQueryResult { SharingDetails = emptyDetails });

            // Act
            var result = await _sut.GetSharings(userId, certId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(emptyDetails);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_NoLiveSharingsExist_Then_ReturnOkWithEmptyDetails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var emptyDetails = new CertificateSharings
            {
                UserId = userId,
                CertificateId = certId,
                CertificateType = CertificateType.Unknown,
                CourseName = string.Empty,
                Sharings = new List<SharingDetail>()
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetSharingsQueryResult { SharingDetails = emptyDetails });

            // Act
            var result = await _sut.GetSharings(userId, certId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(emptyDetails);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnBadRequestWithErrors()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var validationException = new ValidationException("Validation failed");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await _sut.GetSharings(userId, certId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.GetSharings(userId, certId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}