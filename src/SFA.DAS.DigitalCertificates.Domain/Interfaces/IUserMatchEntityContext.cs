using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Models;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IUserMatchEntityContext : IEntityContext<UserMatch>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public async Task<List<Guid>> GetPreviouslyAuthorisedUlns(string familyName, DateTime dateOfBirth)
        {
            var matchedUserIds = await Entities
                .AsNoTracking()
                .Where(um => um.IsMatched && um.FamilyName == familyName && um.DateOfBirth == dateOfBirth)
                .Select(um => um.UserId)
                .Distinct()
                .ToListAsync();

            return matchedUserIds ?? new List<Guid>();
        }
    }
}
