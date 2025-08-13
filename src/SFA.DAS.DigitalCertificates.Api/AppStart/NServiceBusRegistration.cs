using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class NServiceBusRegistration
    {
        public static async Task<UpdateableServiceProvider> StartNServiceBus(this UpdateableServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (configuration.IsIntegrationTests()) return serviceProvider;

            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.DigitalCertificates.Api")
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseServicesBuilder(serviceProvider)
                .UseSqlServerPersistence(() => CreateSqlConnection(configuration, appSettings))
                .UseSendOnly();

            if (appSettings?.NServiceBusConnectionString?.Equals("UseLearningEndpoint=true", StringComparison.CurrentCultureIgnoreCase) ?? false)
            {
                endpointConfiguration.UseTransport<LearningTransport>();
                endpointConfiguration.UseLearningTransport(s => s.AddRouting());
            }
            else
            {
                endpointConfiguration.UseAzureServiceBusTransport(appSettings?.NServiceBusConnectionString, r => r.AddRouting());
            }

            if (!string.IsNullOrEmpty(appSettings?.NServiceBusLicense))
            {
                endpointConfiguration.License(appSettings.NServiceBusLicense);
            }

            var endpoint = await Endpoint.Start(endpointConfiguration);

            serviceProvider.AddSingleton(p => endpoint)
                .AddSingleton<IMessageSession>(p => p.GetRequiredService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();

            return serviceProvider;
        }

        private static DbConnection CreateSqlConnection(IConfiguration configuration, ApplicationSettings? appSettings)
        {
            if (configuration.IsLocalAcceptanceOrDev())
            {
                return new SqlConnection(appSettings?.DbConnectionString);
            }
            else
            {
                return new SqlConnection
                {
                    ConnectionString = appSettings?.DbConnectionString,
                    AccessToken = new DefaultAzureCredential().GetTokenAsync(
                        new TokenRequestContext(scopes: ["https://database.windows.com/"])).Result.Token
                };
            }
        }
    }
}
