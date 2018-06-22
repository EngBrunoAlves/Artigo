using MesGamification.Logger.Api.Repositories;
using MesGamification.Logger.Api.Repositories.Context;
using MesGamification.Logger.Api.Repositories.Interfaces;
using MesGamification.Logger.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MesGamification.Logger.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MesGamificationOptions>(Configuration.GetSection("MesGamificationOptions"));

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //IoC
            services.AddMvc();
            services.AddScoped<MesGamificationContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UserLogService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Cors
            app.UseCors("CorsPolicy");

            //IoC
            app.UseMvc();
        }
    }
}
