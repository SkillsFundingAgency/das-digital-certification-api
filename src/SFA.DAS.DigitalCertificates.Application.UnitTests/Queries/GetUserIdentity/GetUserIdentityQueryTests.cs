using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryTests
    {
        [Test]
        public void Can_Create_Query()
        {
            var id = Guid.NewGuid();

            var q = new GetUserIdentityQuery { UserId = id };

            q.Should().NotBeNull();
            q.UserId.Should().Be(id);
        }
    }
}
