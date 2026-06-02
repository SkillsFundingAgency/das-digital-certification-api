using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation
{
    public class CreateUserAuthorisationCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public required long Uln { get; set; }
    }
}
