using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingCommandHandler : IRequestHandler<CreateCertificateSharingCommand, CreateCertificateSharingCommandResponse>
    {
        private readonly ISharingEntityContext _sharingContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ApplicationSettings _settings;

        public CreateCertificateSharingCommandHandler(
        ISharingEntityContext sharingContext,
        IDateTimeProvider dateTimeProvider,
        IOptions<ApplicationSettings> settings)
        {
            _sharingContext = sharingContext;
            _dateTimeProvider = dateTimeProvider;
            _settings = settings.Value;
        }

        public async Task<CreateCertificateSharingCommandResponse> Handle(CreateCertificateSharingCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.Now;
            var expiryDays = _settings.CertificateSharingExpiryDays;
            var expiryTime = now.AddDays(expiryDays);
            var linkCode = Guid.NewGuid();

            var allSharings = await _sharingContext.GetAllSharings(request.UserId, request.CertificateId);
            var sharingNumber = allSharings.Count + 1;

            var sharing = new Sharing
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CertificateId = request.CertificateId,
                CourseName = request.CourseName,
                CertificateType = request.CertificateType,
                LinkCode = linkCode,
                CreatedAt = now,
                ExpiryTime = expiryTime,
                Status = SharingStatus.Live.ToString()
            };

            _sharingContext.Add(sharing);
            await _sharingContext.SaveChangesAsync(cancellationToken);

            return new CreateCertificateSharingCommandResponse
            {
                UserId = request.UserId,
                CertificateId = request.CertificateId,
                CertificateType = request.CertificateType,
                CourseName = request.CourseName,
                SharingId = sharing.Id,
                SharingNumber = sharingNumber,
                CreatedAt = sharing.CreatedAt,
                LinkCode = sharing.LinkCode,
                ExpiryTime = sharing.ExpiryTime
            };
        }
    }
}
