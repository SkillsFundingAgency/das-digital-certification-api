using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingAccess
{
    public class WhenCreatingSharingAccessCommand
    {
        [Test]
        public void And_PropertiesAreSet_Then_CommandIsCorrect()
        {
            var sharingId = Guid.NewGuid();

            var command = new CreateSharingAccessCommand
            {
                SharingId = sharingId
            };

            command.SharingId.Should().Be(sharingId);
        }
    }
}
