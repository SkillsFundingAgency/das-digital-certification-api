using System;
using System.Collections.Generic;
using AutoFixture.NUnit4;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.UpdateUserIdentity
{
    public class WhenValidatingUpdateUserIdentityCommand
    {
        [Test, AutoData]
        public void Then_Command_Is_Valid_When_All_Fields_Are_Correct(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { GivenNames = "Jane", FamilyName = "Doe" }
                },
                DateOfBirth = now.AddYears(-25)
            }, Guid.NewGuid());

            var validator = new UpdateUserIdentityCommandValidator(mockDateTimeProvider.Object);

            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test, AutoData]
        public void Then_Error_If_Names_List_Is_Empty(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>(),
                DateOfBirth = now.AddYears(-20)
            }, Guid.NewGuid());

            var validator = new UpdateUserIdentityCommandValidator(mockDateTimeProvider.Object);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Names)
                .WithErrorMessage("Names must have at least one entry");
        }

        [Test, AutoData]
        public void Then_Error_If_Names_Is_Null(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = null!,
                DateOfBirth = now.AddYears(-30)
            }, Guid.NewGuid());

            var validator = new UpdateUserIdentityCommandValidator(mockDateTimeProvider.Object);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Names)
                .WithErrorMessage("Names must have at least one entry");
        }

        [Test, AutoData]
        public void Then_Error_If_DateOfBirth_Is_In_The_Future(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { GivenNames = "Jane", FamilyName = "Doe" }
                },
                DateOfBirth = now.AddDays(1)
            }, Guid.NewGuid());

            var validator = new UpdateUserIdentityCommandValidator(mockDateTimeProvider.Object);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth)
                .WithErrorMessage("DateOfBirth cannot be in the future");
        }

        [Test, AutoData]
        public void Then_Error_If_DateOfBirth_Is_Default(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new UpdateUserIdentityCommand(new UpdateUserIdentityRequest
            {
                Names = new List<Name>
                {
                    new() { GivenNames = "Jane", FamilyName = "Doe" }
                },
                DateOfBirth = default
            }, Guid.NewGuid());

            var validator = new UpdateUserIdentityCommandValidator(mockDateTimeProvider.Object);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth)
                .WithErrorMessage("DateOfBirth is required");
        }
    }
}