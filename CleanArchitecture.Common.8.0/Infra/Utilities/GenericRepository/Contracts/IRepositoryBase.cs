using System.Linq.Expressions;

namespace CleanArchitecture.Common.Infra.Utilities.GenericRepository.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        T FindById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void AddRange(List<T> list);
        Task<T> FindByIdAsync(object id);
    }
}
