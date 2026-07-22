using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetSharingsResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; }
        public List<SharingDetailDto>? Sharings { get; set; }

        public static implicit operator GetSharingsResponse?(SFA.DAS.DigitalCertificates.Domain.Models.CertificateSharings? source)
        {
            if (source == null) return null;

            return new GetSharingsResponse
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                Sharings = source.Sharings?.Select(s => new SharingDetailDto
                {
                    SharingId = s.SharingId,
                    SharingNumber = s.SharingNumber,
                    CreatedAt = s.CreatedAt,
                    LinkCode = s.LinkCode,
                    ExpiryTime = s.ExpiryTime,
                    SharingAccess = s.SharingAccess,
                    SharingEmails = s.SharingEmails == null
                        ? null
                        : s.SharingEmails.Select(e => new SharingEmailDetailDto
                        {
                            SharingEmailId = e.SharingEmailId,
                            EmailAddress = e.EmailAddress,
                            EmailLinkCode = e.EmailLinkCode,
                            SentTime = e.SentTime,
                            SharingEmailAccess = e.SharingEmailAccess
                        }).ToList()
                }).ToList()
            };
        }
    }

    public class SharingDetailDto
    {
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime>? SharingAccess { get; set; }
        public List<SharingEmailDetailDto>? SharingEmails { get; set; }
    }

    public class SharingEmailDetailDto
    {
        public Guid SharingEmailId { get; set; }
        public required string EmailAddress { get; set; }
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }
        public List<DateTime>? SharingEmailAccess { get; set; }
    }
}
