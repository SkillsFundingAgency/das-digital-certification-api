using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Application.Models;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryResult
    {
        public IEnumerable<Name>? Identity { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserAuthorisation? Authorisation { get; set; }
        public IEnumerable<long>? Excluded { get; set; }
    }
}
