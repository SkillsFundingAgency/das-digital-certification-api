using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IUserIdentityEntityContext : IEntityContext<UserIdentity>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
