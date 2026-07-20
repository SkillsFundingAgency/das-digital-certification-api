using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetSharingByIdResponse
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public Enums.CertificateType CertificateType { get; set; }
        public required string CourseName { get; set; } = null!;
        public Guid SharingId { get; set; }
        public int SharingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid LinkCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<DateTime>? SharingAccess { get; set; }
        public List<SharingEmailDetailGetById>? SharingEmails { get; set; }

        public static implicit operator GetSharingByIdResponse(CertificateSharing source)
        {
            if (source == null) return null!;

            return new GetSharingByIdResponse
            {
                UserId = source.UserId,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                SharingId = source.SharingId,
                SharingNumber = source.SharingNumber,
                CreatedAt = source.CreatedAt,
                LinkCode = source.LinkCode,
                ExpiryTime = source.ExpiryTime,
                SharingAccess = source.SharingAccess == null ? null : new List<DateTime>(source.SharingAccess),
                SharingEmails = source.SharingEmails == null
                    ? null
                    : source.SharingEmails.Select(e => new SharingEmailDetailGetById
                    {
                        SharingEmailId = e.SharingEmailId,
                        EmailAddress = e.EmailAddress,
                        EmailLinkCode = e.EmailLinkCode,
                        SentTime = e.SentTime,
                        SharingEmailAccess = e.SharingEmailAccess == null ? null : new List<DateTime>(e.SharingEmailAccess)
                    }).ToList()
            };
        }
    }

    public class SharingEmailDetailGetById
    {
        public Guid SharingEmailId { get; set; }
        public required string EmailAddress { get; set; } = null!;
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }
        public List<DateTime>? SharingEmailAccess { get; set; }
    }
}
