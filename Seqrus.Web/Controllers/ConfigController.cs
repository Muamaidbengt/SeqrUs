using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seqrus.Web.Helpers;

namespace Seqrus.Web.Controllers
{
    public class ConfigController : Controller
    {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly ComplianceSettings _settings;

        public ConfigController(ComplianceSettings settings, IApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
            _settings = settings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_settings);
        }

        [HttpPost]
        public IActionResult Configure(ComplianceSettings model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            System.IO.File.WriteAllText("compliancesettings.runtime.json", JsonConvert.SerializeObject(new { ComplianceSettings = model } ));

            _applicationLifetime.StopApplication();
            return View(nameof(Index));
        }
    }
}