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
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserActionByCode
    {
        private Mock<IMediator> _mediatorMock = null!;
        private Mock<ILogger<UserActionsController>> _loggerMock = null!;
        private UserActionsController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UserActionsController>>();
            _sut = new UserActionsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task And_ValidRequest_Then_ReturnOkWithUserAction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expected = new GetUserActionByCodeQueryResult { Id = 1, UserId = userId, ActionTime = DateTime.UtcNow, FamilyName = "A", GivenNames = "B" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetUserActionByCode("CODE123");

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            ok.Value.Should().BeEquivalentTo(expected);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_NotFound_Then_ReturnsNotFound()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetUserActionByCodeQueryResult?)null);

            // Act
            var result = await _sut.GetUserActionByCode("UNKNOWN");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnBadRequestWithErrors()
        {
            // Arrange
            var validationException = new ValidationException("Validation failed");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await _sut.GetUserActionByCode("BAD");

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.GetUserActionByCode("ERR");

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
