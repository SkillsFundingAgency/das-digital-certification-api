using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.DigitalCertificates.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder AllowAnonymousUser(this AuthorizationPolicyBuilder builder)
        {
            builder.Requirements.Add(new NoneRequirement());
            return builder;
        }
    }
}
