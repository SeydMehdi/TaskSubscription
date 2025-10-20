
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CleanArchitecture.Common.Infra.Utilities.GenericRepository.Contracts;

namespace CleanArchitecture.Common.Infra.Utilities.GenericRepository
{
    public abstract class RepositoryBase<T, TDbContext> : IRepositoryBase<T> where T : class where TDbContext : DbContext
    {
        protected TDbContext context { get; set; }

        public RepositoryBase(TDbContext respositoryContext)
        {
            context = respositoryContext;
        }

        public IQueryable<T> FindAll() => context.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            context.Set<T>().Where(expression).AsNoTracking();

        public void Insert(T entity) => MainEntity.Add(entity);
        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public void AddRange(List<T> list)
        {
            context.AddRange(list);
        }

        public void Delete(T entity) => MainEntity.Remove(entity);
        public T FindById(object id)
        {
            return MainEntity.Find(id);
        }
        public async Task<T> FindByIdAsync(object id)
        {
            return await MainEntity.FindAsync(id);
        }

        public DbSet<T> MainEntity { get { return context.Set<T>(); } }
    }
}
