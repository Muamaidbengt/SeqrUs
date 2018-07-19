using System;

namespace Seqrus.Web.Services
{
    public class LoggingAuthenticator : IAuthenticationService
    {
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _wrappedService;

        public LoggingAuthenticator(ILoggingService loggingService, IAuthenticationService wrappedService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _wrappedService = wrappedService ?? throw new ArgumentNullException(nameof(wrappedService));
        }

        public void Authenticate(string username, string password)
        {
            try
            {
                _wrappedService.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                _loggingService.LoginFailed(ex.Message);
                throw;
            }
        }
    }
}