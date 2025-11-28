using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class Sharing
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public required string CertificateType { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryTime { get; set; }
        public required string Status { get; set; }

        public User? User { get; set; }
        public ICollection<SharingAccess>? SharingAccesses { get; set; }
        public ICollection<SharingEmail>? SharingEmails { get; set; } = new List<SharingEmail>();
    }
}
