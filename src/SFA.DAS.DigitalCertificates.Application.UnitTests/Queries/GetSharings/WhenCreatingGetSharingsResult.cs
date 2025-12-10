using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharings
{
    [TestFixture]
    public class WhenCreatingGetSharingsResult
    {
        [Test]
        public void And_DefaultConstructor_Then_SharingDetailsIsNull()
        {
            var result = new GetSharingsQueryResult();
            result.SharingDetails.Should().BeNull();
        }

        [Test]
        public void And_SharingDetailsIsSet_Then_PropertiesAreMapped()
        {
            var details = new CertificateSharings
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "TypeA",
                CourseName = "CourseName",
                Sharings = new List<SharingDetail>()
            };
            var result = new GetSharingsQueryResult { SharingDetails = details };
            result.SharingDetails.Should().Be(details);
        }
    }
}
