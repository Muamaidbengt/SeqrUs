using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seqrus.Web.Helpers;

namespace Seqrus.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureServices((context, services) =>
                {
                    services.Configure<KestrelServerOptions>(opts => 
                        opts.AddServerHeader = 
                        context.Configuration.GetValue<bool>($"{nameof(ComplianceSettings)}:{nameof(ComplianceSettings.SecurityMisconfiguration)}"));
                })
                .Build();
    }
}
