using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess
{
    public class CreateSharingEmailAccessCommand : IRequest<Guid?>
    {
        public Guid SharingEmailId { get; set; }
    }
}
