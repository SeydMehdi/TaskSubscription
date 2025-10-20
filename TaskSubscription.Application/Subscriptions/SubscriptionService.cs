
using TaskSubscription.Application.Subscriptions.Contracts;
using TaskSubscription.Infrastructure.Subscriptions.Contracts;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Common.Application.Results;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;
using CleanArchitecture.Common.Infra.Common;
using Microsoft.EntityFrameworkCore;
using TaskSubscription.Core.Models;
namespace TaskSubscription.Application.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private ILogger<SubscriptionService> logger;
        private ISubscriptionRepository subscriptionRepo;

        public SubscriptionService(ILogger<SubscriptionService> logger,
            ISubscriptionRepository subscriptionRepo)
        {
            this.logger = logger;
            this.subscriptionRepo = subscriptionRepo;
        }

        public async ValueTask<ListResult<SubscriptionDto>> GetSubscriptionList(SubscriptionSearchDto searchItem, CancellationToken token)
        {
            var result = new ListResult<SubscriptionDto>(logger);
            try
            {
                var query = subscriptionRepo.TableNoTracking;



                if (searchItem.IsSearchByPlanId &&
                    searchItem.PlanId > -1)
                {
                    query = query.Where(m =>
                    m.PlanId == searchItem.PlanId);
                }

                if (searchItem.IsSearchByStartDate)
                {
                    query = query.Where(m => m.StartDate >= searchItem.FromDate &&
                    m.StartDate <= searchItem.ToDate);
                }

                if (searchItem.IsSearchByEndDate)
                {
                    query = query.Where(m => m.EndDate >= searchItem.FromDate &&
                    m.EndDate <= searchItem.ToDate);
                }

                if (searchItem.IsSearchByStatus)
                {
                    query = query.Where(m =>
                    m.Status == searchItem.Status);
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
        public async Task<ObjectResult<SubscriptionDto>> Delete(SubscriptionDto dto, CancellationToken token)
        {
            var result = new ObjectResult<SubscriptionDto>(logger);
            try
            {
                var entity = dto.ToEntity();
                await subscriptionRepo.DeleteAsync(entity, token, true);
                return result.SuccessMap(" درخواست مورد نظر حذف گردید.", entity);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }
        public async Task<ObjectResult<SubscriptionDto>> SaveChanges(SubscriptionDto dto, CancellationToken token)
        {
            var result = new ObjectResult<SubscriptionDto>(logger);
            try
            {
                var entity = dto.ToEntity();
                await subscriptionRepo.UpsertAsync(entity, token, true);
                return result.SuccessMap(" تغییرات با موفقیت اعمال گردید.", entity);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }
        public async ValueTask<ObjectResult<SubscriptionDto>> GetSubscriptionInfo(GetByIdDto dto, CancellationToken token)
        {
            var result = new ObjectResult<SubscriptionDto>(logger);
            try
            {
                var query = await subscriptionRepo.FindById(dto.Id);
                return result.SuccessMap("اطلاعات با موفقیت دریافت شد.", query);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان دریافت اطلاعات وجود ندارد.");
            }
        }


        public async Task<ObjectResult<SubscriptionDto>> ActivatePlanForCurrentUser(ActivatePlanDto dto, CancellationToken token)
        {
            var result = new ObjectResult<SubscriptionDto>(logger);
            try
            {
                // Validate input
                if (dto.PlanId <= 0)
                    return result.Error("شناسه طرح معتبر نیست.");

                if (dto.UserId == Guid.Empty)
                    return result.Error("شناسه کاربر معتبر نیست.");

                // Check if user already has an active subscription
                var existingActive = await subscriptionRepo.TableNoTracking
                    .FirstOrDefaultAsync(s =>
                        s.UserId == dto.UserId &&
                        s.Status == SubscriptionStatus.Active, token);

                // Optionally: deactivate old one or throw error
                if (existingActive != null)
                {
                    // Option 1: Allow only one active → return error
                    // return result.Error("کاربر هم‌اکنون یک اشتراک فعال دارد.");

                    // Option 2: Deactivate current and activate new (common)
                    existingActive.Status = SubscriptionStatus.Inactive;
                    existingActive.EndDate = DateTime.UtcNow;
                    await subscriptionRepo.UpdateAsync(existingActive, token);
                }

                // Create new subscription
                var newSubscription = new SubscriptionEntity
                {
                    UserId = dto.UserId,
                    PlanId = dto.PlanId,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1), // or get from Plan service
                    Status = SubscriptionStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await subscriptionRepo.InsertAsync(newSubscription, token, autoSave: true);

                var dtoResult = newSubscription.ToDto(); // Ensure you have this extension
                return result.SuccessMap("طرح با موفقیت فعال شد.", dtoResult);
            }
            catch (Exception exc)
            {
                return result.Error(exc, "امکان فعال‌سازی طرح وجود ندارد.");
            }
        }


    }
}
