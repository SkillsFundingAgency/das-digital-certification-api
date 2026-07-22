using System;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }

        public static implicit operator GetUserResponse?(User? source)
        {
            if (source == null) return null;

            return new GetUserResponse
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                CreatedAt = source.CreatedAt,
                IsLocked = source.IsLocked
            };
        }
    }
}
