using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByEmailLinkCode
{
    public class GetSharingByEmailLinkCodeQuery : IRequest<GetSharingByEmailLinkCodeQueryResult>
    {
        public Guid EmailLinkCode { get; set; }
    }
}
