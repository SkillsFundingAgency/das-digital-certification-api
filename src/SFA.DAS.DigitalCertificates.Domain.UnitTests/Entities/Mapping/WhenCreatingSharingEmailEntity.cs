using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingSharingEmailEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var sharingId = Guid.NewGuid();
            var email = "test@example.com";
            var emailLinkCode = Guid.NewGuid();
            var sentTime = DateTime.UtcNow;

            var sharingEmail = new SharingEmail
            {
                Id = id,
                SharingId = sharingId,
                EmailAddress = email,
                EmailLinkCode = emailLinkCode,
                SentTime = sentTime,
                Sharing = new Sharing { Id = sharingId, CertificateType = CertificateType.Standard, CourseName = "CourseName", Status = SharingStatus.Live },
                SharingEmailAccesses = new List<SharingEmailAccess>()
            };

            sharingEmail.Id.Should().Be(id);
            sharingEmail.SharingId.Should().Be(sharingId);
            sharingEmail.EmailAddress.Should().Be(email);
            sharingEmail.EmailLinkCode.Should().Be(emailLinkCode);
            sharingEmail.SentTime.Should().Be(sentTime);
            sharingEmail.Sharing.Should().NotBeNull();
            sharingEmail.Sharing.Id.Should().Be(sharingId);
            sharingEmail.SharingEmailAccesses.Should().NotBeNull();
        }
    }
}
