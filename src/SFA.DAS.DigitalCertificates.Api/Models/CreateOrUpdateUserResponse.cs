using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateOrUpdateUserResponse
    {
        public Guid UserId { get; set; }

        public static implicit operator CreateOrUpdateUserResponse(CreateOrUpdateUserCommandResponse source)
        {
            if (source == null) return null!;

            return new CreateOrUpdateUserResponse
            {
                UserId = source.UserId
            };
        }
    }
}
