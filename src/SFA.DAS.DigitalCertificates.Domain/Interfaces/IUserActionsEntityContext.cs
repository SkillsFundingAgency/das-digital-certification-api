using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IUserActionsEntityContext : IEntityContext<UserActions>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<UserActions?> GetMostRecentActionAsync(Guid userId, ActionType actionType, Guid? certificateId)
        {
            return await Entities
                .AsNoTracking()
                .Include(ua => ua.AdminActions)
                .Where(ua => ua.UserId == userId && ua.ActionType == actionType && ua.CertificateId == certificateId)
                .OrderByDescending(ua => ua.Id)
                .FirstOrDefaultAsync();
        }
    }
}
