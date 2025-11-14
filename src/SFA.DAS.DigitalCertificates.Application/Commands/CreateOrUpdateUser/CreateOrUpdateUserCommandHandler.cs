using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand, CreateOrUpdateUserCommandResponse>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserEntityContext _userEntityContext;

        public CreateOrUpdateUserCommandHandler(
            IDateTimeProvider dateTimeProvider,
            IUserEntityContext userEntityContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _userEntityContext = userEntityContext;
        }

        public async Task<CreateOrUpdateUserCommandResponse> Handle(CreateOrUpdateUserCommand command, CancellationToken cancellationToken)
        {
            User? user = await _userEntityContext.Get(command.GovUkIdentifier);

            if (user == null)
            {
                user = new User
                {
                    GovUkIdentifier = command.GovUkIdentifier,
                    EmailAddress = command.EmailAddress
                };

                _userEntityContext.Add(user);
            }
            else
            {
                user.EmailAddress = command.EmailAddress;
            }

            user.PhoneNumber = command.PhoneNumber;
            user.LastLoginAt = _dateTimeProvider.Now;

            await _userEntityContext.SaveChangesAsync(cancellationToken);

            return new CreateOrUpdateUserCommandResponse() { UserId = user.Id };
        }
    }
}