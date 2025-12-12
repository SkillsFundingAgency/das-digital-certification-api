using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserAuthorisation
    {
        [Test, MoqAutoData]
        public async Task And_QueryIsSuccessful_Then_ReturnOk(
            Guid userId,
            GetUserAuthorisationQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetUserAuthorisationQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await controller.GetUserAuthorisation(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(queryResult);
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors(
            Guid userId,
            ValidationException validationException,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetUserAuthorisationQuery>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);

            // Act
            var result = await controller.GetUserAuthorisation(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_QueryThrowsException_Then_Return500Result(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetUserAuthorisationQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await controller.GetUserAuthorisation(userId);

            // Assert
            var statusCodeResult = result as StatusCodeResult;

            statusCodeResult.Should().NotBeNull();
            statusCodeResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
