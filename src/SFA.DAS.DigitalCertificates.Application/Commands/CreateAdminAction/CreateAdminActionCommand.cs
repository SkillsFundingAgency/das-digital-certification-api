using MediatR;
using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction
{
    public class CreateAdminActionCommand : IRequest<Unit>
    {
        public required string Username { get; set; }
        public required AdminActionType Action { get; set; }
        public long UserActionId { get; set; }
    }
}
