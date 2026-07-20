using System;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserAuthorisation;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetUserAuthorisationResponse
    {
        public UserAuthorisationResponse? Authorisation { get; set; }

        public static implicit operator GetUserAuthorisationResponse?(GetUserAuthorisationQueryResult? source)
        {
            if (source == null) return null;

            return new GetUserAuthorisationResponse
            {
                Authorisation = source.Authorisation
            };
        }

        public class UserAuthorisationResponse
        {
            public Guid AuthorisationId { get; set; }
            public long ULN { get; set; }
            public DateTime AuthorisedAt { get; set; }

            public static implicit operator UserAuthorisationResponse?(UserAuthorisation? source)
            {
                if (source == null) return null;

                return new UserAuthorisationResponse
                {
                    AuthorisationId = source.AuthorisationId,
                    ULN = source.ULN,
                    AuthorisedAt = source.AuthorisedAt
                };
            }
        }
    }
}
