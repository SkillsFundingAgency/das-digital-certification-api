using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand, CreateOrUpdateUserCommandResponse>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserEntityContext _userEntityContext;
        private readonly IUserIdentityEntityContext _userIdentityEntityContext;

        public CreateOrUpdateUserCommandHandler(
            IDateTimeProvider dateTimeProvider,
            IUserEntityContext userEntityContext,
            IUserIdentityEntityContext userIdentityEntityContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _userEntityContext = userEntityContext;
            _userIdentityEntityContext = userIdentityEntityContext;
        }

        public async Task<CreateOrUpdateUserCommandResponse> Handle(CreateOrUpdateUserCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userEntityContext.GetWithIdentities(command.GovUkIdentifier);

            if (user == null)
            {
                user = new User
                {
                    GovUkIdentifier = command.GovUkIdentifier,
                    EmailAddress = command.EmailAddress,
                    CreatedAt = _dateTimeProvider.UtcNow
                };

                _userEntityContext.Add(user);
            }
            else
            {
                user.EmailAddress = command.EmailAddress;
            }

            user.PhoneNumber = command.PhoneNumber;
            user.LastLoginAt = _dateTimeProvider.Now;

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

            return new CreateOrUpdateUserCommandResponse() { UserId = user.Id };
        }
    }
}