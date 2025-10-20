
using CleanArchitecture.Common.Application.Dto;
using TaskSubscription.Core.Models;
namespace TaskSubscription.Application.Subscriptions.Contracts.Dtos
{
    public class SubscriptionDto : BaseDto<SubscriptionDto, Subscription>
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
        public Plan Plan { set; get; }
        public DateTime? CancelledAt { set; get; }
        public SubscriptionStatus Status { set; get; }
        public DateTime EndDate { set; get; }
        public DateTime StartDate { set; get; }
        public int PlanId { set; get; }
    }
}
