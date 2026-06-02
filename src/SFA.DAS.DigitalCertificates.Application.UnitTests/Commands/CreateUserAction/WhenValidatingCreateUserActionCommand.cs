using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserAction
{
    public class WhenValidatingCreateUserActionCommand
    {
        private CreateUserActionCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateUserActionCommandValidator();
        }

        [Test]
        public void And_ContactAction_Then_CommandIsValid()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_NotMatchedAction_Then_CommandIsValid()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.NotMatched,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_NotFoundAction_Then_CommandIsValid()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.NotFound,
                FamilyName = "Smith",
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_ReprintActionWithCertificateFields_Then_CommandIsValid()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_HelpActionWithCertificateFields_Then_CommandIsValid()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Help,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Framework,
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_ReprintActionWithoutCertificateId_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateId");
        }

        [Test]
        public void And_HelpActionWithoutCourseName_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Help,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = ""
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }

        [Test]
        public void And_FamilyNameIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "",
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FamilyName");
        }

        [Test]
        public void And_GivenNamesIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = ""
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "GivenNames");
        }

        [Test]
        public void And_ReprintActionWithNullCertificateType_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = null,
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType");
        }

        [Test]
        public void And_ReprintActionWithUnknownCertificateType_Then_ErrorReturned()
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Unknown,
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType"
                && e.ErrorMessage.Contains("CertificateType must be either Standard or Framework"));
        }
    }
}
