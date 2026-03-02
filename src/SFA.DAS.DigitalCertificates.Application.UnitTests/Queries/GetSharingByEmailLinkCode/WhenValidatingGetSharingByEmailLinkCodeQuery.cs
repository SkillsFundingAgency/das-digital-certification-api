using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByEmailLinkCode
{
    [TestFixture]
    public class WhenValidatingGetSharingByEmailLinkCodeQuery
    {
        private GetSharingByEmailLinkCodeQueryValidator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new GetSharingByEmailLinkCodeQueryValidator();
        }

        [Test]
        public void And_EmailLinkCodeIsEmpty_Then_ValidationFails()
        {
            var query = new GetSharingByEmailLinkCodeQuery { EmailLinkCode = Guid.Empty };

            var result = _sut.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByEmailLinkCodeQuery.EmailLinkCode));
        }

        [Test]
        public void And_EmailLinkCodeIsValid_Then_ValidationPasses()
        {
            var query = new GetSharingByEmailLinkCodeQuery { EmailLinkCode = Guid.NewGuid() };

            var result = _sut.Validate(query);

            result.IsValid.Should().BeTrue();
        }
    }
}
