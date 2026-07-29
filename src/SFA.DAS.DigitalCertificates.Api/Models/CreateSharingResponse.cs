using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateSharingResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; } = null!;
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }

        public static implicit operator CreateSharingResponse(CreateSharingCommandResponse source)
        {
            if (source == null) return null!;

            return new CreateSharingResponse
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                SharingId = source.SharingId,
                SharingNumber = source.SharingNumber,
                CreatedAt = source.CreatedAt,
                LinkCode = source.LinkCode,
                ExpiryTime = source.ExpiryTime
            };
        }
    }
}
