using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest<UnlockUserCommandResponse>
    {
        public required Guid UserId { get; set; }
    }
}
