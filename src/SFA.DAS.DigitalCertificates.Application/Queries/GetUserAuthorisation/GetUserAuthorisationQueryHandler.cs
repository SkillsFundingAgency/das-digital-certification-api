using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation
{
    public class GetUserAuthorisationQueryHandler : IRequestHandler<GetUserAuthorisationQuery, GetUserAuthorisationQueryResult>
    {
        private readonly IUserEntityContext _userContext;

        public GetUserAuthorisationQueryHandler(IUserEntityContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<GetUserAuthorisationQueryResult> Handle(GetUserAuthorisationQuery request, CancellationToken cancellationToken)
        {
            UserAuthorisation? userAuthorisation = await _userContext.GetUserAuthorisationByUserId(request.UserId);

            return new GetUserAuthorisationQueryResult
            {
                Authorisation = (Domain.Models.UserAuthorisation?)userAuthorisation
            };
        }
    }
}
