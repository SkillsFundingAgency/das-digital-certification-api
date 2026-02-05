using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByLinkCode
{
    [TestFixture]
    public class WhenHandlingGetSharingByLinkCodeQueryHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private GetSharingByLinkCodeQueryHandler _sut = null!;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(_now);
            _sut = new GetSharingByLinkCodeQueryHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNullSharing()
        {
            var linkCode = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetSharingByLinkCode(linkCode, _now)).ReturnsAsync((Sharing?)null);

            var query = new GetSharingByLinkCodeQuery { LinkCode = linkCode };

            var result = await _sut.Handle(query, CancellationToken.None);

            result.Sharing.Should().BeNull();
        }

        [Test]
        public async Task And_SharingExists_Then_ReturnsSummaryWithCorrectData()
        {
            var linkCode = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var expiry = DateTime.UtcNow.AddHours(2);

            var sharing = new Sharing
            {
                Id = sharingId,
                CertificateId = certificateId,
                CourseName = "Test Course",
                CertificateType = Enums.CertificateType.Framework,
                LinkCode = linkCode,
                ExpiryTime = expiry,
                Status = Enums.SharingStatus.Live
            };

            _sharingContextMock.Setup(x => x.GetSharingByLinkCode(linkCode, _now)).ReturnsAsync(sharing);

            var query = new GetSharingByLinkCodeQuery { LinkCode = linkCode };

            var result = await _sut.Handle(query, CancellationToken.None);

            result.Sharing.Should().NotBeNull();
            result.Sharing!.SharingId.Should().Be(sharingId);
            result.Sharing.CertificateId.Should().Be(certificateId);
            result.Sharing.ExpiryTime.Should().Be(expiry);
            result.Sharing.CertificateType.Should().Be(Enums.CertificateType.Framework);
        }
    }
}
