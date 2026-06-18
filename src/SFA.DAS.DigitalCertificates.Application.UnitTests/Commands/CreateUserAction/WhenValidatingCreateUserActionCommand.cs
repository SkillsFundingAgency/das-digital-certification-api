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

        [TestCase("Smith <")]
        [TestCase("Smith >")]
        [TestCase("<Smith>")]
        public void And_FamilyNameContainsInvalidCharacters_Then_ErrorReturned(string familyName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = familyName,
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FamilyName");
        }

        [TestCase("<script>alert('xss')</script>")]
        [TestCase("<b>Smith</b>")]
        [TestCase("<img src='x' onerror='alert(1)'>")]
        [TestCase("</div>")]
        public void And_FamilyNameContainsHtmlTags_Then_ErrorReturnedWithCorrectMessage(string familyName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = familyName,
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "FamilyName" &&
                e.ErrorMessage == "FamilyName contains invalid characters.");
        }

        [TestCase("O'Brien")]
        [TestCase("Smith-Jones")]
        [TestCase("Van Der Berg")]
        [TestCase("Smith (Jr)")]
        public void And_FamilyNameContainsSpecialCharactersButNoHtmlTags_Then_CommandIsValid(string familyName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = familyName,
                GivenNames = "John"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [TestCase("John <")]
        [TestCase("John >")]
        [TestCase("<John>")]
        public void And_GivenNamesContainsInvalidCharacters_Then_ErrorReturned(string givenNames)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = givenNames
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "GivenNames");
        }

        [TestCase("<script>alert('xss')</script>")]
        [TestCase("<b>John</b>")]
        [TestCase("<img src='x' onerror='alert(1)'>")]
        [TestCase("</div>")]
        public void And_GivenNamesContainsHtmlTags_Then_ErrorReturnedWithCorrectMessage(string givenNames)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = givenNames
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "GivenNames" &&
                e.ErrorMessage == "GivenNames contains invalid characters.");
        }

        [TestCase("Mary-Jane")]
        [TestCase("O'Connor")]
        [TestCase("Jean Pierre")]
        [TestCase("John (Jr)")]
        public void And_GivenNamesContainsSpecialCharactersButNoHtmlTags_Then_CommandIsValid(string givenNames)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Contact,
                FamilyName = "Smith",
                GivenNames = givenNames
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [TestCase("Test Course <")]
        [TestCase("Test Course >")]
        [TestCase("<Test Course>")]
        public void And_ReprintActionWithCourseNameContainingInvalidCharacters_Then_ErrorReturned(string courseName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }

        [TestCase("<script>alert('xss')</script>")]
        [TestCase("<b>Course</b>")]
        [TestCase("<img src='x' onerror='alert(1)'>")]
        [TestCase("</div>")]
        public void And_ReprintActionWithCourseNameContainingHtmlTags_Then_ErrorReturnedWithCorrectMessage(string courseName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "CourseName" &&
                e.ErrorMessage == "CourseName contains invalid characters.");
        }

        [TestCase("Test & Course")]
        [TestCase("Test (Course)")]
        [TestCase("Test 'Course'")]
        [TestCase("Test \"Course\"")]
        public void And_ReprintActionWithCourseNameContainingSpecialCharactersButNoHtmlTags_Then_CommandIsValid(string courseName)
        {
            var command = new CreateUserActionCommand
            {
                UserId = Guid.NewGuid(),
                ActionType = ActionType.Reprint,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
