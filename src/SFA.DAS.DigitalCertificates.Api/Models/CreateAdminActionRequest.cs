using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateAdminActionRequest
    {
        public required string Username { get; set; }
        public required AdminActionType Action { get; set; }

        public static implicit operator CreateAdminActionCommand(CreateAdminActionRequest source)
        {
            return new CreateAdminActionCommand
            {
                Username = source.Username,
                Action = source.Action
            };
        }
    }
}
