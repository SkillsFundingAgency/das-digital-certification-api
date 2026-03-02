using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingAccess
{
    [TestFixture]
    public class WhenValidatingCreateSharingAccessCommand
    {
        private CreateSharingAccessCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateSharingAccessCommandValidator();
        }

        [Test]
        public void And_SharingIdIsEmpty_Then_ValidationFails()
        {
            var command = new CreateSharingAccessCommand { SharingId = Guid.Empty };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateSharingAccessCommand.SharingId));
        }

        [Test]
        public void And_SharingIdIsValid_Then_ValidationPasses()
        {
            var command = new CreateSharingAccessCommand { SharingId = Guid.NewGuid() };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
