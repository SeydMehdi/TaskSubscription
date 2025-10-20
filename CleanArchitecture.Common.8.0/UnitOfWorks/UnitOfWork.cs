using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Common.Infra.UnitOfWorks.Contracts;

namespace CleanArchitecture.Common.Infra.UnitOfWorks
{
    public class UnitOfWork<EFDataContext> : IUnitOfWork where EFDataContext : DbContext
    {
        private readonly EFDataContext _dataContext;

        public UnitOfWork(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private bool _isTrasanctionBegin;

        public async Task BeginAsync()
        {
            _isTrasanctionBegin = true;
            await _dataContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dataContext.Database.CommitTransactionAsync();
        }

        public async Task CommitPartial()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            await _dataContext.SaveChangesAsync(token);
        }

        public void Rollback()
        {
            _dataContext.Database.RollbackTransaction();
        }

        public async Task RollbackAsync(CancellationToken token = default)
        {
            await _dataContext.Database.RollbackTransactionAsync(token);
        }
    }
}
