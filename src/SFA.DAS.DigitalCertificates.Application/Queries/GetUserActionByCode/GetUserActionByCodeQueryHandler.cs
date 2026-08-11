using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQueryHandler : IRequestHandler<GetUserActionByCodeQuery, GetUserActionByCodeQueryResult?>
    {
        private readonly IUserActionsEntityContext _userActionsContext;

        public GetUserActionByCodeQueryHandler(IUserActionsEntityContext userActionsContext)
        {
            _userActionsContext = userActionsContext;
        }

        public async Task<GetUserActionByCodeQueryResult?> Handle(GetUserActionByCodeQuery request, CancellationToken cancellationToken)
        {
            var ua = await _userActionsContext.GetByActionCodeAsync(request.ActionCode, cancellationToken);

            if (ua == null)
            {
                return null;
            }

            var result = new GetUserActionByCodeQueryResult
            {
                Id = ua.Id,
                UserId = ua.UserId,
                ActionType = ua.ActionType,
                ActionTime = ua.ActionTime,
                FamilyName = ua.FamilyName,
                GivenNames = ua.GivenNames,
                Uln = ua.User?.UserAuthorisation?.ULN,
                CertificateId = ua.CertificateId,
                CertificateType = ua.CertificateType ?? CertificateType.Unknown,
                CourseName = ua.CourseName,
                AdminActions = ua.AdminActions?.OrderByDescending(a => a.ActionTime).Select(a => new AdminActionDetail
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action
                }).ToList(),
                ActionStatus = (ua.AdminActions != null && ua.AdminActions.Any()) ? UserActionStatus.Viewed : UserActionStatus.New
            };

            return result;
        }
    }
}
