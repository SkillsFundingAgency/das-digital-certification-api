using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserAction
{
    public class GetUserActionByCodeQuery : IRequest<GetUserActionByCodeQueryResult>
    {
        public required string ActionCode { get; set; }
    }
}
