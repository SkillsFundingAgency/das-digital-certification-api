using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity
{
    public class UpdateUserIdentityCommandHandler : IRequestHandler<UpdateUserIdentityCommand, Unit>
    {
        private readonly IUserEntityContext _userEntityContext;
        private readonly IUserIdentityEntityContext _userIdentityEntityContext;

        public UpdateUserIdentityCommandHandler(
            IUserEntityContext userEntityContext,
            IUserIdentityEntityContext userIdentityEntityContext)
        {
            _userEntityContext = userEntityContext;
            _userIdentityEntityContext = userIdentityEntityContext;
        }

        public async Task<Unit> Handle(UpdateUserIdentityCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userEntityContext.GetWithIdentitiesByUserId(command.UserId);

            if (user == null)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(command.UserId), "UserId not found")
                };

                throw new ValidationException(failures);
            }

            if (command.Names != null && command.Names.Count > 0)
            {
                if (user.UserIdentities != null)
                {
                    _userIdentityEntityContext.RemoveRange(user.UserIdentities);
                }

                foreach (var name in command.Names)
                {
                    var identity = new UserIdentity
                    {
                        User = user,
                        FamilyName = name.FamilyName,
                        GivenNames = name.GivenNames,
                        DateOfBirth = command.DateOfBirth ?? default,
                        ValidSince = name.ValidSince,
                        ValidUntil = name.ValidUntil
                    };

                    _userIdentityEntityContext.Add(identity);
                }
            }

            await _userEntityContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}