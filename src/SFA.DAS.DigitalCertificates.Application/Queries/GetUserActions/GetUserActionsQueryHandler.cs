using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryHandler : IRequestHandler<GetUserActionsQuery, GetUserActionsQueryResult>
    {
        private readonly IUserActionsEntityContext _userActionsContext;

        public GetUserActionsQueryHandler(IUserActionsEntityContext userActionsContext)
        {
            _userActionsContext = userActionsContext;
        }

        public async Task<GetUserActionsQueryResult> Handle(GetUserActionsQuery request, CancellationToken cancellationToken)
        {
            var actions = await _userActionsContext.GetByUserIdAsync(request.UserId, cancellationToken);

            var result = new GetUserActionsQueryResult
            {
                UserActions = actions.Select(ua => new UserActionDetail
                {
                    Id = ua.Id,
                    UserId = ua.UserId,
                    ActionType = ua.ActionType,
                    ActionTime = ua.ActionTime,
                    ActionCode = ua.ActionCode,
                    FamilyName = ua.FamilyName,
                    GivenNames = ua.GivenNames,
                    Uln = ua.User?.UserAuthorisation?.ULN,
                    CertificateId = ua.CertificateId,
                    CertificateType = ua.CertificateType,
                    CourseName = ua.CourseName,
                    AdminActions = ua.AdminActions?.OrderByDescending(a => a.ActionTime).Select(a => new AdminActionDetail
                    {
                        Username = a.Username,
                        ActionTime = a.ActionTime,
                        Action = a.Action.ToString()
                    }).ToList(),
                    ActionStatus = (ua.AdminActions != null && ua.AdminActions.Any()) ? UserActionStatus.Viewed : UserActionStatus.New
                }).ToList()
            };

            return result;
        }
    }
}
