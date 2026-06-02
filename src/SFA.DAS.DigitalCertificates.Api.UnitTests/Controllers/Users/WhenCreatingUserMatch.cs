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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingUserMatch
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
        public async Task And_ValidRequest_Then_MediatorIsCalledAndReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserMatchCommand
            {
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990, 1, 1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Test Provider",
                Ukprn = 123456
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserMatchCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _sut.CreateUserMatch(userId, command);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.Is<CreateUserMatchCommand>(c => c.UserId == userId), It.IsAny<CancellationToken>()),
                Times.Once);

            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserMatchCommand
            {
                FamilyName = "Brown",
                DateOfBirth = new DateTime(1992, 8, 10),
                CertificateType = CertificateType.Standard,
                CourseCode = "C999",
                CourseName = "Carpentry",
                ProviderName = "Build Academy",
                Ukprn = 111222
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserMatchCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _sut.CreateUserMatch(userId, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_GeneralException_Then_ReturnsInternalServerError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserMatchCommand
            {
                FamilyName = "Taylor",
                DateOfBirth = new DateTime(1995, 11, 25),
                CertificateType = CertificateType.Standard,
                CourseCode = "C456",
                CourseName = "Plumbing",
                ProviderName = "Trade School",
                Ukprn = 333444
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserMatchCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.CreateUserMatch(userId, command);

            // Assert
            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
