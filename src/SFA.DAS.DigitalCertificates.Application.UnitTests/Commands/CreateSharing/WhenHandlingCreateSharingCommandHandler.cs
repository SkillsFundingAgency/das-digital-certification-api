using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Application.Extensions;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharing
{
    public class WhenHandlingCreateSharingCommandHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeHelper> _dateTimeProviderMock = null!;
        private CreateSharingCommandHandler _sut = null!;
        private ApplicationSettings _settings = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeHelper>();
            _settings = new ApplicationSettings { CertificateSharingExpiryDays = 14 };
            var options = Options.Create(_settings);
            _sut = new CreateSharingCommandHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object, options);
        }

        [Test]
        public async Task And_NoPriorSharings_Then_CreatesSharingWithCorrectProperties()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(1);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.UserId.Should().Be(command.UserId);
            result.CertificateId.Should().Be(command.CertificateId);
            result.CertificateType.Should().Be(command.CertificateType);
            result.CourseName.Should().Be(command.CourseName);
            result.CreatedAt.Should().Be(now);
            result.ExpiryTime.Should().Be(now.AddDays(_settings.CertificateSharingExpiryDays));
            result.SharingNumber.Should().Be(1);
        }

        [Test]
        public async Task And_ExistingSharings_Then_IncrementsSharingNumberCorrectly()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(2);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Framework,
                CourseName = "Test Course"
            };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.SharingNumber.Should().Be(2);
        }

        [Test]
        public void And_SaveChangesFails_Then_ThrowsException()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act & Assert
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }

        [Test]
        public void And_GetSharingsCountFails_After_Save_Then_ThrowsException()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new Exception("Count error"));
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act & Assert
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<Exception>().WithMessage("Count error");
        }
    }
}