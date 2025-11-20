using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation
{
    public class GetUserAuthorisationQuery : IRequest<GetUserAuthorisationQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
