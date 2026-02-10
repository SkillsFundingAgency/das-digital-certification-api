using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Domain.Entities
{
    public class SharingEmail
    {
        public Guid Id { get; set; }
        public Guid SharingId { get; set; }
        public required string EmailAddress { get; set; }
        public Guid EmailLinkCode { get; set; }
        public DateTime SentTime { get; set; }

        public Sharing? Sharing { get; set; }
        public ICollection<SharingEmailAccess>? SharingEmailAccesses { get; set; }
    }
}
