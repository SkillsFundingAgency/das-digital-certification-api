using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Behaviours;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries
{
    public class WhenHandlingGetUserQuery
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

        [TestCase(null)]
        [TestCase("")]
        public async Task And_GetUserQueryValidator_Throws_When_GovUkIdentifier_IsNull(string govUkIdentifier)
        {
            var validator = new GetUserQueryValidator();
            var query = new GetUserQuery { GovUkIdentifier = govUkIdentifier };

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(GetUserQuery.GovUkIdentifier));
        }

        [Test]
        public async Task And_Sending_Query_With_Null_Id_Throws_ValidationException()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetUserQuery>());
            services.AddValidatorsFromAssemblyContaining<GetUserQuery>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped<IUserEntityContext, DigitalCertificatesDataContext>();

            using var sp = services.BuildServiceProvider();
            var mediator = sp.GetRequiredService<IMediator>();

            var act = async () => await mediator.Send(new GetUserQuery { GovUkIdentifier = null! });
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
