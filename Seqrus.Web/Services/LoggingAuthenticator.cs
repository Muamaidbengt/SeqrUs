using Microsoft.AspNetCore.Http;
using System;

namespace Seqrus.Web.Services
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

        public void Authenticate(string username, string password)
        {
            try
            {
                _wrappedService.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                _loggingService.LoginFailed(ex.Message, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                throw;
            }
        }
    }
}