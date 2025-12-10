using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenCreatingGetSharingByIdQuery
    {
        [Test]
        public void Then_SetsPropertiesCorrectly()
        {
            var sharingId = Guid.NewGuid();
            var limit = 10;

            var query = new GetSharingByIdQuery
            {
                SharingId = sharingId,
                Limit = limit
            };

            query.SharingId.Should().Be(sharingId);
            query.Limit.Should().Be(limit);
        }

        [Test]
        public void Then_CanCreateWithoutLimit()
        {
            var sharingId = Guid.NewGuid();

            var query = new GetSharingByIdQuery
            {
                SharingId = sharingId
            };

            query.SharingId.Should().Be(sharingId);
            query.Limit.Should().BeNull();
        }
    }
}