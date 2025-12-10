using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly IUserEntityContext _userContext;

        public GetUserQueryHandler(IUserEntityContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User? user = request.GovUkIdentifier != null ? await _userContext.Get(request.GovUkIdentifier) : null;
            if (user == null && Guid.TryParse(request.GovUkIdentifier, out Guid userId))
            {
                user = await _userContext.GetByUserId(userId);
            }

            return new GetUserQueryResult
            {
                User = (Domain.Models.User?)user
            };
        }
    }
}
