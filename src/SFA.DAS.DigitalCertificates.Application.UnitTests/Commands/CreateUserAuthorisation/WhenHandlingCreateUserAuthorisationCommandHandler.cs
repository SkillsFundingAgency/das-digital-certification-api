using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAuthorisation
{
    public class WhenHandlingCreateUserAuthorisationCommandHandler
    {
        private Mock<IUserAuthorisationEntityContext> _authContextMock = null!;
        private Mock<IUserEntityContext> _userContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateUserAuthorisationCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _authContextMock = new Mock<IUserAuthorisationEntityContext>();
            _userContextMock = new Mock<IUserEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _sut = new CreateUserAuthorisationCommandHandler(
                _authContextMock.Object,
                _userContextMock.Object,
                _dateTimeProviderMock.Object);
        }

        [Test]
        public void And_UserNotFound_Then_ValidationExceptionThrown()
        {
            // Arrange
            var command = new CreateUserAuthorisationCommand { UserId = Guid.NewGuid(), Uln = 1 };

            _userContextMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(command.UserId)).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>().Result.And.Errors.Should().Contain(e => e.PropertyName == nameof(command.UserId));
        }

        [Test]
        public void And_UlnAlreadyExists_Then_ValidationExceptionThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserAuthorisationCommand { UserId = userId, Uln = 1 };
            var user = new User { Id = userId, GovUkIdentifier = "GOV1", EmailAddress = "test@example.com" };

            _userContextMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);
            _authContextMock.Setup(x => x.GetByUlnAsync(command.Uln, It.IsAny<CancellationToken>())).ReturnsAsync(new UserAuthorisation());

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>().Result.And.Errors.Should().Contain(e => e.PropertyName == nameof(command.Uln));
        }

        [Test]
        public void And_UserAlreadyHasAuthorisation_Then_ValidationExceptionThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserAuthorisationCommand { UserId = userId, Uln = 1 };
            var user = new User { Id = userId, GovUkIdentifier = "GOV2", EmailAddress = "test2@example.com", UserAuthorisation = new UserAuthorisation() };

            _userContextMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);
            _authContextMock.Setup(x => x.GetByUlnAsync(command.Uln, It.IsAny<CancellationToken>())).ReturnsAsync((UserAuthorisation?)null);

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>().Result.And.Errors.Should().Contain(e => e.PropertyName == nameof(command.UserId));
        }

        [Test]
        public async Task And_AllGood_Then_UserAuthorisationSaved_And_IdentitiesRemoved()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(now);

            var userId = Guid.NewGuid();
            var command = new CreateUserAuthorisationCommand { UserId = userId, Uln = 1234567890 };

            var identities = new List<UserIdentity>
            {
                new UserIdentity { Id = Guid.NewGuid(), FamilyName = "Smith", GivenNames = "John", DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) }
            };

            var user = new User { Id = userId, GovUkIdentifier = "GOV3", EmailAddress = "test3@example.com", UserIdentities = identities };

            _userContextMock.Setup(x => x.GetWithIdentitiesAndAuthorisation(userId)).ReturnsAsync(user);
            _authContextMock.Setup(x => x.GetByUlnAsync(command.Uln, It.IsAny<CancellationToken>())).ReturnsAsync((UserAuthorisation?)null);
            _userContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _authContextMock.Verify(x => x.Add(It.Is<UserAuthorisation>(a => a.UserId == userId && a.ULN == command.Uln && a.AuthorisedAt == now)), Times.Once);
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
