using System;
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

        [HttpGet]
        public IActionResult FailDatabaseConnection()
        {
            // This is compiled into our source code so noone will ever see it
            var secretConnectionString = "Server=8.8.8.8;User Id=S4nt4;Password=R00d0lph;";
            throw new ApplicationException("Unable to connect to database");
        }
    }
}