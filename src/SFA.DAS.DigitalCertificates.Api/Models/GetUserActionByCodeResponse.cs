using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetUserActionByCodeResponse
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime ActionTime { get; set; }
        public UserActionStatus ActionStatus { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseName { get; set; }
        public IEnumerable<AdminActionResponse>? AdminActions { get; set; }
        public long? Uln { get; set; }

        public static implicit operator GetUserActionByCodeResponse(GetUserActionByCodeQueryResult source)
        {
            if (source == null) return null!;

            return new GetUserActionByCodeResponse
            {
                Id = source.Id,
                UserId = source.UserId,
                ActionType = source.ActionType,
                ActionTime = source.ActionTime,
                ActionStatus = source.ActionStatus,
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName,
                Uln = source.Uln,
                AdminActions = source.AdminActions?.Select(a => new AdminActionResponse
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action
                }).ToList()
            };
        }
    }
}
