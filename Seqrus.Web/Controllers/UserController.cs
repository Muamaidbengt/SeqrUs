using System;
using Microsoft.AspNetCore.Mvc;
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
                var user = _authenticationService.Authenticate(credentials.Username, credentials.Password);
                return View(user);
            }
            catch (Exception ex)
            {
                credentials.Message = $"Login failed: {ex.Message}";
                return View(nameof(Index), credentials);
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var model = new ResetPasswordModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                _authenticationService.ResetPassword(resetPasswordModel.Username,
                    resetPasswordModel.SecretAnswer, resetPasswordModel.NewPassword);
                return View();
            }
            catch (Exception ex)
            {
                resetPasswordModel.Message = $"Password reset failed: {ex.Message}";
                return View(nameof(ForgotPassword), resetPasswordModel);
            }
        }
    }
}