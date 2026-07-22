using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateUserAuthorisationRequest
    {
        public required long Uln { get; set; }

        public static implicit operator CreateUserAuthorisationCommand(CreateUserAuthorisationRequest source)
        {
            return new CreateUserAuthorisationCommand
            {
                Uln = source.Uln
            };
        }
    }
}
