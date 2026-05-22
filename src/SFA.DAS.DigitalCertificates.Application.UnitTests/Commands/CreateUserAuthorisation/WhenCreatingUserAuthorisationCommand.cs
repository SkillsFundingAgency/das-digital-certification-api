using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAuthorisation
{
    public class WhenCreatingUserAuthorisationCommand
    {
        [Test]
        public void And_AllPropertiesAreSet_Then_CommandIsCorrect()
        {
            var userId = Guid.NewGuid();

            var command = new CreateUserAuthorisationCommand
            {
                UserId = userId,
                Uln = 1234567890
            };

            command.UserId.Should().Be(userId);
            command.Uln.Should().Be(1234567890);
        }
    }
}
