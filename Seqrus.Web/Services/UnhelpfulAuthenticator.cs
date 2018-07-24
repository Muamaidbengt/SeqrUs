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
                // Do not emit details to the user
                throw new LoginFailedException("Login failed! ");
            }
        }
    }
}