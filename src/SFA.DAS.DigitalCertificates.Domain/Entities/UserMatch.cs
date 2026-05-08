using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class UserMatch
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long? Uln { get; set; }
        public required string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? CourseLevel { get; set; }
        public int? YearAwarded { get; set; }
        public string? ProviderName { get; set; }
        public int? Ukprn { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFailed { get; set; }

        public User? User { get; set; }
    }
}
