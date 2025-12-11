using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharings
{
    [TestFixture]
    public class WhenHandlingGetSharingsQueryHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private GetSharingsQueryHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _sut = new GetSharingsQueryHandler(_sharingContextMock.Object);
        }

        [Test]
        public async Task And_NoSharings_Then_ReturnsSharingDetailsAsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(new List<Sharing>());

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.SharingDetails.Should().BeNull();
        }

        [Test]
        public async Task And_LiveSharingsExist_Then_ReturnsOnlyLiveSharings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var sharings = new List<Sharing>
            {
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-2), ExpiryTime = DateTime.UtcNow.AddDays(1), Status = SharingStatus.Live },
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-1), ExpiryTime = DateTime.UtcNow.AddDays(2), Status = SharingStatus.Deleted }
            };
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(sharings);

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.SharingDetails.Should().NotBeNull();
            result.SharingDetails!.Sharings.Should().HaveCount(1);
            result.SharingDetails!.Sharings.First().SharingId.Should().Be(sharings[0].Id);
        }

        [Test]
        public async Task And_LimitIsApplied_Then_ReturnsLimitedSharings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var sharings = new List<Sharing>
            {
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-2), ExpiryTime = DateTime.UtcNow.AddDays(1), Status = SharingStatus.Live },
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-1), ExpiryTime = DateTime.UtcNow.AddDays(2), Status = SharingStatus.Live }
            };
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(sharings);

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId, Limit = 1 };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.SharingDetails.Should().NotBeNull();
            result.SharingDetails!.Sharings.Should().HaveCount(1);
        }
    }
}
