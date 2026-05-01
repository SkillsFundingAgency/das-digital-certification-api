using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Commands.CreateUserMatch
{
    public class WhenValidatingCreateUserMatchCommand
    {
        private CreateUserMatchCommandValidator _validator = null!;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateUserMatchCommandValidator();
        }

        [Test]
        public void And_AllRequiredFieldsProvided_Then_IsValid()
        {
            var command = new CreateUserMatchCommand
            {
                UserId = Guid.NewGuid(),
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                CourseLevel = "2",
                DateAwarded = 2020,
                ProviderName = "Provider",
                Ukprn = 123456,
                IsMatched = true,
                IsFailed = false
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_FamilyNameMissing_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FamilyName");
        }

        [Test]
        public void And_UkprnIsZero_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 0,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Ukprn");
        }

        [Test]
        public void And_DateOfBirthDefault_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = default,
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
        }

        [Test]
        public void And_CertificateTypeMissing_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Unknown,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType");
        }

        [Test]
        public void And_CourseCodeMissing_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseCode");
        }

        [Test]
        public void And_CourseNameMissing_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "",
                ProviderName = "Provider",
                Ukprn = 1,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
        }

        [Test]
        public void And_ProviderNameMissing_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "",
                Ukprn = 1,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ProviderName");
        }

        [Test]
        public void And_UkprnIsNegative_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = -5,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Ukprn");
        }

        [Test]
        public void And_MultipleRequiredFieldsMissing_Then_MultipleErrorsReturned()
        {
            var command = new CreateUserMatchCommand
            {
                Uln = 1234567890,
                FamilyName = "",
                DateOfBirth = default,
                CertificateType = CertificateType.Unknown,
                CourseCode = "",
                CourseName = "",
                ProviderName = "",
                Ukprn = 0,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FamilyName");
            result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateType");
            result.Errors.Should().Contain(e => e.PropertyName == "CourseCode");
            result.Errors.Should().Contain(e => e.PropertyName == "CourseName");
            result.Errors.Should().Contain(e => e.PropertyName == "ProviderName");
            result.Errors.Should().Contain(e => e.PropertyName == "Ukprn");
        }

        [Test]
        public void And_UlnMissingWhenMatched_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                UserId = Guid.NewGuid(),
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1,
                IsMatched = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Uln" && e.ErrorMessage.Contains("ULN is required when matched"));
        }

        [Test]
        public void And_IsMatchedAndIsFailed_BothTrue_Then_ErrorReturned()
        {
            var command = new CreateUserMatchCommand
            {
                UserId = Guid.NewGuid(),
                Uln = 1234567890,
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                CertificateType = CertificateType.Standard,
                CourseCode = "C123",
                CourseName = "Test Course",
                ProviderName = "Provider",
                Ukprn = 1,
                IsMatched = true,
                IsFailed = true
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "A user match cannot be both matched and failed");
        }

        [Test]
        public void And_ConditionalFieldsNotRequiredWhenNotMatched_Then_IsValid()
        {
            var command = new CreateUserMatchCommand
            {
                UserId = Guid.NewGuid(),
                FamilyName = "Smith",
                DateOfBirth = new DateTime(1990,1,1),
                IsMatched = false,
                IsFailed = false
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
