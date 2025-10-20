
using TaskSubscription.Application.Common;
using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Utilities.Mapping;
namespace TaskSubscription.Application.Plans.Contracts.Dtos
{
    public class PlanSearchDto : BaseSearchItem
    { 
        public bool IsSearchByName { get;set; }
		public string  Name { get;set; }
		public bool IsSearchByDescription { get;set; }
		public string  Description { get;set; }
		public bool IsSearchByPrice { get;set; }
		public decimal  Price { get;set; }
		public bool IsSearchByCurrency { get;set; }
		public string  Currency { get;set; }
		public bool IsSearchByDurationDays { get;set; }
		public int  DurationDays { get;set; }
		public bool IsSearchByFeatures { get;set; }
		public string  Features { get;set; }
    }
}
