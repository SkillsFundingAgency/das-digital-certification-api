using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface ISharingEntityContext : IEntityContext<Sharing>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<List<Sharing>> GetAllSharings(Guid userId, Guid certificateId)
        {
            return await Entities
                .Include(s => s.SharingAccesses)
                .Include(s => s.SharingEmails!)
                .ThenInclude(se => se.SharingEmailAccesses)
                .Where(s => s.UserId == userId && s.CertificateId == certificateId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}
