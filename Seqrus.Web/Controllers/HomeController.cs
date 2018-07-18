using Microsoft.AspNetCore.Mvc;
using Seqrus.Web.Helpers;

namespace Seqrus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ComplianceSettings _settings;

        public HomeController(ComplianceSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FailDatabaseConnection()
        {
            // For extra security, the connection string is compiled into our source code so noone will ever see it
            const string secretConnectionString = "Server=8.8.8.8;User Id=S4nt4;Password=R00d0lph;";
            throw new GuruMeditationException("Unable to connect to database");
        }

        [HttpGet]
        public IActionResult Risks()
        {
            return View(_settings);
        }

        [HttpGet]
        public IActionResult Secrets()
        {
            return View();
        }
    }
}