using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Domain.Models;
using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.UnitTests.Models
{
    public class WhenMappingFromCertificateSharingDetailsModel
    {
        [Test]
        public void ThenTheFieldsAreCorrectlyMapped()
        {
            var userId = Guid.NewGuid();
            var certificateId = Guid.NewGuid();
            var certificateType = CertificateType.Standard;
            var courseName = "TestType";
            var sharings = new List<SharingDetail>
            {
                new SharingDetail
                {
                    SharingId = Guid.NewGuid(),
                    SharingNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    LinkCode = Guid.NewGuid(),
                    ExpiryTime = DateTime.UtcNow.AddDays(1),
                    SharingAccess = new List<DateTime> { DateTime.UtcNow },
                    SharingEmails = new List<SharingEmailDetail>()
                }
            };

            var model = new CertificateSharings
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                Sharings = sharings
            };

            model.UserId.Should().Be(userId);
            model.CertificateId.Should().Be(certificateId);
            model.CertificateType.Should().Be(certificateType);
            model.Sharings.Should().BeEquivalentTo(sharings);
        }
    }
}