using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Models
{
    public class UpdateUserIdentityRequest
    {
        public List<IdentityName> Names { get; set; } = [];
        public DateTime DateOfBirth { get; set; }
    }

    public class IdentityName
    {
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
    }
}
