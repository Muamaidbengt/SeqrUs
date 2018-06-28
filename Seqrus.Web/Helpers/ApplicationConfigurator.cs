using Microsoft.AspNetCore.Builder;

namespace Seqrus.Web.Helpers
{
    public static class ApplicationConfigurator
    {
        public static ComplianceSettings Settings { get; private set; }

        public static void Use(ComplianceSettings settings)
        {
            Settings = settings;
        }

        public static void ConfigureOriginHeaders(IApplicationBuilder app)
        {
            if (Settings.SecurityMisconfiguration)
                return; // Kestrel is non-compliant by default

            app.Use((context, next) =>
            {
                // Alternatively, you could address this in Program.cs, by doing something like
                // WebHostBuilder.UseStartup<Startup>()
                // .UseKestrel(options => options.AddServerHeader = false)

                context.Response.Headers.Remove("Server");
                return next();
            });
        }

        public static void ConfigureErrorHandling(IApplicationBuilder app)
        {
            if (Settings.SecurityMisconfiguration)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/");
            }
        }

        public static void ConfigureContentSecurityPolicy(IApplicationBuilder app)
        {
            if (Settings.CrossSiteScripting)
                return; // ASP.Net Mvc is non-compliant by default

            app.Use((context, next) =>
            {
                // Adjust as needed for your app
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                return next();
            });
        }
    }
}