using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByLinkCode
{
    public class GetSharingByLinkCodeQuery : IRequest<GetSharingByLinkCodeQueryResult>
    {
        public Guid LinkCode { get; set; }
    }
}
