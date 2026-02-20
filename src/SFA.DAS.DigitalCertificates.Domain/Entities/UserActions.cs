using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class UserActions
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public required ActionType ActionType { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseName { get; set; }
        public string? ActionCode { get; set; }
        public DateTime ActionTime { get; set; }
        public ICollection<AdminActions> AdminActions { get; set; } = new List<AdminActions>();
    }
}
