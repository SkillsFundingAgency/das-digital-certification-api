using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingAccess
{
    public class WhenHandlingCreateSharingAccessCommandHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateSharingAccessCommandHandler _sut = null!;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(_now);

            _sut = new CreateSharingAccessCommandHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNull()
        {
            // Arrange
            var command = new CreateSharingAccessCommand { SharingId = Guid.NewGuid() };
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(command.SharingId)).ReturnsAsync((Sharing?)null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _sharingContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task And_SharingExists_Then_AddsAndSavesAndReturnsId()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var sharing = new Sharing { Id = sharingId, CourseName = "Test Course", SharingAccesses = new List<SharingAccess>() };
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(sharingId)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new CreateSharingAccessCommand { SharingId = sharingId };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            sharing.SharingAccesses.Should().ContainSingle(sa => sa.SharingId == sharingId && sa.AccessedAt == _now);
            _sharingContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void And_SaveChangesFails_Then_ThrowsException()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var sharing = new Sharing { Id = sharingId, CourseName = "Test Course", SharingAccesses = new List<SharingAccess>() };
            _sharingContextMock.Setup(x => x.GetSharingByIdTracked(sharingId)).ReturnsAsync(sharing);
            _sharingContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));
            var command = new CreateSharingAccessCommand { SharingId = sharingId };

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
