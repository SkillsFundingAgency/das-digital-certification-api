using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateAdminAction
{
    public class WhenCreatingCreateAdminActionCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var command = new CreateAdminActionCommand
            {
                Username = "admin",
                Action = AdminActionType.Viewed,
                UserActionId = 123
            };

            command.Username.Should().Be("admin");
            command.Action.Should().Be(AdminActionType.Viewed);
            command.UserActionId.Should().Be(123);
        }
    }
}
