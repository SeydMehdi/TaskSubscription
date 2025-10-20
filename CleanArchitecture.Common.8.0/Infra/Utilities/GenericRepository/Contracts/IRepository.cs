using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Common.Core.Common;

namespace CleanArchitecture.Common.Infra.Utilities.GenericRepository.Contracts
{
    public interface IRepository<TEntity, TDbContext> where TEntity : class, IModel where TDbContext : DbContext
    {
        DbSet<TEntity> Entities { get; }
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }
        TDbContext context { get; set; }

        Task<TEntity> FindById(object id);

        void Add(TEntity dailyReport);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

        void AddRange(IEnumerable<TEntity> entities, bool saveNow = true);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveChanges = true);
        Task UpsertAsync(TEntity entity, CancellationToken cancellationToken, bool saveChanges = true);
        void Delete(TEntity entity, bool saveChanges = true);
        ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
