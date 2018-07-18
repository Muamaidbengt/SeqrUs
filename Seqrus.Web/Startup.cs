using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seqrus.Web.Helpers;

namespace Seqrus.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private ComplianceSettings _complianceLevel;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(_complianceLevel);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }

        private int? GetHttpsPort(IServerAddressesFeature serverAddressesFeature)
        {
            foreach (var address in serverAddressesFeature.Addresses)
            {
                var serverAddress = ServerAddress.FromUrl(address);

                if (serverAddress.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    return serverAddress.Port;
                }
            }

            return null;
        }
    }
}
