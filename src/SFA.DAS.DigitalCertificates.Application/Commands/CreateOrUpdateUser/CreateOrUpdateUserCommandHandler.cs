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
                    EmailAddress = command.EmailAddress,
                    PhoneNumber = command.PhoneNumber,
                    LastLoginAt = _dateTimeProvider.Now
                };

                _userEntityContext.Add(user);
                await _userEntityContext.SaveChangesAsync();
            }

            // check whether the user is authorized i.e. associated with a Uln - if it is then
            // only the EmailAddress and PhoneNumber should probably be updated? this is
            // a question awaiting alans input - do we care here anyway or does the api just 
            // ignore the names in that case

            // if the names are to be updated then will check if they are different if they 
            // are different should delete the existing ones and create new ones as updating
            // them is pointlessly complicated


            return new CreateOrUpdateUserCommandResponse() { UserId = user.Id };
        }
    }
}