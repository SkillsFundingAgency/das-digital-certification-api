using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateAdminAction
{
    public class WhenValidatingCreateAdminActionCommand
    {
        private CreateAdminActionCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateAdminActionCommandValidator();
        }

        [Test]
        public void And_AllFieldsAreCorrect_Then_CommandIsValid()
        {
            var command = new CreateAdminActionCommand
            {
                Username = "admin",
                Action = AdminActionType.Viewed,
                UserActionId = 1
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_UsernameIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateAdminActionCommand
            {
                Username = "",
                Action = AdminActionType.Viewed,
                UserActionId = 1
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
        }

        [Test]
        public void And_UserActionIdIsZero_Then_ErrorReturned()
        {
            var command = new CreateAdminActionCommand
            {
                Username = "admin",
                Action = AdminActionType.Viewed,
                UserActionId = 0
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserActionId");
        }

        [Test]
        public void And_ActionIsInvalid_Then_ErrorReturned()
        {
            var command = new CreateAdminActionCommand
            {
                Username = "admin",
                Action = (AdminActionType)999,
                UserActionId = 1
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Action");
        }
    }
}
