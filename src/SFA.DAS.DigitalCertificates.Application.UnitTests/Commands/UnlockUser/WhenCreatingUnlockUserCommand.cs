using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UnlockUser
{
    public class WhenCreatingUnlockUserCommand
    {
        [Test]
        public void Response_Should_Allow_UserId_Assignment()
        {
            var id = Guid.NewGuid();
            var cmd = new UnlockUserCommand { UserId = id };

            cmd.Should().NotBeNull();
            cmd.UserId.Should().Be(id);
        }
    }
}
