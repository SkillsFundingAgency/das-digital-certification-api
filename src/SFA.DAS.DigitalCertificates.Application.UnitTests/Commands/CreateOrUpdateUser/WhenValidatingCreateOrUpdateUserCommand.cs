using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateOrUpdateUser
{
    public class WhenValidatingCreateOrUpdateUserCommand
    {
        [Test, AutoData]
        public void Then_Command_Is_Valid_When_All_Fields_Are_Correct(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com"
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test, AutoData]
        public void Then_Error_If_GovUkIdentifier_Is_Empty([Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "",
                EmailAddress = "user@example.com"
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.GovUkIdentifier)
                .WithErrorMessage("GovUkIdentifier must not be empty");
        }

        [Test, AutoData]
        public void Then_Error_If_EmailAddress_Is_Empty([Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = ""
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.EmailAddress)
                .WithErrorMessage("EmailAddress must not be empty");
        }
    }
}
