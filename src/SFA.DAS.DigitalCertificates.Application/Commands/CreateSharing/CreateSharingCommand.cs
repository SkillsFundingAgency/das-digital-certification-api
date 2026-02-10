using System;
using MediatR;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommand : IRequest<CreateSharingCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; }
    }
}
