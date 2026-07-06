using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Models
{
    public class UpdateUserIdentityRequest
    {
        public List<Name> Names { get; set; } = [];
        public DateTime DateOfBirth { get; set; }
    }
}
