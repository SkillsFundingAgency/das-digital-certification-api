using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserMatch
{
    public class WhenHandlingCreateUserMatchCommandHandler
    {
        private Mock<IUserMatchEntityContext> _userMatchContextMock = null!;
        private Mock<IUserEntityContext> _userContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null;
        private CreateUserMatchCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _userMatchContextMock = new Mock<IUserMatchEntityContext>();
            _userContextMock = new Mock<IUserEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _sut = new CreateUserMatchCommandHandler(_userMatchContextMock.Object, _userContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_IsFailedFalse_Then_UserMatchIsSaved_And_UserIsNotLocked()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com", IsLocked = false };
            var command = new CreateUserMatchCommand
            {
                UserId = userId,
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990, 1, 1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                CourseLevel = "2",
                YearAwarded = 2020,
                ProviderName = "Provider",
                Ukprn = 123456,
                IsMatched = true,
                IsFailed = false
            };

            _userContextMock
                .Setup(x => x.GetByUserId(userId))
                .ReturnsAsync(user);

            _userContextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _userMatchContextMock.Verify(x => x.Add(It.Is<UserMatch>(m =>
                m.UserId == userId &&
                m.FamilyName == "Smith" &&
                m.IsMatched == true &&
                m.IsFailed == false)),
                Times.Once);

            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _userContextMock.Verify(x => x.GetByUserId(userId), Times.Once);
            user.IsLocked.Should().BeFalse();
        }

        [Test]
        public async Task And_IsFailedTrue_And_UserExists_Then_UserMatchIsSaved_And_UserIsLocked()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, GovUkIdentifier = "GOV123", EmailAddress = "test@example.com", IsLocked = false };

            var command = new CreateUserMatchCommand
            {
                UserId = userId,
                Uln = 1234567890,
                FamilyName = "Jones",
                DateOfBirth = new DateTime(1990, 1, 1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                CourseLevel = "2",
                YearAwarded = 2020,
                ProviderName = "Provider",
                Ukprn = 123456,
                IsMatched = false,
                IsFailed = true
            };

            _userContextMock
                .Setup(x => x.GetByUserId(userId))
                .ReturnsAsync(user);

            _userContextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _userMatchContextMock.Verify(x => x.Add(It.Is<UserMatch>(m =>
                m.UserId == userId &&
                m.FamilyName == "Jones" &&
                m.IsFailed == true)),
                Times.Once);

            _userContextMock.Verify(x => x.GetByUserId(userId), Times.Once);
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            user.IsLocked.Should().BeTrue();
        }

        [Test]
        public async Task And_UserDoesNotExist_Then_NoMatchCreated_And_NoSaveCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateUserMatchCommand
            {
                UserId = userId,
                Uln = 1234567890,
                FamilyName = "NoUser",
                DateOfBirth = new DateTime(1990, 1, 1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                CourseLevel = "2",
                YearAwarded = 2020,
                ProviderName = "Provider",
                Ukprn = 123456,
                IsMatched = false,
                IsFailed = false
            };

            _userContextMock
                .Setup(x => x.GetByUserId(userId))
                .ReturnsAsync((User?)null);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _userMatchContextMock.Verify(x => x.Add(It.IsAny<UserMatch>()), Times.Never);
            _userContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _userContextMock.Verify(x => x.GetByUserId(userId), Times.Once);
        }

    }
}
