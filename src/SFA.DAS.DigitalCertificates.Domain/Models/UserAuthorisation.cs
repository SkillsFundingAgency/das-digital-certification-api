using System;

namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public class UserAuthorisation
    {
        public Guid AuthorisationId { get; set; }
        public long ULN { get; set; }
        public DateTime AuthorisedAt { get; set; }


        public static implicit operator UserAuthorisation?(Entities.UserAuthorisation? source)
        {
            if (source == null)
            {
                return null;
            }

            return new UserAuthorisation
            {
                AuthorisationId = source.Id,
                ULN = source.ULN,
                AuthorisedAt = source.AuthorisedAt
            };
        }
    }
}
