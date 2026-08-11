using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateUserActionResponse
    {
        public required string ActionCode { get; set; }

        public static implicit operator CreateUserActionResponse(CreateUserActionCommandResponse source)
        {
            if (source == null) return null!;
            return new CreateUserActionResponse { ActionCode = source.ActionCode };
        }
    }
}
