using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class AdminActionDto
    {
        public required string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public required string Action { get; set; }
    }

    public class GetUserActionByCodeResponse
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime ActionTime { get; set; }
        public required string ActionStatus { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string? CertificateType { get; set; }
        public string? CourseName { get; set; }
        public IEnumerable<AdminActionDto>? AdminActions { get; set; }
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
                ActionStatus = source.ActionStatus.ToString(),
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType?.ToString(),
                CourseName = source.CourseName,
                Uln = source.Uln,
                AdminActions = source.AdminActions?.Select(a => new AdminActionDto
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action.ToString()
                }).ToList()
            };
        }
    }
}
