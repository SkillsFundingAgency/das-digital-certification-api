using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Encoding;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;

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
            _encodingService = encodingService;
        }

        public async Task<CreateUserActionCommandResponse> Handle(CreateUserActionCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                var dbContext = _userActionsContext as DbContext
                    ?? throw new InvalidOperationException(
                        "IUserActionsEntityContext must be a DbContext to support a single transactional save.");

                var strategy = dbContext.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                    var mostRecent = await _userActionsContext.GetMostRecentActionAsync(
                        request.UserId,
                        request.ActionType,
                        request.CertificateId);

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

                    var hashedActionCode = _encodingService.Encode(userAction.Id, EncodingType.SupportReference);
                    userAction.ActionCode = hashedActionCode;

                    await _userActionsContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);

                    return new CreateUserActionCommandResponse
                    {
                        ActionCode = hashedActionCode
                    };
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}