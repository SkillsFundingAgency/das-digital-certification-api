using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingByLinkCode
{
    [TestFixture]
    public class WhenValidatingGetSharingByLinkCodeQuery
    {
        private GetSharingByLinkCodeQueryValidator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new GetSharingByLinkCodeQueryValidator();
        }

        [Test]
        public void And_LinkCodeIsEmpty_Then_ValidationFails()
        {
            var query = new GetSharingByLinkCodeQuery { LinkCode = Guid.Empty };

            var result = _sut.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByLinkCodeQuery.LinkCode));
        }

        [Test]
        public void And_LinkCodeIsValid_Then_ValidationPasses()
        {
            var query = new GetSharingByLinkCodeQuery { LinkCode = Guid.NewGuid() };

            var result = _sut.Validate(query);

            result.IsValid.Should().BeTrue();
        }
    }
}
