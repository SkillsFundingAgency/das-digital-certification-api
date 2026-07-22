using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingEmailResponse
    {
        public Guid Id { get; set; }
        public Guid EmailLinkCode { get; set; }

        public static implicit operator CreateSharingEmailResponse(CreateSharingEmailCommandResponse source)
        {
            if (source == null) return null!;

            return new CreateSharingEmailResponse
            {
                Id = source.Id,
                EmailLinkCode = source.EmailLinkCode
            };
        }
    }
}
