using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class Sharing
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryTime { get; set; }
        public SharingStatus Status { get; set; }

        public User? User { get; set; }
        public ICollection<SharingAccess> SharingAccesses { get; set; } = new List<SharingAccess>();
        public ICollection<SharingEmail> SharingEmails { get; set; } = new List<SharingEmail>();
    }
}
