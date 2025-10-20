
using CleanArchitecture.Common.Application.Results;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;

namespace TaskSubscription.Application.Subscriptions.Contracts
{
    public interface ISubscriptionService
    {
        ValueTask<ListResult<SubscriptionDto>> GetSubscriptionList(SubscriptionSearchDto searchItem, CancellationToken token);
        Task<ObjectResult<SubscriptionDto>> Delete(SubscriptionDto dto, CancellationToken token);
        Task<ObjectResult<SubscriptionDto>> SaveChanges(SubscriptionDto dto, CancellationToken token);
        ValueTask<ObjectResult<SubscriptionDto>> GetSubscriptionInfo(GetByIdDto dto, CancellationToken token);
    }
}