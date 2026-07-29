using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class UpdateUserIdentityRequest
    {
        public List<NameRequest>? Names { get; set; }
        public DateTime DateOfBirth { get; set; }

        public static implicit operator Application.Models.UpdateUserIdentityRequest(UpdateUserIdentityRequest source)
        {
            return new Application.Models.UpdateUserIdentityRequest
            {
                Names = source.Names?.Select(n => new Application.Models.Name
                {
                    UserIdentityId = n.UserIdentityId,
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList() ?? new List<Application.Models.Name>(),
                DateOfBirth = source.DateOfBirth
            };
        }
    }

    public class NameRequest
    {
        public Guid UserIdentityId { get; set; }
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
    }
}
