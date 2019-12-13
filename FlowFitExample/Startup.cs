using System;
using FlowFitExample.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace FlowFitExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Needed to re-inject Newtonsoft.  New System.JSON can't handle polymorphism
            // https://github.com/dotnet/corefx/issues/41338
            // https://github.com/dotnet/corefx/issues/38650
            services.AddMvc()
                .AddNewtonsoftJson();

            services.AddRouting();
            services.AddControllers();

            services.AddSpaStaticFiles(o => o.RootPath = "ClientApp/dist/ClientApp");

            services.AddSingleton<IMeeseeksManager, MeeseeksManager>();
            services.AddSingleton<IScienceManager, ScienceManager>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlowFitExample API", Version = "v1" });
                c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "FlowFitExample.xml", true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlowFitApi Example V1");
                    c.InjectStylesheet("/static/css/swaggerui-dark.css");
                });

            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(e => {
                e.MapControllers();
            });

            // Other route groups are excluded from the SPA middleware to ensure HTTP-level 404s happen
            // Otherwise, invalid routes will end up getting JS errors from the Angular router
            app.MapWhen(context => 
                !(context.Request.Path.Value.StartsWith("/api") ||
                context.Request.Path.Value.StartsWith("/swagger") ||
                context.Request.Path.Value.StartsWith("/static")), builder =>
            {
                builder.UseSpaStaticFiles();
                builder.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    if (env.IsDevelopment()) { spa.UseAngularCliServer("start"); }
                });
            });
        }
    }
}
