using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateCertificateSharing
{
    public class WhenValidatingCreateCertificateSharingCommand
    {
        private CreateCertificateSharingCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateCertificateSharingCommandValidator();
        }

        [Test]
        public void And_AllFieldsAreCorrect_Then_CommandIsValid()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_UserIdIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.Empty,
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
        }

        [Test]
        public void And_CertificateIdIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.Empty,
                CertificateType = "Standard",
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateId");
        }

        [Test]
        public void And_CertificateTypeIsInvalid_Then_ErrorReturned()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Other",
                CourseName = "Test Course"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType");
        }

        [Test]
        public void And_CourseNameIsEmpty_Then_ErrorReturned()
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = Guid.NewGuid(),
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = ""
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }
    }
}
