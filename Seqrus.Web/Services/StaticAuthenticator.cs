using System.Collections.Generic;

namespace Seqrus.Web.Services
{
    public class StaticAuthenticator : IAuthenticationService
    {
        private readonly Dictionary<string, string> _knownAccounts = 
            new Dictionary<string, string> { { "admin", "passw0rd" }};

        public void Authenticate(string username, string password)
        {
            if (username == null)
                throw new LoginFailedException("No username provided");

            if (!_knownAccounts.TryGetValue(username, out var correctPassword))
                throw new LoginFailedException($"No such user: {username}");
            
            if(!correctPassword.Equals(password))
                throw new LoginFailedException($"Incorrect password for user: {username}");
        }
    }
}