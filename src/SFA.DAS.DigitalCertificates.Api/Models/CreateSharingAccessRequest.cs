using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingAccessRequest
    {
        public Guid SharingId { get; set; }

        public static implicit operator CreateSharingAccessCommand(CreateSharingAccessRequest source)
        {
            return new CreateSharingAccessCommand
            {
                SharingId = source.SharingId
            };
        }
    }
}
