using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAuthorisation
{
    public class WhenValidatingCreateUserAuthorisationCommand
    {
        private CreateUserAuthorisationCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateUserAuthorisationCommandValidator();
        }

        [Test]
        public void And_AllRequiredFieldsProvided_Then_IsValid()
        {
            var command = new CreateUserAuthorisationCommand
            {
                UserId = Guid.NewGuid(),
                Uln = 1234567890
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_UlnIsZero_Then_ErrorReturned()
        {
            var command = new CreateUserAuthorisationCommand
            {
                UserId = Guid.NewGuid(),
                Uln = 0
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Uln");
        }

        [Test]
        public void And_UserIdEmpty_Then_ErrorReturned()
        {
            var command = new CreateUserAuthorisationCommand
            {
                UserId = default,
                Uln = 1234567890
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
        }
    }
}
