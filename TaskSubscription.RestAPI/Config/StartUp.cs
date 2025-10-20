
using Microsoft.AspNetCore.Identity;

using Payment.Core.Models.Identities;
using TaskSubscription.Application.Common;
using TaskSubscription.Application.Utils;
using TaskSubscription.Infrastructure;
using TaskSubscription.Infrastructure.Common.Extensions;
namespace TaskSubscription.RestAPI.Common
{
    public class StartUp
    {
        private WebApplicationBuilder builder;
        private IServiceCollection services;

        public WebApplication app { get; private set; }

        public StartUp(WebApplicationBuilder builder)
        {
            this.builder = builder;
            this.services = builder.Services;
        }

        public StartUp AddServices()
        {
            //-----------------------------------------------------------------------------------
            var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
            //-----------------------------------------------------------------------------------
            builder.Services.AddSingleton(appSettings);
            //------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("mainCors", policy =>
                {
                    var array = builder.Configuration.GetSection("CORSAllowedOrigin").Get<string[]>();
                    policy.WithOrigins(array).AllowAnyHeader().AllowAnyMethod();
                });
            });

            builder.AddEFDataContext();
            services.AddIdentity<AspnetUser, AspnetRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                
                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EFDataContext>()
            .AddDefaultTokenProviders();

            // Add Identity Service


            //------------------------------------------

            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.UseMemberCasing();
            });


            builder.Services.AddRepositories();
            builder.Services.AddUnitOfWorks();
            builder.Services.AddApplicationServices();
            return this;
        }


        public StartUp AddServicesAndBuild()
        {
            AddServices();
            Build();
            return this;
        }
        public StartUp Build()
        {
            this.app = builder.Build();
            return this;
        }
        public StartUp AddPipeLines()
        {
            addSwagger();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("mainCors");
            return this;
        }



        public void Run()
        {
            app.Run();
        }

        private void addSwagger()
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
