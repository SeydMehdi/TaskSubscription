using CleanArchitecture.Common.Core.Common;
using CleanArchitecture.Common.Core.Utils;
using CleanArchitecture.Common.Infra.Utilities.GenericRepository.Contracts;
using Microsoft.EntityFrameworkCore;


namespace CleanArchitecture.Common.Infra.Utilities.GenericRepository
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity, TDbContext>
    where TEntity : class, IModel where TDbContext : DbContext
    {
        public TDbContext context { get; set; }
        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();
        public void Add(TEntity dailyReport)
        {
            Entities.Add(dailyReport);
            context.SaveChanges();
        }

        public async Task<TEntity> FindById(object id)
        {
            return await Entities.FindAsync(id);
        }


        public void Delete(TEntity entity, bool saveChanges = true)
        {
            Entities.Remove(entity);
            if (saveChanges)
                context.SaveChanges();
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveChanges = true)
        {
            Entities.Remove(entity);
            if (saveChanges)
                await context.SaveChangesAsync(cancellationToken);
        }

        public Repository(TDbContext dbContext)
        {
            context = dbContext;
            Entities = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync
            (CancellationToken cancellationToken,
            params object[] ids)
        {
            return await Entities.FindAsync(ids, cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync
            (CancellationToken cancellationToken,
            object ids)
        {
            return await Entities.FindAsync(ids, cancellationToken);
        }

        public virtual void Add(
            TEntity entity,
            bool saveNow = true)
        {
            Entities.Add(entity);
            if (saveNow)
                context.SaveChanges();
        }


        public virtual async Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken,
            bool saveNow = true)
        {
            await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            if (saveNow)
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            if (saveNow)
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveChanges = true)
        {
            Entities.Update(entity);
            if (saveChanges)
                await context.SaveChangesAsync(cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            Entities.UpdateRange(entities);
            if (saveNow)
                await context.SaveChangesAsync(cancellationToken);
        }


        public virtual async Task UpsertAsync(TEntity entity, CancellationToken cancellationToken, bool saveChanges = true)
        {
            TEntity exist = await GetById(entity);
            if (exist != null)
            {
                entity.MapTo(exist);
                await UpdateAsync(exist, cancellationToken, true);
            }
            else
            {
                await AddAsync(entity, cancellationToken, true);
            }
        }

        public async Task<TEntity> GetById(TEntity entity)
        {
            TEntity exist = null;
            if (entity is IModel<long>)
            {
                var model = (IModel<long>)entity;
                exist = await FindById(model.Id);
            }
            if (entity is IModel<int>)
            {
                var model = (IModel<int>)entity;
                exist = await FindById(model.Id);
            }
            if (entity is IModel<Guid>)
            {
                var model = (IModel<Guid>)entity;
                exist = await FindById(model.Id);
            }

            if (entity is IModel<string>)
            {
                var model = (IModel<string>)entity;
                exist = await FindById(model.Id);
            }

            return exist;
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            Entities.RemoveRange(entities);
            if (saveNow)
                await context.SaveChangesAsync(cancellationToken);
        }

        public void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Entities.AddRange(entities);
            if (saveNow)
                context.SaveChanges();
        }


    }
}
