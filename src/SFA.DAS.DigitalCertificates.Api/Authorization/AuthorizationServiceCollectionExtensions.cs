using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.DigitalCertificates.Api.Authentication;

namespace SFA.DAS.DigitalCertificates.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class AuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddApiAuthorization(this IServiceCollection services, bool IsDevelopment)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy(PolicyNames.Default, policy =>
                {
                    if (IsDevelopment)
                        policy.AllowAnonymousUser();
                    else
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(RoleNames.Default);
                    }
                });

                var defaultPolicy = x.GetPolicy(PolicyNames.Default);
                if (defaultPolicy != null)
                    x.DefaultPolicy = defaultPolicy;
            });

            if (IsDevelopment)
                services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();

            return services;
        }
    }
}
