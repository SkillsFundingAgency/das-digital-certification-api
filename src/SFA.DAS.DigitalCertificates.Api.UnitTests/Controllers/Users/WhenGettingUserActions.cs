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
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserActions
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
        public async Task And_ValidRequest_Then_ReturnOkWithUserActions()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var actions = new List<UserActionDetail>
            {
                new UserActionDetail { Id = 1, UserId = userId, ActionType = ActionType.Reprint, FamilyName = "A", GivenNames = "B", ActionTime = DateTime.UtcNow, Uln = 12345678 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetUserActionsQueryResult { UserActions = actions });

            // Act
            var result = await _sut.GetUserActions(userId);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            ok.Value.Should().NotBeNull();

            var useractionsProp = ok.Value.GetType().GetProperty("userActions");
            useractionsProp.Should().NotBeNull();
            var value = useractionsProp.GetValue(ok.Value) as IEnumerable<UserActionDetail>;
            value.Should().BeEquivalentTo(actions);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserActionsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnBadRequestWithErrors()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var validationException = new ValidationException("Validation failed");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            // Act
            var result = await _sut.GetUserActions(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserActionsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.GetUserActions(userId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
