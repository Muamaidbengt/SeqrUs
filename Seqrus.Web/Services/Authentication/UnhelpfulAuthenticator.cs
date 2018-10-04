using System;

namespace Seqrus.Web.Services.Authentication
{
    public class UnhelpfulAuthenticator : IAuthenticationService
    {
        private readonly IAuthenticationService _wrappedService;

        public UnhelpfulAuthenticator(IAuthenticationService wrappedService)
        {
            _wrappedService = wrappedService;
        }

        public UserAccount Authenticate(string username, string password)
        {
            try
            {
                return _wrappedService.Authenticate(username, password);
            }
            catch (Exception)
            {
                // Do not emit details to the user
                throw new LoginFailedException("Login failed! ");
            }
        }

        public void ResetPassword(string username, string secretAnswer, string newPassword)
        {
            try
            {
                _wrappedService.ResetPassword(username, secretAnswer, newPassword);
            }
            catch (Exception)
            {
                // Do not emit details to the user
                throw new PasswordResetFailedException("Password reset failed! ");
            }
        }
    }
}