using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Plans.Contracts;

namespace TaskSubscription.Infrastructure.Common.Contracts
{
    public interface IPlanRepository : IRepository<Plan>
    {
    }
}