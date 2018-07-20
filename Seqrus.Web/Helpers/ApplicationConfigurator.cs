using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Seqrus.Web.Services;

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
                //
                // or if using IIS and its RequestFiltering module, by altering the web.config:
                // <configuration>
                //   <system.webServer>
                //     <security>
                //        <requestFiltering removeServerHeader="true" />
                
                context.Response.Headers.Remove(HeaderNames.Server);
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

            // With ASP.NET Core 2.1 or greater you can instead do
            //app.UseHsts()
        }

        public static void ConfigureHttpsRedirection(IApplicationBuilder app, int? port)
        {
            if (Settings.SensitiveDataExposure)
                return; // ASP.Net is non-compliant by default. You should always encrypt data in transit (i.e. use HTTPS).

            app.Use((context, next) =>
            {
                if (context.Request.IsHttps || !port.HasValue)
                    return next();

                var host = context.Request.Host;
                host = new HostString(host.Host, port.Value);

                var request = context.Request;
                var redirectUrl = UriHelper.BuildAbsolute("https",
                    host,
                    request.PathBase,
                    request.Path,
                    request.QueryString);

                context.Response.StatusCode = (int) HttpStatusCode.TemporaryRedirect;
                context.Response.Headers[HeaderNames.Location] = redirectUrl;
                return Task.CompletedTask;
            });

            // With ASP.NET Core 2.1 or greater you can instead do
            //app.UseHttpsRedirection()
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
                    NoStore = true,
                    MustRevalidate = true
                };

                // Note that for backwards compatibility with HTTP1.0 clients you will probably want to add the Pragma header instead/as well.
                // headers.Headers["Pragma"] = "no-cache"
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
                const string csp = "default-src 'self' https://maxcdn.bootstrapcdn.com https://code.jquery.com https://use.fontawesome.com;";

                // Note that you could also use a <meta>-tag to specify the policy
                context.Response.Headers.Add("Content-Security-Policy", csp);
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                return next();
            });
        }

        public static void AddAuthentication(IServiceCollection services)
        {
            services.AddScoped<StaticAuthenticator>();

            services.AddScoped(provider =>
            {
                Func<IAuthenticationService> getAuthenticator = provider.GetService<StaticAuthenticator>;

                if (!Settings.InsufficientLoggingAndMonitoring)
                {
                    // Decorate authenticator with a logger
                    var notLoggingAuth = getAuthenticator;
                    getAuthenticator = () =>
                        new LoggingAuthenticator(provider.GetService<ILoggingService>(), notLoggingAuth());
                }

                if (!Settings.SecurityMisconfiguration)
                {
                    // Decorate the authenticator to strip unneccessary details from error messages
                    var helpfulAuth = getAuthenticator;
                    getAuthenticator = () => new UnhelpfulAuthenticator(helpfulAuth());
                }
                return getAuthenticator();
            });
        }
    }
}