using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserIdentity;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class IdentityNameDto
    {
        public Guid UserIdentityId { get; set; }
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string FamilyName { get; set; } = null!;
        public string GivenNames { get; set; } = null!;
    }

    public class GetUserIdentityResponse
    {
        public IEnumerable<IdentityNameDto>? Identity { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AuthorisationResponse? Authorisation { get; set; }
        public IEnumerable<long>? Excluded { get; set; }

        public static implicit operator GetUserIdentityResponse?(GetUserIdentityQueryResult? source)
        {
            if (source == null) return null;

            return new GetUserIdentityResponse
            {
                Identity = source.Identity?.Select(i => new IdentityNameDto
                {
                    UserIdentityId = i.UserIdentityId,
                    ValidSince = i.ValidSince,
                    ValidUntil = i.ValidUntil,
                    FamilyName = i.FamilyName,
                    GivenNames = i.GivenNames
                }).ToList(),
                DateOfBirth = source.DateOfBirth,
                Authorisation = source.Authorisation,
                Excluded = source.Excluded
            };
        }
    }

    public class AuthorisationResponse
    {
        public Guid AuthorisationId { get; set; }
        public long ULN { get; set; }
        public DateTime AuthorisedAt { get; set; }

        public static implicit operator AuthorisationResponse?(UserAuthorisation? source)
        {
            if (source == null) return null;

            return new AuthorisationResponse
            {
                AuthorisationId = source.AuthorisationId,
                ULN = source.ULN,
                AuthorisedAt = source.AuthorisedAt
            };
        }
    }
}
