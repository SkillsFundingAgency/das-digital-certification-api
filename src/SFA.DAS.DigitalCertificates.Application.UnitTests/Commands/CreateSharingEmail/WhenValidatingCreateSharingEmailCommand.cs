using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharingEmail
{
    public class WhenValidatingCreateSharingEmailCommand
    {
        private CreateSharingEmailCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateSharingEmailCommandValidator();
        }

        [Test]
        public void And_AllFieldsAreCorrect_Then_CommandIsValid()
        {
            // Arrange
            var command = new CreateSharingEmailCommand
            {
                SharingId = Guid.NewGuid(),
                EmailAddress = "test@example.com"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_SharingIdIsEmpty_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingEmailCommand
            {
                SharingId = Guid.Empty,
                EmailAddress = "test@example.com"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "SharingId");
        }

        [Test]
        public void And_EmailAddressIsEmpty_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingEmailCommand
            {
                SharingId = Guid.NewGuid(),
                EmailAddress = ""
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "EmailAddress");
        }

        [Test]
        public void And_EmailAddressIsInvalid_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingEmailCommand
            {
                SharingId = Guid.NewGuid(),
                EmailAddress = "not-an-email"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "EmailAddress");
        }
    }
}
