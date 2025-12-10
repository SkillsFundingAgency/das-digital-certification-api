using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.DigitalCertificates.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public class NoneRequirement : IAuthorizationRequirement
    {
    }
}
