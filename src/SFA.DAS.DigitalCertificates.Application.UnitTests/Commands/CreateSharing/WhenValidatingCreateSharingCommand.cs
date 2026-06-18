using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateSharing
{
    public class WhenValidatingCreateSharingCommand
    {
        private CreateSharingCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateSharingCommandValidator();
        }

        [Test]
        public void And_AllFieldsAreCorrect_Then_CommandIsValid()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_CertificateTypeIsFramework_Then_CommandIsValid()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Framework,
                CourseName = "Test Course"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_UserIdIsEmpty_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.Empty,
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
        }

        [Test]
        public void And_CertificateIdIsEmpty_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.Empty,
                CertificateType = CertificateType.Standard,
                CourseName = "Test Course"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateId");
        }

        [Test]
        public void And_CertificateTypeIsUnknown_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Unknown,
                CourseName = "Test Course"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType");
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("CertificateType must be either Standard or Framework"));
        }

        [Test]
        public void And_CourseNameIsEmpty_Then_ErrorReturned()
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = ""
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }

        [TestCase("Test Course <")]
        [TestCase("Test Course >")]
        [TestCase("<Test Course>")]
        public void And_CourseNameContainsInvalidCharacters_Then_ErrorReturned(string courseName)
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }

        [TestCase("<script>alert('xss')</script>")]
        [TestCase("<b>Bold</b>")]
        [TestCase("<img src='x' onerror='alert(1)'>")]
        [TestCase("</div>")]
        public void And_CourseNameContainsHtmlTags_Then_ErrorReturnedWithCorrectMessage(string courseName)
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == "CourseName" &&
                e.ErrorMessage == "CourseName contains invalid characters.");
        }

        [TestCase("Test & Course")]
        [TestCase("Test (Course)")]
        [TestCase("Test 'Course'")]
        [TestCase("Test \"Course\"")]
        public void And_CourseNameContainsSpecialCharactersButNoHtmlTags_Then_CommandIsValid(string courseName)
        {
            // Arrange
            var command = new CreateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = CertificateType.Standard,
                CourseName = courseName
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
