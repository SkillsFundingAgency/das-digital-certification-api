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
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserById
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
        public async Task And_ValidRequest_Then_ReturnOkWithResult()
        {
            var userId = Guid.NewGuid();

            var expected = new GetUserByIdQueryResult
            {
                UserId = userId,
                GovUkIdentifier = "G1",
                EmailAddress = "a@b.com",
                PhoneNumber = "01234",
                IsLocked = false
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _sut.GetUserById(userId);

            var ok = result.Should().BeOfType<OkObjectResult>().Which;
            ok.Value.Should().BeEquivalentTo(expected);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task And_ValidationException_Then_ReturnNotFound()
        {
            var userId = Guid.NewGuid();
            var validationException = new ValidationException("Validation failed");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(validationException);

            var result = await _sut.GetUserById(userId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task And_Exception_Then_ReturnInternalServerError()
        {
            var userId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _sut.GetUserById(userId);

            result.Should().BeOfType<StatusCodeResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
