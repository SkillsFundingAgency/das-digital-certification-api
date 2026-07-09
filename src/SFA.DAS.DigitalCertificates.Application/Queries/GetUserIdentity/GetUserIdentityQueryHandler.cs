using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Models;
using SFA.DAS.DigitalCertificates.Application.Models;
using System.Collections.Generic;
using FluentValidation.Results;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity
{
    public class GetUserIdentityQueryHandler : IRequestHandler<GetUserIdentityQuery, GetUserIdentityQueryResult>
    {
        private readonly IUserEntityContext _userContext;
        private readonly IUserMatchEntityContext _matchContext;
        private readonly IUserAuthorisationEntityContext _authContext;

        public GetUserIdentityQueryHandler(
            IUserEntityContext userContext,
            IUserMatchEntityContext matchContext,
            IUserAuthorisationEntityContext authContext)
        {
            _userContext = userContext;
            _matchContext = matchContext;
            _authContext = authContext;
        }

        public async Task<GetUserIdentityQueryResult> Handle(GetUserIdentityQuery request, CancellationToken cancellationToken)
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

            var identities = user.UserIdentities?.Select(i => new Name
            {
                UserIdentityId = i.Id,
                ValidSince = i.ValidSince,
                ValidUntil = i.ValidUntil,
                FamilyName = i.FamilyName,
                GivenNames = i.GivenNames
            }).OrderByDescending(n => n.ValidSince).ToList();

            var excluded = new List<long>();

            var dob = user.UserIdentities?.OrderByDescending(i => i.ValidSince).FirstOrDefault()?.DateOfBirth;

            if (user.UserAuthorisation == null)
            {
                if (user.UserIdentities != null)
                {
                    var allUserIds = new List<Guid>();

                    foreach (var ui in user.UserIdentities)
                    {
                        if (string.IsNullOrWhiteSpace(ui.FamilyName)) continue;

                        var userIdsForIdentity = await _matchContext.GetPreviouslyAuthorisedUlns(ui.FamilyName, ui.DateOfBirth);
                        if (userIdsForIdentity?.Any() == true)
                        {
                            allUserIds.AddRange(userIdsForIdentity);
                        }
                    }

                    if (allUserIds.Any())
                    {
                        var authorised = await _authContext.GetAuthorisedUlns(allUserIds);
                        if (authorised?.Any() == true)
                        {
                            excluded = authorised.Distinct().ToList();
                        }
                    }
                }
            }

            return new GetUserIdentityQueryResult
            {
                Identity = identities,
                DateOfBirth = dob,
                Authorisation = (UserAuthorisation?)user.UserAuthorisation,
                Excluded = excluded
            };
        }
    }
}
