using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class NameDto
    {
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; } = null!;
        public required string GivenNames { get; set; } = null!;
    }

    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IEnumerable<NameDto>? Names { get; set; }

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
                IsLocked = source.IsLocked,
                DateOfBirth = source.DateOfBirth,
                Names = source.Names?.Select(n => new NameDto
                {
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList()
            };
        }
    }
}
