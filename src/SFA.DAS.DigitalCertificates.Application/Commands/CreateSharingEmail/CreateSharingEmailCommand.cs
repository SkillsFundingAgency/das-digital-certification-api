using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommand : IRequest<CreateSharingEmailCommandResponse?>
    {
        public Guid SharingId { get; set; }
        public required string EmailAddress { get; set; }
    }
}
