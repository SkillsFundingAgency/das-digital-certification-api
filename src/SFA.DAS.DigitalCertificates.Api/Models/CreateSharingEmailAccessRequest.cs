using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingEmailAccessRequest
    {
        public Guid SharingEmailId { get; set; }

        public static implicit operator CreateSharingEmailAccessCommand(CreateSharingEmailAccessRequest source)
        {
            return new CreateSharingEmailAccessCommand
            {
                SharingEmailId = source.SharingEmailId
            };
        }
    }
}
