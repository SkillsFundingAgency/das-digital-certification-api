using System;
using NUnit.Framework;
using FluentAssertions;
using SFA.DAS.DigitalCertificates.Application.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Extensions
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [Test]
        public void ToReadableString_ShouldCorrectlyFormatDays()
        {
            // Arrange
            TimeSpan timeSpan = TimeSpan.FromDays(2);

            // Act
            string result = timeSpan.ToReadableString();

            // Assert
            result.Should().Be("2 days");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyFormatHoursMinutesSeconds()
        {
            // Arrange
            TimeSpan timeSpan = new TimeSpan(0, 2, 3, 4);

            // Act
            string result = timeSpan.ToReadableString();

            // Assert
            result.Should().Be("2 hours, 3 minutes, 4 seconds");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyHandleSingularUnits()
        {
            // Arrange
            TimeSpan timeSpan = new TimeSpan(1, 1, 1, 1);

            // Act
            string result = timeSpan.ToReadableString();

            // Assert
            result.Should().Be("1 day, 1 hour, 1 minute, 1 second");
        }

        [Test]
        public void ToReadableString_ShouldReturnZeroSecondsForZeroTimeSpan()
        {
            // Arrange
            TimeSpan timeSpan = TimeSpan.Zero;

            // Act
            string result = timeSpan.ToReadableString();

            // Assert
            result.Should().Be("0 seconds");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyFormatComplexTimeSpan()
        {
            // Arrange
            TimeSpan timeSpan = new TimeSpan(1, 22, 333, 44444);

            // Act
            string result = timeSpan.ToReadableString();

            // Assert
            result.Should().Be("2 days, 15 hours, 53 minutes, 44 seconds");
        }
    }

}
