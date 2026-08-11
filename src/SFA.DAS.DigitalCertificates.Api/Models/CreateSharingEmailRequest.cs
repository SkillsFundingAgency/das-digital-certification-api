using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingEmailRequest
    {
        public required string EmailAddress { get; set; }

        public static implicit operator CreateSharingEmailCommand(CreateSharingEmailRequest source)
        {
            return new CreateSharingEmailCommand
            {
                EmailAddress = source.EmailAddress
            };
        }
    }
}
