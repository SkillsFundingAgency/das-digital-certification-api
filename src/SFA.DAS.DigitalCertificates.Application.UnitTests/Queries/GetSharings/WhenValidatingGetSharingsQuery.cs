using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharings
{
    [TestFixture]
    public class WhenValidatingGetSharingsQuery
    {
        [Test]
        public void And_UserIdIsEmpty_Then_ValidationFails()
        {
            // Arrange
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.Empty, CertificateId = Guid.NewGuid() };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
        }

        [Test]
        public void And_CertificateIdIsEmpty_Then_ValidationFails()
        {
            // Arrange
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.Empty };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CertificateId");
        }

        [Test]
        public void And_LimitIsZeroOrNegative_Then_ValidationFails()
        {
            // Arrange
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.NewGuid(), Limit = 0 };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Limit");
        }

        [Test]
        public void And_ValidQuery_Then_ValidationSucceeds()
        {
            // Arrange
            var validator = new GetSharingsQueryValidator();
            var query = new GetSharingsQuery { UserId = Guid.NewGuid(), CertificateId = Guid.NewGuid(), Limit = 1 };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
