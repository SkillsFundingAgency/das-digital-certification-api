using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmailAccess
{
    public class WhenCreatingSharingEmailAccessCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var sharingEmailId = Guid.NewGuid();

            var command = new CreateSharingEmailAccessCommand
            {
                SharingEmailId = sharingEmailId
            };

            command.SharingEmailId.Should().Be(sharingEmailId);
        }
    }
}
