using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Application.Extensions;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands
{
    public class WhenValidatingCreateOrUpdateUserCommand
    {
        [Test, AutoData]
        public void Then_Command_Is_Valid_When_All_Fields_Are_Correct(
            [Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            // Arrange
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeHelper.Setup(x => x.Now).Returns(now);

            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com",
                Names = new List<Name> { new Name { GivenNames = "Jane", FamilyName = "Doe" } },
                DateOfBirth = now.AddYears(-25)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test, AutoData]
        public void Then_Error_If_GovUkIdentifier_Is_Empty([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "",
                EmailAddress = "user@example.com",
                Names = new List<Name> { new Name { GivenNames = "Jane", FamilyName = "Doe" } },
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.GovUkIdentifier)
                .WithErrorMessage("GovUkIdentifier must not be empty");
        }

        [Test, AutoData]
        public void Then_Error_If_EmailAddress_Is_Empty([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "",
                Names = new List<Name> { new Name { GivenNames = "Jane", FamilyName = "Doe" } },
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.EmailAddress)
                .WithErrorMessage("EmailAddress must not be empty");
        }

        [Test, AutoData]
        public void Then_Error_If_Names_List_Is_Empty([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com",
                Names = new List<Name>(),
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Names)
                .WithErrorMessage("Names must have at least one entry if provided");
        }

        [Test, AutoData]
        public void Then_Valid_If_Names_Is_Null([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com",
                Names = null,
                DateOfBirth = DateTime.UtcNow.AddYears(-30)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.Names);
        }

        [Test, AutoData]
        public void Then_Error_If_DateOfBirth_Is_In_The_Future([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeHelper.Setup(x => x.Now).Returns(now);

            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com",
                Names = new List<Name> { new Name { GivenNames = "Jane", FamilyName = "Doe" } },
                DateOfBirth = now.AddDays(1)
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth)
                .WithErrorMessage("DateOfBirth cannot be in the future");
        }

        [Test, AutoData]
        public void Then_Valid_If_DateOfBirth_Is_Null([Frozen] Mock<IDateTimeHelper> mockDateTimeHelper)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeHelper.Setup(x => x.Now).Returns(now);

            var command = new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = "gov-123",
                EmailAddress = "user@example.com",
                Names = new List<Name> { new Name { GivenNames = "Jane", FamilyName = "Doe" } },
                DateOfBirth = null
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeHelper.Object);
            var result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.DateOfBirth);
        }
    }
}
