using System;
using Microsoft.AspNetCore.Http;
using Seqrus.Web.Services.Logging;

namespace Seqrus.Web.Services.Authentication
{
    public class LoggingAuthenticator : IAuthenticationService
    {
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _wrappedService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingAuthenticator(ILoggingService loggingService, IAuthenticationService wrappedService, IHttpContextAccessor httpContextAccessor)

        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _wrappedService = wrappedService ?? throw new ArgumentNullException(nameof(wrappedService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public UserAccount Authenticate(string username, string password)
        {
            try
            {
                return _wrappedService.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                _loggingService.LoginFailed(ex.Message, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                throw;
            }
        }

        public void ResetPassword(string username, string secretAnswer, string newPassword)
        {
            try
            {
                _wrappedService.ResetPassword(username, secretAnswer, newPassword);
            }
            catch (Exception ex)
            {
                _loggingService.PasswordResetFailed(ex.Message, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                throw;
            }
        }
    }
}