using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seqrus.Web.Helpers;
using Seqrus.Web.Services;
using Seqrus.Web.Services.Authentication;
using Seqrus.Web.Services.Logging;
using Seqrus.Web.Services.Rendering;

namespace Seqrus.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ComplianceSettings _complianceLevel;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("compliancesettings.json")
                .AddJsonFile("compliancesettings.runtime.json", optional: true);

            Configuration = builder.Build();
            _complianceLevel = Configuration
                .GetSection(nameof(ComplianceSettings))
                .Get<ComplianceSettings>();
            ConfigurableCountermeasures.Use(_complianceLevel);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddSingleton(_complianceLevel);
            services.AddSingleton<ILoggingService, InMemoryLogger>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ConfigurableCountermeasures.AddAntiforgery(services);
            ConfigurableCountermeasures.AddAuthentication(services);

            DbInitializer.CreateAndSeedDatabase(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigurableCountermeasures.ConfigureContentSecurityPolicy(app);
            ConfigurableCountermeasures.ConfigureOriginServerHeaders(app);
            ConfigurableCountermeasures.ConfigureTransportSecurityHeaders(app);
            ConfigurableCountermeasures.ConfigureErrorHandling(app);
            ConfigurableCountermeasures.ConfigureHttpsRedirection(app, Configuration.GetValue<int>("Bindings:https"));

            app.UseStaticFiles();

            ConfigurableCountermeasures.ConfigureCachingHeaders(app);
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=About}/{action=Index}/{id?}");
            });

            app.ApplicationServices.GetService<ILoggingService>().ApplicationStarted();
        }
    }
}
