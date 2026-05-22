using System.Text.Json;
using SFA.DAS.Encoding;
using SFA.DAS.DigitalCertificates.Api.Configuration;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.DigitalCertificates.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationExtensions
{
    public static IServiceCollection AddEncodingConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var encodingsConfiguration = configuration.GetSection(ConfigurationKeys.EncodingConfig).Value;

        var encodingConfig = JsonSerializer.Deserialize<EncodingConfig>(encodingsConfiguration!);
        services.AddSingleton(encodingConfig!);

        return services;
    }
}
