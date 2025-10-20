using Microsoft.EntityFrameworkCore;
using TaskSubscription.Infrastructure;

namespace TaskSubscription.RestAPI.Common
{
    public static class APIExtension
    {
        public static IMvcBuilder AddControllersWithFilters(this IServiceCollection services)
        {
            return services.AddControllers(options =>
            {
                // Guarantees to return all results as IResult implementation
                //options.Filters.Add<Payment.API.Filters.ApiResultFilterAttribute>();
            });
        }
        
        public static void AddEFDataContext(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<EFDataContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
        }
    }
}
