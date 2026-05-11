using System;

namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public class NameRecord
    {
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
    }
}
