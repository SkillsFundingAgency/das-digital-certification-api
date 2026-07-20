using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserById;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetUserByIdResponse
    {
        public Guid UserId { get; set; }
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<UserMatchDetailDto> UserMatches { get; set; } = new List<UserMatchDetailDto>();

        public static implicit operator GetUserByIdResponse?(GetUserByIdQueryResult? source)
        {
            if (source == null) return null;

            return new GetUserByIdResponse
            {
                UserId = source.UserId,
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                CreatedAt = source.CreatedAt,
                LastLoginAt = source.LastLoginAt,
                IsLocked = source.IsLocked,
                UserMatches = source.UserMatches?.Select(um => new UserMatchDetailDto
                {
                    Id = um.Id,
                    Uln = um.Uln,
                    FamilyName = um.FamilyName,
                    DateOfBirth = um.DateOfBirth,
                    EventTime = um.EventTime,
                    CertificateType = um.CertificateType,
                    CourseCode = um.CourseCode,
                    CourseName = um.CourseName,
                    CourseLevel = um.CourseLevel,
                    DateAwarded = um.DateAwarded,
                    ProviderName = um.ProviderName,
                    Ukprn = um.Ukprn,
                    IsMatched = um.IsMatched,
                    IsFailed = um.IsFailed
                }).ToList() ?? new List<UserMatchDetailDto>()
            };
        }
    }

    public class UserMatchDetailDto
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
