using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult>
    {
        private readonly IUserEntityContext _userContext;
        private readonly IUserMatchEntityContext _matchContext;

        public GetUserByIdQueryHandler(IUserEntityContext userContext, IUserMatchEntityContext matchContext)
        {
            _userContext = userContext;
            _matchContext = matchContext;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
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

            var matches = await _matchContext.GetByUserIdAsync(request.UserId, cancellationToken);

            var result = new GetUserByIdQueryResult
            {
                UserId = user.Id,
                GovUkIdentifier = user.GovUkIdentifier,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsLocked = user.IsLocked,
                UserMatches = matches.Select(um => new UserMatchDetail
                {
                    Id = um.Id,
                    Uln = um.Uln,
                    FamilyName = um.FamilyName,
                    DateOfBirth = um.DateOfBirth,
                    CertificateType = um.CertificateType?.ToString(),
                    CourseCode = um.CourseCode,
                    CourseName = um.CourseName,
                    CourseLevel = um.CourseLevel,
                    DateAwarded = um.YearAwarded,
                    ProviderName = um.ProviderName,
                    Ukprn = um.Ukprn,
                    IsMatched = um.IsMatched,
                    IsFailed = um.IsFailed
                }).ToList()
            };

            return result;
        }
    }
}
