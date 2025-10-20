
using TaskSubscription.Application.Plans.Contracts;
using Microsoft.Extensions.Logging;
using TaskSubscription.Infrastructure.Common.Contracts;
using CleanArchitecture.Common.Application.Results;
using TaskSubscription.Application.Plans.Contracts.Dtos;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;
namespace TaskSubscription.Application.Plans
{
    public class PlanService : IPlanService
    {
        private ILogger<PlanService> logger;
        private IPlanRepository planRepo;

        public PlanService(ILogger<PlanService> logger,
            IPlanRepository planRepo)
        {
            this.logger = logger;
            this.planRepo = planRepo;
        }
        
        public async ValueTask<ListResult<PlanDto>> GetPlanList(PlanSearchDto searchItem, CancellationToken token)
        {
            var result = new ListResult<PlanDto>(logger);
            try
            {
                var query = planRepo.TableNoTracking;
                
                        if (searchItem.IsSearchByName && 
                            !string.IsNullOrWhiteSpace(searchItem.Name))
                        {
                            query = query.Where(m =>
                            m.Name.Contains(searchItem.Name));
                        }

                        if (searchItem.IsSearchByDescription && 
                            !string.IsNullOrWhiteSpace(searchItem.Description))
                        {
                            query = query.Where(m =>
                            m.Description.Contains(searchItem.Description));
                        }

                        if (searchItem.IsSearchByPrice)
                        {
                            query = query.Where(m =>
                            m.Price == searchItem.Price);
                        }

                        if (searchItem.IsSearchByCurrency && 
                            !string.IsNullOrWhiteSpace(searchItem.Currency))
                        {
                            query = query.Where(m =>
                            m.Currency.Contains(searchItem.Currency));
                        }

                        if (searchItem.IsSearchByDurationDays && 
                            searchItem.DurationDays > -1)
                        {
                            query = query.Where(m =>
                            m.DurationDays == searchItem.DurationDays);
                        }

                        if (searchItem.IsSearchByFeatures && 
                            !string.IsNullOrWhiteSpace(searchItem.Features))
                        {
                            query = query.Where(m =>
                            m.Features.Contains(searchItem.Features));
                        }

                result.info = query.Count();
                query = query.AddPaging(searchItem);
                return await result.SuccessMapAsync("اطلاعات با موفقیت دریافت شد.", query, token);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }
        public async Task<ObjectResult<PlanDto>> Delete(PlanDto dto, CancellationToken token)
        {
            var result = new ObjectResult<PlanDto>(logger);
            try
            {
                var entity = dto.ToEntity();
                await planRepo.DeleteAsync(entity, token, true);
                return result.SuccessMap(" درخواست مورد نظر حذف گردید.", entity);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }
        public async Task<ObjectResult<PlanDto>> SaveChanges(PlanDto dto, CancellationToken token)
        {
            var result = new ObjectResult<PlanDto>(logger);
            try
            {
                var entity = dto.ToEntity();
                await planRepo.UpsertAsync(entity, token, true);
                return result.SuccessMap(" تغییرات با موفقیت اعمال گردید.", entity);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }
        public async ValueTask<ObjectResult<PlanDto>> GetPlanInfo(GetByIdDto dto, CancellationToken token)
        {
            var result = new ObjectResult<PlanDto>(logger);
            try
            {
                var query = await planRepo.FindById(dto.Id);
                return result.SuccessMap("اطلاعات با موفقیت دریافت شد.", query);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }

        
    }
}
