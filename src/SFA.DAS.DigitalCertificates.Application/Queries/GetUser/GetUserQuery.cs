using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserQueryResult>
    {
        public string? GovUkIdentifier { get; set; }
    }
}
