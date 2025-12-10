using System;
using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Configuration;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddDatabaseRegistrations
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            if (configuration.IsLocalAcceptanceOrDev())
            {
                services.AddDbContext<DigitalCertificatesDataContext>(options => options.UseSqlServer(appSettings?.DbConnectionString).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            }
            else if (configuration.IsIntegrationTests())
            {
                services.AddDbContext<DigitalCertificatesDataContext>(options => options.UseSqlServer("Server=localhost;Database=SFA.DAS.DigitalCertificates.IntegrationTests.Database;Trusted_Connection=True;MultipleActiveResultSets=true").EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            }
            else
            {
                services.AddSingleton(new ChainedTokenCredential(
                    new ManagedIdentityCredential(),
                    new AzureCliCredential(),
                    new VisualStudioCodeCredential(),
                    new VisualStudioCredential())
            );
                services.AddDbContext<DigitalCertificatesDataContext>(ServiceLifetime.Transient);
            }

            services.AddTransient(provider => new Lazy<DigitalCertificatesDataContext>(() => provider.GetRequiredService<DigitalCertificatesDataContext>()));
        }
    }
}
