using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmailAccess
{
    public class WhenHandlingCreateSharingEmailAccessCommandHandler
    {
        private Mock<ISharingEmailEntityContext> _sharingEmailContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateSharingEmailAccessCommandHandler _sut = null!;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _sharingEmailContextMock = new Mock<ISharingEmailEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(_now);

            _sut = new CreateSharingEmailAccessCommandHandler(_sharingEmailContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingEmailNotFound_Then_ReturnsNull()
        {
            // Arrange
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };
            _sharingEmailContextMock.Setup(x => x.GetSharingEmailByIdTracked(command.SharingEmailId)).ReturnsAsync((SharingEmail?)null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _sharingEmailContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_SharingEmailExists_Then_AddsAndSavesAndReturnsId()
        {
            // Arrange
            var sharingEmailId = Guid.NewGuid();
            var sharingEmail = new SharingEmail { Id = sharingEmailId, EmailAddress = "test@example.com", SharingEmailAccesses = new List<SharingEmailAccess>() };
            _sharingEmailContextMock.Setup(x => x.GetSharingEmailByIdTracked(sharingEmailId)).ReturnsAsync(sharingEmail);
            _sharingEmailContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new CreateSharingEmailAccessCommand { SharingEmailId = sharingEmailId };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            sharingEmail.SharingEmailAccesses.Should().ContainSingle(sa => sa.SharingEmailId == sharingEmailId && sa.AccessedAt == _now);
            _sharingEmailContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void And_SaveChangesFails_Then_ThrowsException()
        {
            // Arrange
            var sharingEmailId = Guid.NewGuid();
            var sharingEmail = new SharingEmail { Id = sharingEmailId, EmailAddress = "test@example.com", SharingEmailAccesses = new List<SharingEmailAccess>() };
            _sharingEmailContextMock.Setup(x => x.GetSharingEmailByIdTracked(sharingEmailId)).ReturnsAsync(sharingEmail);
            _sharingEmailContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = sharingEmailId };

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
