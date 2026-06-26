using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser
{
    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, UnlockUserCommandResponse>
    {
        private readonly IUserEntityContext _userContext;

        public UnlockUserCommandHandler(IUserEntityContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<UnlockUserCommandResponse> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userContext.GetByUserId(request.UserId);
            if (user == null)
            {
                return new UnlockUserCommandResponse { NotFound = true };
            }

            if (!user.IsLocked)
            {
                return new UnlockUserCommandResponse { Updated = false };
            }

            user.IsLocked = false;
            await _userContext.SaveChangesAsync(cancellationToken);

            return new UnlockUserCommandResponse { Updated = true };
        }
    }
}
