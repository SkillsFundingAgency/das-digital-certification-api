using System;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryValidatorTests
    {
        private GetUserIdentityQueryValidator _validator = null!;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetUserIdentityQueryValidator();
        }

        [Test]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var query = new GetUserIdentityQuery { UserId = Guid.Empty };

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Test]
        public void Should_Not_Have_Error_When_UserId_Is_Provided()
        {
            var query = new GetUserIdentityQuery { UserId = Guid.NewGuid() };

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
