using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Api.Authentication
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, ApplicationSettings? applicationSettings, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                services.AddAuthentication(auth =>
                {
                    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(auth =>
                {
                    if (applicationSettings != null)
                    {
                        auth.Authority = $"https://login.microsoftonline.com/{applicationSettings.AzureAd?.Tenant}";

                        if (applicationSettings.AzureAd?.Audiences != null)
                        {
                            auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                ValidAudiences = new List<string>
                                {
                                    applicationSettings.AzureAd.Audiences
                                }
                            };
                        }
                    }
                });
                services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();
            }

            return services;
        }
    }
}
