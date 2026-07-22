using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.DigitalCertificates.Application.Models;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserCommandResponse>
    {
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}