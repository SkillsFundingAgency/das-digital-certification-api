using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.DigitalCertificates.Application.Models;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity
{
    public class UpdateUserIdentityCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public List<Name>? Names { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public UpdateUserIdentityCommand(UpdateUserIdentityRequest source, Guid userId)
        {
            UserId = userId;
            Names = source.Names;
            DateOfBirth = source.DateOfBirth;
        }
    }
}