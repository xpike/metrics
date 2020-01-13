using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XPike.Configuration.Microsoft.AspNetCore;
using XPike.IoC.SimpleInjector.AspNetCore;
using XPike.Logging.Microsoft.AspNetCore;
using XPike.Metrics;
using XPike.Metrics.Console;
using XPike.Metrics.DataDog;

namespace XPikeMetrics22
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.UseMicrosoftConfigurationForXPike();
            var xpike = services.AddXPikeDependencyInjection();
            xpike.AddXPikeDataDogMetrics()
                .AddSingletonToCollection<IMetricsProvider, IConsoleMetricsProvider>(container =>
                    container.ResolveDependency<IConsoleMetricsProvider>());
            xpike.AddXPikeLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseXPikeDependencyInjection()
                .UseXPikeLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
