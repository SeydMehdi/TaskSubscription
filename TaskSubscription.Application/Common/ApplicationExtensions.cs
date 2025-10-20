
using Microsoft.Extensions.DependencyInjection;
using TaskSubscription.Application.Plans;
using TaskSubscription.Application.Plans.Contracts;
using TaskSubscription.Application.Subscriptions;
using TaskSubscription.Application.Subscriptions.Contracts;


namespace TaskSubscription.Application.Common
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPlanService,PlanService>();
            services.AddScoped<ISubscriptionService,SubscriptionService>();

        }
    }
}
