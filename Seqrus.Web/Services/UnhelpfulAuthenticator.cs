using System;

namespace Seqrus.Web.Services
{
    public class UnhelpfulAuthenticator : IAuthenticationService
    {
        private readonly IAuthenticationService _wrappedService;

        public UnhelpfulAuthenticator(IAuthenticationService wrappedService)
        {
            _wrappedService = wrappedService;
        }

        public void Authenticate(string username, string password)
        {
            try
            {
                _wrappedService.Authenticate(username, password);
            }
            catch (Exception)
            {
                throw new LoginFailedException("Login failed!");
            }
        }
    }
}