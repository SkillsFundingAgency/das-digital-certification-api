using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Models;
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

        public async Task<Sharing?> GetSharingById(Guid sharingId, DateTime now)
        {
            return await Entities
                .AsNoTracking()
                .Include(s => s.SharingAccesses)
                .Include(s => s.SharingEmails)
                .ThenInclude(se => se.SharingEmailAccesses)
                .FirstOrDefaultAsync(s => s.Id == sharingId && s.Status != Enums.SharingStatus.Deleted && s.ExpiryTime > now);
        }

        public async Task<Sharing?> GetSharingByLinkCode(Guid linkCode, DateTime now)
        {
            return await Entities
                .AsNoTracking()
                .Include(s => s.SharingAccesses)
                .Include(s => s.SharingEmails)
                .FirstOrDefaultAsync(s => s.LinkCode == linkCode && s.Status != Enums.SharingStatus.Deleted && s.ExpiryTime > now);
        }

        public async Task<Sharing?> GetSharingByEmailLinkCode(Guid emailLinkCode, DateTime now)
        {
            return await Entities
                .AsNoTracking()
                .Include(s => s.SharingAccesses)
                .Include(s => s.SharingEmails)
                .FirstOrDefaultAsync(s => s.SharingEmails.Any(se => se.EmailLinkCode == emailLinkCode) && s.Status != Enums.SharingStatus.Deleted && s.ExpiryTime > now);
        }

        public async Task<List<Sharing>> GetAllSharings(Guid userId, Guid certificateId)
        {
            return await Entities
                .AsNoTracking()
                .Include(s => s.SharingAccesses)
                .Include(s => s.SharingEmails)
                .ThenInclude(se => se.SharingEmailAccesses)
                .Where(s => s.UserId == userId && s.CertificateId == certificateId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetSharingsCount(Guid userId, Guid certificateId)
        {
            return await Entities
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.CertificateId == certificateId)
                .CountAsync();
        }

        public async Task<List<Sharing>> GetAllSharingsBasic(Guid userId, Guid certificateId)
        {
            return await Entities
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.CertificateId == certificateId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Sharing?> GetSharingByIdTracked(Guid sharingId)
        {
            return await Entities
                .FirstOrDefaultAsync(s => s.Id == sharingId);
        }
    }
}