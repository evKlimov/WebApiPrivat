using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System.Data.SqlClient;
using WebApi.ApplicationLayer;
using WebApi.ApplicationLayer.Executors;
using WebApi.ApplicationLayer.Executors.Commands;
using WebApi.ApplicationLayer.RabbitMQ;
using WebApi.Repository;

namespace WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ISession>(x => new Session(new SqlConnection(Configuration.GetSection("DapperConnection").Value)));

            services.AddScoped<ApiExecutor>();
            services.AddScoped<RpcClient>();

            services.AddScoped<IRepositoryClient, ClientRepository>();
            services.AddScoped<IRepositoryLog, LogRepository>();

            services.AddScoped(typeof(ExecutorCommandApi<RequestClientCommand>), typeof(RequestClientApiExecutor));
            services.AddScoped(typeof(ExecutorCommandApi<ClientCommand>), typeof(ClientApiExecutor));
            services.AddScoped(typeof(ExecutorCommandApi<RequestIdCommand>), typeof(RequestIdApiExecutor));

            services.AddSwaggerExamples();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WebApi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ActionMiddleware>();

            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
