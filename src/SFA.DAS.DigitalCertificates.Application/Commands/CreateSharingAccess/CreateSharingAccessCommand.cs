using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess
{
    public class CreateSharingAccessCommand : IRequest<Guid?>
    {
        public Guid SharingId { get; set; }
    }
}
