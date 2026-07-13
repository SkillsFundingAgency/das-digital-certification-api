using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class AdminActionResponse
    {
        public required string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public required AdminActionType Action { get; set; }
    }
}
