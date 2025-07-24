using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public class NoneRequirement : IAuthorizationRequirement
    {
    }
}
