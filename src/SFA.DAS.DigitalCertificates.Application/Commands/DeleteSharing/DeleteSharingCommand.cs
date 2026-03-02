using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing
{
    public class DeleteSharingCommand : IRequest<DeleteSharingCommandResponse?>
    {
        public Guid SharingId { get; set; }
    }
}
