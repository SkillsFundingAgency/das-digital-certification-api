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
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private GetSharingsQueryHandler _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(d => d.Now).Returns(DateTime.UtcNow);

            _sut = new GetSharingsQueryHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_NoSharings_Then_ReturnsEmptySharingDetails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(new List<Sharing>());

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.SharingDetails.Should().NotBeNull();
            result.SharingDetails!.Sharings.Should().BeEmpty();
            result.SharingDetails.CertificateType.Should().Be(CertificateType.Unknown);
            result.SharingDetails.CourseName.Should().Be(string.Empty);
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
        public async Task And_ExpiredSharings_Are_Excluded()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(now);

            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();

            var expired = new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = now.AddDays(-10), ExpiryTime = now.AddDays(-1), Status = SharingStatus.Live };
            var live = new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = CertificateType.Standard, CourseName = "CourseName", LinkCode = Guid.NewGuid(), CreatedAt = now.AddDays(-2), ExpiryTime = now.AddDays(1), Status = SharingStatus.Live };

            var sharings = new List<Sharing> { expired, live };
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(sharings);

            // Act
            var query = new GetSharingsQuery { UserId = userId, CertificateId = certId };
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.SharingDetails.Should().NotBeNull();
            result.SharingDetails!.Sharings.Should().HaveCount(1);
            result.SharingDetails!.Sharings.First().SharingId.Should().Be(live.Id);
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
