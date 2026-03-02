using System;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface ISharingEmailEntityContext : IEntityContext<SharingEmail>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<SharingEmail?> GetSharingEmailByIdTracked(Guid sharingEmailId)
        {
            return await Entities
                .FirstOrDefaultAsync(se => se.Id == sharingEmailId);
        }
    }
}
