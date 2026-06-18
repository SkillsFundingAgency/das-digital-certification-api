using System;
using MediatR;
// TODO: Align the namespace and folder structure. This will be addressed during cleanup, as there are dependent branches in the chain.
namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQuery : IRequest<GetUserActionsQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
