using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateCertificateSharing
{
    public class WhenHandlingCreateCertificateSharingCommandHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateCertificateSharingCommandHandler _sut = null!;
        private ApplicationSettings _settings = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _settings = new ApplicationSettings { CertificateSharingExpiryDays = 14 };
            var options = Options.Create(_settings);
            _sut = new CreateCertificateSharingCommandHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object, options);
        }

        [Test]
        public async Task And_NoExistingSharings_Then_CreatesSharingWithCorrectProperties()
        {
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(0);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            var result = await _sut.Handle(command, CancellationToken.None);

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
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(1);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            var result = await _sut.Handle(command, CancellationToken.None);

            result.SharingNumber.Should().Be(2);
        }

        [Test]
        public void And_SaveChangesFails_Then_ThrowsException()
        {
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(x => x.Now).Returns(now);
            _sharingContextMock.Setup(x => x.GetSharingsCount(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(0);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}