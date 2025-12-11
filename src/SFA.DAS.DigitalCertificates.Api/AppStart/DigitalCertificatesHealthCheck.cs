using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public class DigitalCertificatesHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultsDescription = "Digital Certificates API Health Check";
        private readonly IUserEntityContext _userEntityContext;

        public DigitalCertificatesHealthCheck(IUserEntityContext userEntityContext)
        {
            _userEntityContext = userEntityContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbConnectionHealthy = true;
            try
            {
                await _userEntityContext.GetFirstOrDefault();
            }
            catch
            {
                dbConnectionHealthy = false;
            }

            return dbConnectionHealthy ? HealthCheckResult.Healthy(HealthCheckResultsDescription) : HealthCheckResult.Unhealthy(HealthCheckResultsDescription);
        }
    }
}