using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorisation
{
    public class CreateUserAuthorisationCommandHandler : IRequestHandler<CreateUserAuthorisationCommand, Unit>
    {
        private readonly IUserAuthorisationEntityContext _authContext;
        private readonly IUserEntityContext _userContext;
        private readonly IUserIdentityEntityContext _userIdentityContext;
        
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateUserAuthorisationCommandHandler(
            IUserAuthorisationEntityContext authContext,
            IUserEntityContext userContext,
            IUserIdentityEntityContext userIdentityContext,
            IDateTimeProvider dateTimeProvider)
        {
            _authContext = authContext;
            _userContext = userContext;
            _userIdentityContext = userIdentityContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CreateUserAuthorisationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userContext.GetWithIdentitiesAndAuthorisation(request.UserId);

            if (user == null)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.UserId), "UserId not found")
                };

                throw new ValidationException(failures);
            }

            var existingUln = await _authContext.GetByUlnAsync(request.Uln, cancellationToken);
            if (existingUln != null)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.Uln), "ULN already authorised for another user")
                };

                throw new ValidationException(failures);
            }

            if (user.UserAuthorisation != null)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.UserId), "User already has an authorisation")
                };

                throw new ValidationException(failures);
            }

            var entity = new UserAuthorisation
            {
                UserId = request.UserId,
                ULN = request.Uln,
                AuthorisedAt = _dateTimeProvider.Now
            };

            _authContext.Add(entity);

            if (user.UserIdentities != null && user.UserIdentities.Any())
            {
                _userIdentityContext.RemoveRange(user.UserIdentities);
            }

            await _userContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
