using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByEmailLinkCode
{
    [TestFixture]
    public class WhenHandlingGetSharingByEmailLinkCodeQueryHandler
    {
        private Mock<ISharingEntityContext> _sharingContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private GetSharingByEmailLinkCodeQueryHandler _sut = null!;
        private DateTime _now;

        [SetUp]
        public void SetUp()
        {
            _sharingContextMock = new Mock<ISharingEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _now = DateTime.UtcNow;
            _dateTimeProviderMock.Setup(d => d.Now).Returns(_now);
            _sut = new GetSharingByEmailLinkCodeQueryHandler(_sharingContextMock.Object, _dateTimeProviderMock.Object);
        }

        [Test]
        public async Task And_SharingNotFound_Then_ReturnsNullSharing()
        {
            var emailLinkCode = Guid.NewGuid();
            _sharingContextMock.Setup(x => x.GetSharingByEmailLinkCode(emailLinkCode, _now)).ReturnsAsync((Sharing?)null);

            var query = new GetSharingByEmailLinkCodeQuery { EmailLinkCode = emailLinkCode };

            var result = await _sut.Handle(query, CancellationToken.None);

            result.SharingEmail.Should().BeNull();
        }

        [Test]
        public async Task And_SharingExists_Then_ReturnsSummaryWithCorrectData()
        {
            var emailLinkCode = Guid.NewGuid();
            var sharingEmailId = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var expiry = DateTime.UtcNow.AddHours(2);

            var sharingEmail = new SharingEmail
            {
                Id = sharingEmailId,
                EmailLinkCode = emailLinkCode,
                SentTime = DateTime.UtcNow,
                EmailAddress = "test@example.com"
            };

            var sharing = new Sharing
            {
                Id = sharingId,
                CertificateId = certificateId,
                CertificateType = Enums.CertificateType.Standard,
                LinkCode = Guid.NewGuid(),
                ExpiryTime = expiry,
                Status = Enums.SharingStatus.Live,
                CourseName = "Test Course",
                SharingEmails = new List<SharingEmail> { sharingEmail }
            };

            _sharingContextMock.Setup(x => x.GetSharingByEmailLinkCode(emailLinkCode, _now)).ReturnsAsync(sharing);

            var query = new GetSharingByEmailLinkCodeQuery { EmailLinkCode = emailLinkCode };

            var result = await _sut.Handle(query, CancellationToken.None);

            result.SharingEmail.Should().NotBeNull();
            result.SharingEmail!.SharingEmailId.Should().Be(sharingEmailId);
            result.SharingEmail.CertificateId.Should().Be(certificateId);
            result.SharingEmail.ExpiryTime.Should().Be(expiry);
            result.SharingEmail.CertificateType.Should().Be(Enums.CertificateType.Standard);
        }
    }
}
