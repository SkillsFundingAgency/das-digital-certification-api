using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenValidatingGetSharingByIdQuery
    {
        private GetSharingByIdQueryValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new GetSharingByIdQueryValidator();
        }

        [Test]
        public void And_SharingIdIsEmpty_Then_ValidationFails()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.Empty };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.SharingId));
        }

        [Test]
        public void And_SharingIdIsValid_Then_ValidationPasses()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid() };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_LimitIsZero_Then_ValidationFails()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = 0 };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.Limit));
        }

        [Test]
        public void And_LimitIsNegative_Then_ValidationFails()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = -1 };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.Limit));
        }

        [Test]
        public void And_LimitIsPositive_Then_ValidationPasses()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = 10 };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_LimitIsNull_Then_ValidationPasses()
        {
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = null };
            var result = _validator.Validate(query);

            result.IsValid.Should().BeTrue();
        }
    }
}