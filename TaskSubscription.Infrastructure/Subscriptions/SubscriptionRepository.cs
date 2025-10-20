
using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Common;
using TaskSubscription.Infrastructure.Subscriptions.Contracts;

namespace TaskSubscription.Infrastructure.Subscriptions
{
    public class SubscriptionRepository : Repository<Subscription> , ISubscriptionRepository
    {
        public SubscriptionRepository(EFDataContext dbContext) : base(dbContext)
        {}
    }
}