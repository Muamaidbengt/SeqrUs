using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Seqrus.Web.Services;

namespace Seqrus.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly ILoggingService _logger;

        public LogController(ILoggingService logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var logs = _logger.Logs.ToList();

            return View(logs);
        }
    }
}