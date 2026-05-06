using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IUserAuthorisationEntityContext : IEntityContext<UserAuthorisation>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<List<long>> GetAuthorisedUlns(IEnumerable<Guid> userIds)
        {
            if (userIds == null || !userIds.Any()) return new List<long>();

            return await Entities
                .AsNoTracking()
                .Where(ua => userIds.Contains(ua.UserId))
                .Select(ua => ua.ULN)
                .Distinct()
                .ToListAsync();
        }

        public async Task<UserAuthorisation?> GetByUlnAsync(long uln, CancellationToken cancellationToken = default)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ULN == uln, cancellationToken);
        }

        public async Task<UserAuthorisation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        }
    }
}
