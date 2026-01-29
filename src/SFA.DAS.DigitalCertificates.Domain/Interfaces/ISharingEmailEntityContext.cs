using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface ISharingEmailEntityContext : IEntityContext<SharingEmail>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
