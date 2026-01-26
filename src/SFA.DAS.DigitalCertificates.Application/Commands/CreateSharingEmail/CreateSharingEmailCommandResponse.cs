using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandResponse
    {
        public Guid Id { get; set; }
        public Guid EmailLinkCode { get; set; }
    }
}
