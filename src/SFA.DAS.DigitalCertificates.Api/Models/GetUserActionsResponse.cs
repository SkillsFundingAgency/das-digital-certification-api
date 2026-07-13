using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class UserActionDetailDto
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
        public string? ActionCode { get; set; }
        public IEnumerable<AdminActionResponse>? AdminActions { get; set; }
        public long? Uln { get; set; }
    }

    public class GetUserActionsResponse
    {
        public IEnumerable<UserActionDetailDto> UserActions { get; set; } = new List<UserActionDetailDto>();

        public static implicit operator GetUserActionsResponse(GetUserActionsQueryResult source)
        {
            if (source == null) return null!;

            return new GetUserActionsResponse
            {
                UserActions = source.UserActions?.Select(ua => new UserActionDetailDto
                {
                    Id = ua.Id,
                    UserId = ua.UserId,
                    ActionType = ua.ActionType,
                    ActionTime = ua.ActionTime,
                    ActionStatus = ua.ActionStatus,
                    FamilyName = ua.FamilyName,
                    GivenNames = ua.GivenNames,
                    CertificateId = ua.CertificateId,
                    CertificateType = ua.CertificateType,
                    CourseName = ua.CourseName,
                    ActionCode = ua.ActionCode,
                    Uln = ua.Uln,
                    AdminActions = ua.AdminActions?.Select(a => new AdminActionResponse
                    {
                        Username = a.Username,
                        ActionTime = a.ActionTime,
                        Action = a.Action
                    }).ToList()
                }).ToList() ?? new List<UserActionDetailDto>()
            };
        }
    }
}
