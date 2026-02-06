using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByEmailLinkCode
{
    [TestFixture]
    public class WhenCreatingGetSharingByEmailLinkCodeQuery
    {
        [Test]
        public void And_PropertySet_Then_ValueIsMapped()
        {
            var emailLinkCode = Guid.NewGuid();

            var query = new GetSharingByEmailLinkCodeQuery { EmailLinkCode = emailLinkCode };

            query.EmailLinkCode.Should().Be(emailLinkCode);
        }
    }
}
