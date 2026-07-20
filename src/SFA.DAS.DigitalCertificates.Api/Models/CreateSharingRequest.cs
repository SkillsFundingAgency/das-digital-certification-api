using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingRequest
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; }

        public static implicit operator CreateSharingCommand(CreateSharingRequest source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CreateSharingCommand
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName
            };
        }
    }
}
