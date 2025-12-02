using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public class CertificateSharingDetails
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public required string CertificateType { get; set; }
        public required string CourseName { get; set; }
        public List<SharingDetail>? Sharings { get; set; }
    }

    public class SharingDetail
    {
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime>? SharingAccess { get; set; }
        public List<SharingEmailDetail>? SharingEmails { get; set; }
    }

    public class SharingEmailDetail
    {
        public Guid SharingEmailId { get; set; }
        public required string EmailAddress { get; set; }
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }
        public List<DateTime>? SharingEmailAccess { get; set; }
    }
}
