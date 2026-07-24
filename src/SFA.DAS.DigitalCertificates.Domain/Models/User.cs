using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string GovUkIdentifier { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IEnumerable<NameRecord>? Names { get; set; }

        public static implicit operator User?(Entities.User? source)
        {
            if (source == null)
            {
                return null;
            }

            var dob = source.UserIdentities?.OrderByDescending(i => i.ValidSince).FirstOrDefault()?.DateOfBirth;

            var names = source.UserIdentities?
                .Select(i => new NameRecord
                {
                    ValidSince = i.ValidSince,
                    ValidUntil = i.ValidUntil,
                    FamilyName = i.FamilyName,
                    GivenNames = i.GivenNames
                })
                .OrderByDescending(n => n.ValidSince)
                .ToList();

            return new User
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                EmailAddress = source.EmailAddress,
                CreatedAt = source.CreatedAt,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                IsLocked = source.IsLocked,
                DateOfBirth = dob,
                Names = names
            };
        }
    }
}