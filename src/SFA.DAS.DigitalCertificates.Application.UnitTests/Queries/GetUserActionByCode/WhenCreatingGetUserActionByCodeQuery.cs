using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode;
namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserActionByCode
{
    [TestFixture]
    public class WhenCreatingGetUserActionByCodeQuery
    {
        [Test]
        public void And_PropertiesSet_Then_ValuesAreMapped()
        {
            // Arrange
            var actionCode = "CODE123";

            // Act
            var query = new GetUserActionByCodeQuery { ActionCode = actionCode };

            // Assert
            query.ActionCode.Should().Be(actionCode);
        }
    }
}
