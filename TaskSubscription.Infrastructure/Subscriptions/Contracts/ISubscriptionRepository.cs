
using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Plans.Contracts;

namespace TaskSubscription.Infrastructure.Subscriptions.Contracts
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
    }
}