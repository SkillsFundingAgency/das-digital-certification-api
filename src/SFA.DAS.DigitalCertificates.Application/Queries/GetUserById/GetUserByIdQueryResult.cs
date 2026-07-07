using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserById
{
    public class GetUserByIdQueryResult
    {
        public Guid UserId { get; set; }
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<UserMatchDetail> UserMatches { get; set; } = new List<UserMatchDetail>();
    }

    public class UserMatchDetail
    {
        public Guid Id { get; set; }
        public long? Uln { get; set; }
        public required string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EventTime { get; set; }
        public CertificateType CertificateType { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? CourseLevel { get; set; }
        public int? DateAwarded { get; set; }
        public string? ProviderName { get; set; }
        public int? Ukprn { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFailed { get; set; }
    }
}
