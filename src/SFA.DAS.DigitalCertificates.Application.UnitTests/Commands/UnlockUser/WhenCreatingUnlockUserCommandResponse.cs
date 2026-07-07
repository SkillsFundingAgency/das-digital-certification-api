using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UnlockUser
{
    public class WhenCreatingUnlockUserCommandResponse
    {
        [Test]
        public void Response_Should_Allow_Property_Assignment()
        {
            var resp = new UnlockUserCommandResponse
            {
                NotFound = true,
                Updated = true
            };

            resp.NotFound.Should().BeTrue();
            resp.Updated.Should().BeTrue();
        }
    }
}
