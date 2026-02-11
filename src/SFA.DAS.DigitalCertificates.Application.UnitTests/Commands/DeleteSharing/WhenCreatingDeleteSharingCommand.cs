using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.DeleteSharing
{
    public class WhenCreatingDeleteSharingCommand
    {
        [Test]
        public void Then_Can_Create_Command_With_SharingId()
        {
            var id = Guid.NewGuid();
            var command = new DeleteSharingCommand { SharingId = id };

            command.Should().NotBeNull();
            command.SharingId.Should().Be(id);
        }
    }
}
