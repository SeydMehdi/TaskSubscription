
using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Common;
using TaskSubscription.Infrastructure.Common.Contracts;

namespace TaskSubscription.Infrastructure.Plans
{
    public class PlanRepository : Repository<Plan> , IPlanRepository
    {
        public PlanRepository(EFDataContext dbContext) : base(dbContext)
        {}
    }
}