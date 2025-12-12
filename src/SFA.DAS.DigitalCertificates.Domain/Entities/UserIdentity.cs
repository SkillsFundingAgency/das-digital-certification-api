using System;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class UserIdentity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }

        public User? User { get; set; }
    }
}
