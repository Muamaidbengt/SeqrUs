using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Seqrus.Web.Services.Logging;

namespace Seqrus.Web.Controllers
{
    public class AuditController : Controller
    {
        private readonly ILoggingService _logger;

        public AuditController(ILoggingService logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Logs()
        {
            var logs = _logger.Logs.ToList();

            return View(logs);
        }
    }
}