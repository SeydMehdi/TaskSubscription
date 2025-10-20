
using CleanArchitecture.Common.Application.Dto;
using TaskSubscription.Application.Common;
using TaskSubscription.Core.Models;
namespace TaskSubscription.Application.Subscriptions.Contracts.Dtos
{
    public class SubscriptionSearchDto : BaseSearchItem
    { 

		
        public bool IsSearchByUserId { get;set; }
		public bool IsSearchByPlanId { get;set; }
		public int  PlanId { get;set; }
		public bool IsSearchByStartDate { get;set; }
		public DateTime  StartDate { get;set; }
		public bool IsSearchByEndDate { get;set; }
		public DateTime  EndDate { get;set; }
		public bool IsSearchByStatus { get;set; }
		public SubscriptionStatus  Status { get;set; }
    }
}
