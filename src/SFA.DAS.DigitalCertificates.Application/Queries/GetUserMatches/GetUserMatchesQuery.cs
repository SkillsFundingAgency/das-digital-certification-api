using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserMatches
{
    public class GetUserMatchesQuery : IRequest<GetUserMatchesQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
