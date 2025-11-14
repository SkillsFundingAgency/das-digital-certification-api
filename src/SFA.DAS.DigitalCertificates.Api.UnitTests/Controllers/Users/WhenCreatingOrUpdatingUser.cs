using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingOrUpdatingUser
    {
        [Test, MoqAutoData]
        public async Task And_CommandIsSuccessful_Then_ReturnOk(
            CreateOrUpdateUserRequest request,
            CreateOrUpdateUserCommandResponse commandResponse,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<CreateOrUpdateUserCommand>(c =>
                        c.GovUkIdentifier == request.GovUkIdentifier &&
                        c.EmailAddress == request.EmailAddress),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResponse);

            // Act
            var result = await controller.CreateOrUpdateUser(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(commandResponse);
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors(
            CreateOrUpdateUserRequest request,
            ValidationException validationException,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);

            // Act
            var result = await controller.CreateOrUpdateUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_CommandThrowsException_Then_ReturnBadRequest(
            CreateOrUpdateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await controller.CreateOrUpdateUser(request);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
