using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetSharingById
{
    [TestFixture]
    public class WhenValidatingGetSharingByIdQuery
    {
        private GetSharingByIdQueryValidator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new GetSharingByIdQueryValidator();
        }

        [Test]
        public void And_SharingIdIsEmpty_Then_ValidationFails()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.Empty };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.SharingId));
        }

        [Test]
        public void And_SharingIdIsValid_Then_ValidationPasses()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid() };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_LimitIsZero_Then_ValidationFails()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = 0 };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.Limit));
        }

        [Test]
        public void And_LimitIsNegative_Then_ValidationFails()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = -1 };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(GetSharingByIdQuery.Limit));
        }

        [Test]
        public void And_LimitIsPositive_Then_ValidationPasses()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = 10 };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void And_LimitIsNull_Then_ValidationPasses()
        {
            // Arrange
            var query = new GetSharingByIdQuery { SharingId = Guid.NewGuid(), Limit = null };

            // Act
            var result = _sut.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}