using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SFA.DAS.DigitalCertificates.Domain.Interfaces
{
    public interface IEntityContext<T> where T : class
    {
        DbSet<T> Entities { get; }

        EntityEntry<T> Add(T entity) => Entities.Add(entity);
        EntityEntry<T> Remove(T entity) => Entities.Remove(entity);
        void RemoveRange(IEnumerable<T> entities) => Entities.RemoveRange(entities);
    }
}
