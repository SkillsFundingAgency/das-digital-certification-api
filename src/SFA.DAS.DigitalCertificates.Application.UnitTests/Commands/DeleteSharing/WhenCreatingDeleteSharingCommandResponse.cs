using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.DeleteSharing
{
    public class WhenCreatingDeleteSharingCommandResponse
    {
        [Test]
        public void Then_Can_Create_Response_With_SharingId()
        {
            var id = Guid.NewGuid();
            var response = new DeleteSharingCommandResponse { SharingId = id };

            response.Should().NotBeNull();
            response.SharingId.Should().Be(id);
        }
    }
}
