using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.DigitalCertificates.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public class LocalAuthorizationHandler : AuthorizationHandler<NoneRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            NoneRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
