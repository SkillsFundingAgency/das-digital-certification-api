using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IAdminActionsEntityContext : IEntityContext<AdminActions>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
