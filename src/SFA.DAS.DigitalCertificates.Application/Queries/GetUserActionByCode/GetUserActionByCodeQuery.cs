using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQuery : IRequest<GetUserActionByCodeQueryResult?>
    {
        public required string ActionCode { get; set; }
    }
}
