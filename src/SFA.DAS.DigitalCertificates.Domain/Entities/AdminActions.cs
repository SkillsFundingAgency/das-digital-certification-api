using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class AdminActions
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public AdminActionType Action { get; set; }
        public long UserActionId { get; set; }

        public UserActions? UserAction { get; set; }
    }
}
