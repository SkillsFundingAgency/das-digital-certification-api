using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingCommand : IRequest<CreateCertificateSharingCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public required string CertificateType { get; set; }
        public required string CourseName { get; set; }
    }
}
