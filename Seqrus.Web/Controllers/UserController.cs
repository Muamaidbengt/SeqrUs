using System;
using Microsoft.AspNetCore.Mvc;
using Seqrus.Web.Services;
using Seqrus.Web.Services.Authentication;
using Seqrus.Web.ViewModels;

namespace Seqrus.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginModel credentials)
        {
            try
            {
                _authenticationService.Authenticate(credentials.Username, credentials.Password);
                return View(credentials);
            }
            catch (Exception ex)
            {
                credentials.Message = $"Login failed: {ex.Message}";
                return View(nameof(Index), credentials);
            }
        }
    }
}