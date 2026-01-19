using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmail
{
    public class WhenUsingCreateSharingEmailCommandResponse
    {
        [Test]
        public void Response_Should_Allow_Property_Assignment()
        {
            var resp = new CreateSharingEmailCommandResponse
            {
                Id = Guid.NewGuid(),
                EmailLinkCode = Guid.NewGuid()
            };

            resp.Id.Should().NotBeEmpty();
            resp.EmailLinkCode.Should().NotBeEmpty();
        }
    }
}
