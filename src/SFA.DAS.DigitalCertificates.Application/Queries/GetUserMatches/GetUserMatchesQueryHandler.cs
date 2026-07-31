using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserMatches
{
    public class GetUserMatchesQueryHandler : IRequestHandler<GetUserMatchesQuery, GetUserMatchesQueryResult>
    {
        private readonly IUserEntityContext _userContext;

        public GetUserMatchesQueryHandler(IUserEntityContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<GetUserMatchesQueryResult> Handle(GetUserMatchesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userContext.GetWithMatchesByUserId(request.UserId);

            if (user == null)
            {
                var failures = new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.UserId), "UserId not found")
                };

                throw new ValidationException(failures);
            }

            var result = new GetUserMatchesQueryResult
            {
                UserId = user.Id,
                GovUkIdentifier = user.GovUkIdentifier,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsLocked = user.IsLocked,
                UserMatches = user.UserMatches?
                    .Select(um => new UserMatchDetail
                    {
                        Id = um.Id,
                        Uln = um.Uln,
                        FamilyName = um.FamilyName,
                        DateOfBirth = um.DateOfBirth,
                        CertificateType = um.CertificateType ?? CertificateType.Unknown,
                        CourseCode = um.CourseCode,
                        CourseName = um.CourseName,
                        CourseLevel = um.CourseLevel,
                        EventTime = um.EventTime,
                        DateAwarded = um.YearAwarded,
                        ProviderName = um.ProviderName,
                        Ukprn = um.Ukprn,
                        IsMatched = um.IsMatched,
                        IsFailed = um.IsFailed
                    })
                    .ToList() ?? []
            };

            return result;
        }
    }
}
