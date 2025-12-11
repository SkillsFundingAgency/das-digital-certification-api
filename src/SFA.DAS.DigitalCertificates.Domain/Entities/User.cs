using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }

        public UserAuthorisation? UserAuthorisation { get; set; }

        public IEnumerable<UserIdentity>? UserIdentities { get; set; }
    }
}
