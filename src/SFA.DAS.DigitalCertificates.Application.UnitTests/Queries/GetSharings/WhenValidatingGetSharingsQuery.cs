using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharings
{
    [TestFixture]
    public class WhenValidatingGetSharingsQuery
    {
        [Test]
        public void And_UserIdIsEmpty_Then_ValidationFails()
        {
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.Empty, CertificateId = Guid.NewGuid() };
            var result = validator.Validate(query);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
        }

        [Test]
        public void And_CertificateIdIsEmpty_Then_ValidationFails()
        {
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.Empty };
            var result = validator.Validate(query);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateId");
        }

        [Test]
        public void And_LimitIsZeroOrNegative_Then_ValidationFails()
        {
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.NewGuid(), Limit = 0 };
            var result = validator.Validate(query);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Limit");
        }

        [Test]
        public void And_ValidQuery_Then_ValidationSucceeds()
        {
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.NewGuid(), Limit = 1 };
            var result = validator.Validate(query);
            result.IsValid.Should().BeTrue();
        }
    }
}
