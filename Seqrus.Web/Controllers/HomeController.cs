using Microsoft.AspNetCore.Mvc;

namespace Seqrus.Web.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}