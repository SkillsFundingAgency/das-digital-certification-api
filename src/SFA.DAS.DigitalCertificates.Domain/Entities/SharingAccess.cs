using System;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class SharingAccess
    {
        public Guid Id { get; set; }
        public Guid SharingId { get; set; }
        public DateTime AccessedAt { get; set; }

        public required Sharing Sharing { get; set; }
    }
}
