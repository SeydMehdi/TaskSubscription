using Payment.Application.Common;
using Payment.Infrastructure.Common;
using Payment.API.Utils.Config.Auth;
using Payment.Application.Utils;

namespace Payment.API.Common
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

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
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
            //------------------------------------------
            builder.Services.AddControllersWithFilters()
               .AddNewtonsoftJson(option =>
               {
                   option.UseMemberCasing();
               });

            builder.AddJwtAuthentication();
            builder.Services.AddRepositories();
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
