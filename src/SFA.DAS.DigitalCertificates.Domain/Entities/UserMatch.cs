using System;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class UserMatch
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long? Uln { get; set; }
        public required string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string CertificateType { get; set; }
        public required string CourseCode { get; set; }
        public required string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public required string ProviderName { get; set; }
        public required int Ukprn { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFailed { get; set; }

        public User? User { get; set; }
    }
}
