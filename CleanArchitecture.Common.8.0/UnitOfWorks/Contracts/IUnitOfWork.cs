namespace CleanArchitecture.Common.Infra.UnitOfWorks.Contracts
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task CommitPartial();
        Task CommitAsync();
        void Rollback();
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken token = default);
        Task RollbackAsync(CancellationToken token = default);
    }
}
