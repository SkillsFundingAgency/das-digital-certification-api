using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.DeleteSharing
{
    public class WhenHandlingDeleteSharingCommandHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private DeleteSharingCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _sut = new DeleteSharingCommandHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(It.IsAny<Guid>())).ReturnsAsync((Sharing?)null);
            var command = new DeleteSharingCommand { SharingId = id };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _sharingContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_SharingAlreadyDeletedOrExpired_Then_NoSaveAndReturnsResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(now);

            var sharing = new Sharing { Id = id, Status = SharingStatus.Deleted, ExpiryTime = now.AddDays(1), CourseName = "Test Course" };
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(id)).ReturnsAsync(sharing);

            var command = new DeleteSharingCommand { SharingId = id };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.SharingId.Should().Be(id);
            _sharingContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_SharingIsLive_Then_MarksDeletedAndSaves()
        {
            // Arrange
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(now);

            var sharing = new Sharing { Id = id, Status = SharingStatus.Live, ExpiryTime = now.AddDays(1), CourseName = "Test Course" };
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(id)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new DeleteSharingCommand { SharingId = id };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.SharingId.Should().Be(id);
            sharing.Status.Should().Be(SharingStatus.Deleted);
            _sharingContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
