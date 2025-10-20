
using CleanArchitecture.Common.Core.Common;

namespace TaskSubscription.Infrastructure.Common
{
    public class Repository<TEntity> : CleanArchitecture.Common.Infra.Utilities.GenericRepository.Repository<TEntity, EFDataContext>
        where TEntity : class, IModel
    {
        public Repository(EFDataContext dbContext) : base(dbContext)
        {
        }
    }
}
