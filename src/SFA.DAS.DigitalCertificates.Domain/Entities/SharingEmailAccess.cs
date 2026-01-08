using System;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class SharingEmailAccess
    {
        public Guid Id { get; set; }
        public Guid SharingEmailId { get; set; }
        public DateTime AccessedAt { get; set; }

        public SharingEmail? SharingEmail { get; set; }
    }
}
