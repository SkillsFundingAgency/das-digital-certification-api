using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IUserEntityContext : IEntityContext<User>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<User?> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();

        public async Task<User?> Get(string govUkIdentifer)
            => await Entities
                .FirstOrDefaultAsync(er => er.GovUkIdentifier == govUkIdentifer);

        public async Task<User?> GetByUserId(Guid userId)
            => await Entities
                .FirstOrDefaultAsync(er => er.Id == userId);

        public async Task<UserAuthorisation?> GetUserAuthorisationByUserId(Guid userId)
        {
            var user = await Entities
                .Include(u => u.UserAuthorisation)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.UserAuthorisation;
        }
    }
}
