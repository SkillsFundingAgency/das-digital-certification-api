using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Application.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmail
{
    public class WhenHandlingCreateSharingEmailCommandHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<ISharingEmailEntityContext> _sharingEmailContextMock = null!;
        private Mock<IDateTimeHelper> _dateTimeHelperMock = null!;
        private CreateSharingEmailCommandHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _sharingEmailContextMock = new Mock<ISharingEmailEntityContext>();
            _dateTimeHelperMock = new Mock<IDateTimeHelper>();

            _sut = new CreateSharingEmailCommandHandler(_sharingContextMock.Object, _sharingEmailContextMock.Object, _dateTimeHelperMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNull()
        {
            // Arrange
            var command = new CreateSharingEmailCommand { SharingId = Guid.NewGuid(), EmailAddress = "x@x.com" };
            _sharingContextMock.Setup(x => x.GetSharingById(It.IsAny<Guid>())).ReturnsAsync((Sharing?)null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _sharingEmailContextMock.Verify(x => x.Add(It.IsAny<SharingEmail>()), Times.Never);
        }

        [Test]
        public async Task And_SharingExists_Then_AddsAndSavesAndReturnsResponse()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var expiry = DateTime.UtcNow.AddDays(1);
            var sharing = new Sharing { Id = sharingId, CourseName = "Test Course", ExpiryTime = expiry };
            var now = DateTime.UtcNow;
            _sharingContextMock.Setup(x => x.GetSharingById(sharingId)).ReturnsAsync(sharing);
            _dateTimeHelperMock.Setup(d => d.Now).Returns(now);

            _sharingEmailContextMock
                .Setup(x => x.Add(It.IsAny<SharingEmail>()))
                .Callback<SharingEmail>(se => se.Id = Guid.NewGuid())
                .Returns((EntityEntry<SharingEmail>)null!);

            _sharingEmailContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().NotBeEmpty();
            result.EmailLinkCode.Should().NotBeEmpty();
            _sharingEmailContextMock.Verify(x => x.Add(It.Is<SharingEmail>(se => se.SharingId == sharingId && se.EmailAddress == command.EmailAddress)), Times.Once);
            _sharingEmailContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void And_SaveChangesFails_Then_ThrowsException()
        {
            // Arrange
            var sharingId = Guid.NewGuid();
            var sharing = new Sharing { Id = sharingId, CourseName = "Test Course" };
            _sharingContextMock.Setup(x => x.GetSharingById(sharingId)).ReturnsAsync(sharing);
            _sharingEmailContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));
            var command = new CreateSharingEmailCommand { SharingId = sharingId, EmailAddress = "test@example.com" };

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
