using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Encoding;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommandHandler : IRequestHandler<CreateUserActionCommand, CreateUserActionCommandResponse>
    {
        private readonly IUserActionsEntityContext _userActionsContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEncodingService _encodingService;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public CreateUserActionCommandHandler(IUserActionsEntityContext userActionsContext, IDateTimeProvider dateTimeProvider, IEncodingService encodingService)
        {
            _userActionsContext = userActionsContext;
            _dateTimeProvider = dateTimeProvider;
            _encodingService = encodingService ?? throw new ArgumentNullException(nameof(encodingService));
        }

        public async Task<CreateUserActionCommandResponse> Handle(CreateUserActionCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var mostRecent = await _userActionsContext.GetMostRecentActionAsync(request.UserId, request.ActionType, request.CertificateId);

                if (mostRecent != null)
                {
                    var isNew = mostRecent.AdminActions == null || !mostRecent.AdminActions.Any();
                    if (isNew)
                    {
                        return new CreateUserActionCommandResponse
                        {
                            ActionCode = mostRecent.ActionCode ?? string.Empty
                        };
                    }
                }

                var now = _dateTimeProvider.Now;

                var userAction = new UserActions
                {
                    UserId = request.UserId,
                    ActionType = request.ActionType,
                    FamilyName = request.FamilyName,
                    GivenNames = request.GivenNames,
                    CertificateId = request.CertificateId,
                    CertificateType = request.CertificateType,
                    CourseName = request.CourseName,
                    ActionTime = now,
                    
                };

                _userActionsContext.Add(userAction);
                await _userActionsContext.SaveChangesAsync(cancellationToken);

                var hashedActionCode = _encodingService.Encode(userAction.Id, EncodingType.AccountId);
                userAction.ActionCode = hashedActionCode;

                await _userActionsContext.SaveChangesAsync(cancellationToken);

                return new CreateUserActionCommandResponse
                {
                    ActionCode = hashedActionCode
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }

        
    }
}
