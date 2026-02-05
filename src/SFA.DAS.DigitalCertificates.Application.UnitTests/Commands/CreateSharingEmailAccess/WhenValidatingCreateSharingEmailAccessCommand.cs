using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmailAccess
{
    [TestFixture]
    public class WhenValidatingCreateSharingEmailAccessCommand
    {
        private CreateSharingEmailAccessCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateSharingEmailAccessCommandValidator();
        }

        [Test]
        public void And_SharingEmailIdIsEmpty_Then_ValidationFails()
        {
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.Empty };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateSharingEmailAccessCommand.SharingEmailId));
        }

        [Test]
        public void And_SharingEmailIdIsValid_Then_ValidationPasses()
        {
            var command = new CreateSharingEmailAccessCommand { SharingEmailId = Guid.NewGuid() };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
