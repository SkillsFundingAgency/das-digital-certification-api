using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction;
// TODO: Align the namespace and folder structure. This will be addressed during cleanup, as there are dependent branches in the chain.
namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries.GetUserAction
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
