
using CleanArchitecture.Common.Application.Results;
using TaskSubscription.Application.Plans.Contracts.Dtos;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;

namespace TaskSubscription.Application.Plans.Contracts
{
    public interface IPlanService
    {

        ValueTask<ListResult<PlanDto>> GetPlanList(PlanSearchDto searchItem, CancellationToken token);
        Task<ObjectResult<PlanDto>> Delete(PlanDto dto, CancellationToken token);
        Task<ObjectResult<PlanDto>> SaveChanges(PlanDto dto, CancellationToken token);
        ValueTask<ObjectResult<PlanDto>> GetPlanInfo(GetByIdDto dto, CancellationToken token);

    }
}