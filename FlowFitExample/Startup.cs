using System.Net;
using System.Threading.Tasks;
using FlowFitExample.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(e =>
                {
                e.MapControllers();
                // Anything that begins with /api and doesn't match a controller hits a 404 instead of going to the SPA
                e.Map("{*url:regex(^api.*)}", context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return Task.CompletedTask;
                    });
                });

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
