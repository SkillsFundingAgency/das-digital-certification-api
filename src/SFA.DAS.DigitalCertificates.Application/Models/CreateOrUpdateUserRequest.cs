using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Models
{
    public class CreateOrUpdateUserRequest
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        
        public List<Name>? Names { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
