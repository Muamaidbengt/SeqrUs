using Microsoft.AspNetCore.Mvc;

namespace Seqrus.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
