
using CleanArchitecture.Common.Application.Dto;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;
using TaskSubscription.Core.Models;
namespace TaskSubscription.Application.Plans.Contracts.Dtos
{
    public class PlanDto : BaseDto<PlanDto, Plan>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int DurationDays { get; set; }
        public string Features { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<SubscriptionDto> Subscriptions { get; set; }
    }
}
