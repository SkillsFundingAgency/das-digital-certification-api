using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity
{
    public class GetUserIdentityQuery : IRequest<GetUserIdentityQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
