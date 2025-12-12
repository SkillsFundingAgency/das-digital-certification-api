using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationExtensions
    {
        public static bool IsLocalAcceptanceOrDev(this IConfiguration config)
        {
            return (config?["EnvironmentName"]?.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ?? false) ||
                   (config?["EnvironmentName"]?.Equals("ACCEPTANCE_TESTS", StringComparison.CurrentCultureIgnoreCase) ?? false) ||
                   (config?["EnvironmentName"]?.Equals("DEV", StringComparison.CurrentCultureIgnoreCase) ?? false);
        }

        public static bool IsIntegrationTests(this IConfiguration config)
        {
            return config?["EnvironmentName"]?.Equals("IntegrationTests", StringComparison.CurrentCultureIgnoreCase) ?? false;
        }
    }
}
