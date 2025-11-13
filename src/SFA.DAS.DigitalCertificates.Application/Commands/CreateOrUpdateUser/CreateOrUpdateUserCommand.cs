using MediatR;
using SFA.DAS.DigitalCertificates.Application.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserCommandResponse>
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public required List<Name> Names { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public static implicit operator CreateOrUpdateUserCommand(CreateOrUpdateUserRequest source)
        {
            return new CreateOrUpdateUserCommand
            {
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                Names = source.Names,
                DateOfBirth = source.DateOfBirth
            };
        }
    }
}