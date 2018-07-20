using Microsoft.AspNetCore.Mvc;

namespace Seqrus.Web.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}