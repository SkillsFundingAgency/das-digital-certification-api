using System;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserById
{
    public class GetUserByIdQueryValidatorTests
    {
        private GetUserByIdQueryValidator _validator = null!;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetUserByIdQueryValidator();
        }

        [Test]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var query = new GetUserByIdQuery { UserId = Guid.Empty };

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Test]
        public void Should_Not_Have_Error_When_UserId_Is_Provided()
        {
            var query = new GetUserByIdQuery { UserId = Guid.NewGuid() };

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
