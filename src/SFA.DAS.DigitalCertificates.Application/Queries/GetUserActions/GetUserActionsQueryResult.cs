using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryResult
    {
        public IEnumerable<UserActionDetail> UserActions { get; set; } = new List<UserActionDetail>();
    }
}
