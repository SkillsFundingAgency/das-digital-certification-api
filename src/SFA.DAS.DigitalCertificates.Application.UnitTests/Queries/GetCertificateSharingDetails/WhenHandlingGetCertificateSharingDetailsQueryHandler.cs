using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateSharingDetails;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using UserEntity = SFA.DAS.DigitalCertificates.Domain.Entities.User;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetCertificateSharingDetails
{
    [TestFixture]
    public class WhenHandlingGetCertificateSharingDetailsQueryHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IUserEntityContext> _userContextMock = null!;
        private GetCertificateSharingDetailsQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _userContextMock = new Mock<IUserEntityContext>();
            _handler = new GetCertificateSharingDetailsQueryHandler(_sharingContextMock.Object, _userContextMock.Object);
        }

        [Test]
        public async Task And_UserNotFound_Then_ReturnsNullSharingDetails()
        {
            var query = new GetCertificateSharingDetailsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.NewGuid() };
            _userContextMock.Setup(x => x.GetByUserId(query.UserId)).ReturnsAsync((UserEntity?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.SharingDetails.Should().BeNull();
        }

        [Test]
        public async Task And_NoSharings_Then_ReturnsEmptySharings()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var user = new UserEntity { Id = userId, GovUkIdentifier = "abc", EmailAddress = "test@test.com" };
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync(user);
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(new List<Sharing>());

            var query = new GetCertificateSharingDetailsQuery { UserId = userId, CertificateId = certId };
            var result = await _handler.Handle(query, CancellationToken.None);

            result.SharingDetails.Should().NotBeNull();
            result.SharingDetails!.Sharings.Should().BeEmpty();
        }

        [Test]
        public async Task And_LiveSharingsExist_Then_ReturnsOnlyLiveSharings()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var user = new UserEntity { Id = userId, GovUkIdentifier = "abc", EmailAddress = "test@test.com" };
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync(user);
            var sharings = new List<Sharing>
            {
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = "TypeA", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-2), ExpiryTime = DateTime.UtcNow.AddDays(1), Status = SharingStatus.Live.ToString() },
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = "TypeA", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-1), ExpiryTime = DateTime.UtcNow.AddDays(2), Status = "Deleted" }
            };
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(sharings);

            var query = new GetCertificateSharingDetailsQuery { UserId = userId, CertificateId = certId };
            var result = await _handler.Handle(query, CancellationToken.None);

            result.SharingDetails!.Sharings.Should().HaveCount(1);
            result.SharingDetails!.Sharings.First().SharingId.Should().Be(sharings[0].Id);
        }

        [Test]
        public async Task And_LimitIsApplied_Then_ReturnsLimitedSharings()
        {
            var userId = Guid.NewGuid();
            var certId = Guid.NewGuid();
            var user = new UserEntity { Id = userId, GovUkIdentifier = "abc", EmailAddress = "test@test.com" };
            _userContextMock.Setup(x => x.GetByUserId(userId)).ReturnsAsync(user);
            var sharings = new List<Sharing>
            {
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = "TypeA", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-2), ExpiryTime = DateTime.UtcNow.AddDays(1), Status = SharingStatus.Live.ToString() },
                new Sharing { Id = Guid.NewGuid(), UserId = userId, CertificateId = certId, CertificateType = "TypeA", LinkCode = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-1), ExpiryTime = DateTime.UtcNow.AddDays(2), Status = SharingStatus.Live.ToString() }
            };
            _sharingContextMock.Setup(x => x.GetAllSharings(userId, certId)).ReturnsAsync(sharings);

            var query = new GetCertificateSharingDetailsQuery { UserId = userId, CertificateId = certId, Limit = 1 };
            var result = await _handler.Handle(query, CancellationToken.None);

            result.SharingDetails!.Sharings.Should().HaveCount(1);
        }
    }
}
