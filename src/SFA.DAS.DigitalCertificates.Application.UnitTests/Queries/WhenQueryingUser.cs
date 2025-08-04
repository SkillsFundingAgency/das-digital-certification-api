using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries
{
    public class WhenQueryingUser
    {
        [Test, AutoMoqData]
        public async Task And_User_IsFound_ByGovUkIdentifier_ThenUserIsReturned(
            string govUkIdentifier,
            [Frozen(Matching.ImplementedInterfaces)] DigitalCertificatesDataContext context)
        {
            // Arrange
            var userId = Guid.NewGuid();

            context.Add(new User { Id = userId, GovUkIdentifier = govUkIdentifier, EmailAddress = "test@test.com" });
            await context.SaveChangesAsync();

            var query = new GetUserQuery() { GovUkIdentifier = govUkIdentifier };
            var handler = new GetUserQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResult = new GetUserQueryResult
            {
                User = new Domain.Models.User
                {
                    Id = userId,
                    GovUkIdentifier = govUkIdentifier,
                    EmailAddress = "test@test.com"
                }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test, AutoMoqData]
        public async Task And_User_IsNotFound_ThenNullIsReturned(
            [Frozen(Matching.ImplementedInterfaces)] DigitalCertificatesDataContext context)
        {
            // Arrange
            var query = new GetUserQuery() { GovUkIdentifier = "Unknown" };
            var handler = new GetUserQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.User.Should().BeNull();
        }
    }
}
