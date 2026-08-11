using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UnlockUser
{
    public class WhenValidatingUnlockUserCommand
    {
        private UnlockUserCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new UnlockUserCommandValidator();
        }

        [Test]
        public void And_UserIdIsEmpty_Then_ValidationFails()
        {
            var command = new UnlockUserCommand { UserId = Guid.Empty };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(UnlockUserCommand.UserId));
        }

        [Test]
        public void And_UserIdIsValid_Then_ValidationPasses()
        {
            var command = new UnlockUserCommand { UserId = Guid.NewGuid() };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
