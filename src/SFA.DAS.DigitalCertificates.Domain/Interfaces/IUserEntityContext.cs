﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.DigitalCertificates.Domain.Entities;
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
    }
}
