using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Models;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Mapping
{
    [TestFixture]
    public class UpdateUserIdentityRequestTests
    {
        [Test]
        public void ImplicitOperator_Maps_ApiModel_To_ApplicationModel()
        {
            // Arrange
            var api = new UpdateUserIdentityRequest
            {
                Names = new List<NameRequest>
                {
                    new NameRequest
                    {
                        FamilyName = "Smith",
                        GivenNames = "John",
                        ValidSince = new DateTime(2020,1,1),
                        ValidUntil = new DateTime(2021,1,1)
                    }
                },
                DateOfBirth = new DateTime(1990,1,1)
            };

            // Act
            UpdateUserIdentityRequest app = api;

            // Assert
            app.Should().NotBeNull();
            app.DateOfBirth.Should().Be(api.DateOfBirth);
            app.Names.Should().NotBeNull();
            app.Names.Should().HaveCount(1);

            var name = app.Names[0];
            name.FamilyName.Should().Be("Smith");
            name.GivenNames.Should().Be("John");
            name.ValidSince.Should().Be(new DateTime(2020,1,1));
            name.ValidUntil.Should().Be(new DateTime(2021,1,1));
        }
    }
}
