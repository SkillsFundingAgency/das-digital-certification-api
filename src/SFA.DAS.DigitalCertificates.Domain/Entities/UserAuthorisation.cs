using System;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class UserAuthorisation
    {
        public Guid Id { get; set; }
        public Guid UserId {  get; set; }
        public long ULN { get; set; }
        public DateTime AuthorisedAt { get; set; }

        public User? User { get; set; }
    }
}
