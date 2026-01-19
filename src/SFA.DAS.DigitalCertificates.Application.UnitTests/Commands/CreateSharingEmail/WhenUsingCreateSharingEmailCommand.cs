using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmail
{
    public class WhenUsingCreateSharingEmailCommand
    {
        [Test]
        public void Command_Should_Allow_Property_Assignment()
        {
            var cmd = new CreateSharingEmailCommand
            {
                SharingId = Guid.NewGuid(),
                EmailAddress = "test@example.com"
            };

            cmd.SharingId.Should().NotBeEmpty();
            cmd.EmailAddress.Should().Be("test@example.com");
        }
    }
}
