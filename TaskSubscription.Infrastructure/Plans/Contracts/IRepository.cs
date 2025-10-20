using CleanArchitecture.Common.Infra.Utilities.GenericRepository.Contracts;
using CleanArchitecture.Common.Core.Common;

namespace TaskSubscription.Infrastructure.Plans.Contracts
{
    public interface IRepository<T> : IRepository<T,EFDataContext> where T :class, IModel
    {

    }
}