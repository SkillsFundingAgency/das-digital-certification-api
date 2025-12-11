using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommandHandler : IRequestHandler<CreateSharingCommand, CreateSharingCommandResponse>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ApplicationSettings _settings;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public CreateSharingCommandHandler(
        ISharingEntityContext sharingContext,
        IDateTimeProvider dateTimeProvider,
        IOptions<ApplicationSettings> settings)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
            _settings = settings.Value;
        }

        public async Task<CreateSharingCommandResponse> Handle(CreateSharingCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var now = _dateTimeProvider.Now;
                var expiryDays = _settings.CertificateSharingExpiryDays;
                var expiryTime = now.AddDays(expiryDays);
                var linkCode = Guid.NewGuid();

                var sharing = new Sharing
                {
                    UserId = request.UserId,
                    CertificateId = request.CertificateId,
                    CourseName = request.CourseName,
                    CertificateType = request.CertificateType,
                    LinkCode = linkCode,
                    CreatedAt = now,
                    ExpiryTime = expiryTime,
                    Status = SharingStatus.Live
                };

                _sharingContext.Add(sharing);
                await _sharingContext.SaveChangesAsync(cancellationToken);

                var sharingsCount = await _sharingContext.GetSharingsCount(request.UserId, request.CertificateId);

                return new CreateSharingCommandResponse
                {
                    UserId = request.UserId,
                    CertificateId = request.CertificateId,
                    CertificateType = request.CertificateType,
                    CourseName = request.CourseName,
                    SharingId = sharing.Id,
                    SharingNumber = sharingsCount,
                    CreatedAt = sharing.CreatedAt,
                    LinkCode = sharing.LinkCode,
                    ExpiryTime = sharing.ExpiryTime
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
