using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Behaviours;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests.Queries
{
    public class WhenHandlingGetUserAuthorisationQuery
    {
        [Test, MoqAutoData]
        public async Task And_UserAuthorisationExists_Then_ReturnsResultWithAuthorisation(
            Guid userId,
            [Frozen] Mock<IUserEntityContext> userEntityContext)
        {
            // Arrange
            var userAuthorisation = new UserAuthorisation { Id = Guid.NewGuid(), UserId = userId, ULN = 12345678, AuthorisedAt = DateTime.Now };
            userEntityContext
                .Setup(x => x.GetUserAuthorisationByUserId(userId))
                .ReturnsAsync(userAuthorisation);

            var _sut = new GetUserAuthorisationQueryHandler(userEntityContext.Object);

            var query = new GetUserAuthorisationQuery { UserId = userId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Authorisation.Should().BeEquivalentTo((Domain.Models.UserAuthorisation?)userAuthorisation);
        }

        [Test, MoqAutoData]
        public async Task And_UserAuthorisationDoesNotExist_Then_ReturnsResultWithNullAuthorisation(
            Guid userId,
            [Frozen] Mock<IUserEntityContext> userEntityContext)
        {
            // Arrange
            userEntityContext
                .Setup(x => x.GetUserAuthorisationByUserId(userId))
                .ReturnsAsync((UserAuthorisation?)null);

            var _sut = new GetUserAuthorisationQueryHandler(userEntityContext.Object);

            var query = new GetUserAuthorisationQuery { UserId = userId };

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Authorisation.Should().BeNull();
        }

        public async Task And_GetUserAuthorisationQueryValidator_Throws_When_UserId_IsEmpty()
        {
            var validator = new GetUserAuthorisationQueryValidator();
            var query = new GetUserAuthorisationQuery { UserId = Guid.Empty };

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(GetUserAuthorisationQuery.UserId));
        }

        [Test]
        public async Task And_Sending_Query_With_Empty_Id_Throws_ValidationException()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetUserAuthorisationQuery>());
            services.AddValidatorsFromAssemblyContaining<GetUserAuthorisationQuery>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped<IUserEntityContext, DigitalCertificatesDataContext>();

            using var sp = services.BuildServiceProvider();
            var mediator = sp.GetRequiredService<IMediator>();

            var act = async () => await mediator.Send(new GetUserAuthorisationQuery { UserId = Guid.Empty });
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
