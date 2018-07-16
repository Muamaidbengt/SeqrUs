using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

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
                // You could also address this in Program.cs, by telling Kestrel to not include the Server header in the first place,
                //
                // e.g.
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

        public static void ConfigureTransportSecurityHeaders(IApplicationBuilder app)
        {
            if (Settings.SensitiveDataExposure)
                return; // ASP.Net is non-compliant by default. You should always encrypt data in transit (i.e. use HTTPS).

            app.Use((context, next) =>
            {
                // Adjust as needed for your app
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
                return next();
            });
        }

        public static void ConfigureCachingHeaders(IApplicationBuilder app)
        {
            if (Settings.SensitiveDataExposure)
                return; // ASP.Net doesn't send any cache headers by default (which MIGHT be non-compliant depending on if the content contains sensitive information or not)

            app.Use((context, next) =>
            {
                var headers = context.Response.GetTypedHeaders();
                headers.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                    NoStore = true
                };
                headers.Headers["Pragma"] = "no-cache";
                return next();
            });
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