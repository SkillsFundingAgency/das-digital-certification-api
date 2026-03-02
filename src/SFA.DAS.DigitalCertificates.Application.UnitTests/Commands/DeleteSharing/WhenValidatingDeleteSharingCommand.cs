using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.DeleteSharing
{
    [TestFixture]
    public class WhenValidatingDeleteSharingCommand
    {
        private DeleteSharingCommandValidator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new DeleteSharingCommandValidator();
        }

        [Test]
        public void And_SharingIdIsEmpty_Then_ValidationFails()
        {
            var command = new DeleteSharingCommand { SharingId = Guid.Empty };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(DeleteSharingCommand.SharingId));
        }

        [Test]
        public void And_SharingIdIsValid_Then_ValidationPasses()
        {
            var command = new DeleteSharingCommand { SharingId = Guid.NewGuid() };

            var result = _sut.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
