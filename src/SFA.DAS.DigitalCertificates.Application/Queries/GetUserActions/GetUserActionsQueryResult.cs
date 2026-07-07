using System;
using System.Collections.Generic;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryResult
    {
        public IEnumerable<UserActionDetail> UserActions { get; set; } = new List<UserActionDetail>();
    }

    public class UserActionDetail
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime ActionTime { get; set; }
        public UserActionStatus ActionStatus { get; set; } = UserActionStatus.New;
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseName { get; set; }
        public string? ActionCode { get; set; }
        public IEnumerable<AdminActionDetail>? AdminActions { get; set; }
        public long? Uln { get; set; }
    }

    public class AdminActionDetail
    {
        public required string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public required string Action { get; set; }
    }
}
