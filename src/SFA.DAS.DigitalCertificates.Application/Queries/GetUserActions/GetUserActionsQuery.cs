using System;
using MediatR;
namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQuery : IRequest<GetUserActionsQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
