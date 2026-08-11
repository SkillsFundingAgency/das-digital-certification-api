using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System.Collections.Generic;
using FluentValidation.Results;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateAdminAction
{
    public class CreateAdminActionCommandHandler : IRequestHandler<CreateAdminActionCommand, Unit>
    {
        private readonly IAdminActionsEntityContext _adminActionsContext;
        private readonly IUserActionsEntityContext _userActionsContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateAdminActionCommandHandler(IAdminActionsEntityContext adminActionsContext, IUserActionsEntityContext userActionsContext, IDateTimeProvider dateTimeProvider)
        {
            _adminActionsContext = adminActionsContext;
            _userActionsContext = userActionsContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CreateAdminActionCommand request, CancellationToken cancellationToken)
        {
            var exists = await _userActionsContext.ExistsAsync(request.UserActionId, cancellationToken);

            if (!exists)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.UserActionId), "UserActionId not found")
                };

                throw new ValidationException(failures);
            }

            var entity = new AdminActions
            {
                Username = request.Username,
                Action = request.Action,
                ActionTime = _dateTimeProvider.Now,
                UserActionId = request.UserActionId
            };

            _adminActionsContext.Add(entity);
            await _adminActionsContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
