using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seqrus.Web.Helpers;
using Seqrus.Web.Services;

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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            _complianceLevel = Configuration
                .GetSection(nameof(ComplianceSettings))
                .Get<ComplianceSettings>();
            ApplicationConfigurator.Use(_complianceLevel);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(_complianceLevel);
            services.AddSingleton<ILoggingService>(new InMemoryLogger());
            ApplicationConfigurator.AddAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ApplicationConfigurator.ConfigureContentSecurityPolicy(app);
            ApplicationConfigurator.ConfigureOriginHeaders(app);
            ApplicationConfigurator.ConfigureTransportSecurityHeaders(app);
            ApplicationConfigurator.ConfigureErrorHandling(app);
            ApplicationConfigurator.ConfigureHttpsRedirection(app, 44395);

            app.UseStaticFiles();

            ApplicationConfigurator.ConfigureCachingHeaders(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.ApplicationServices.GetService<ILoggingService>().ApplicationStarted();
        }
    }
}
