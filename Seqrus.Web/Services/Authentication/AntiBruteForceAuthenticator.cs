using System;
using System.Linq;

namespace Seqrus.Web.Services.Authentication
{
    public class AntiBruteForceAuthenticator : IAuthenticationService
    {
        private readonly IAuthenticationService _wrappedAuthenticationService;
        private readonly IFailedLoginAttemptsRepository _failedLoginAttemptsRepository;
        
        private static readonly TimeSpan SlidingExpiration = TimeSpan.FromSeconds(60);
        private const int MaxAttemptsBeforeLockout = 3;

        public AntiBruteForceAuthenticator(
            IAuthenticationService wrappedAuthenticationService, 
            IFailedLoginAttemptsRepository failedLoginAttemptsRepository)
        {
            _wrappedAuthenticationService = wrappedAuthenticationService 
                                            ?? throw new ArgumentNullException(nameof(wrappedAuthenticationService));
            _failedLoginAttemptsRepository = failedLoginAttemptsRepository
                ?? throw new ArgumentNullException(nameof(failedLoginAttemptsRepository));
        }

        public UserAccount Authenticate(string username, string password)
        {
            if (IsLocked(username))
                throw new LoginFailedException($"Account is temporarily locked: {username}");
            try
            {
                return _wrappedAuthenticationService.Authenticate(username, password);
            }
            catch (Exception)
            {
                _failedLoginAttemptsRepository.Add(username, DateTime.Now);
                throw;
            }
        }

        public void ResetPassword(string username, string secretAnswer, string newPassword)
        {
            if (IsLocked(username))
                throw new LoginFailedException($"Account is temporarily locked: {username}");
            try
            {
                _wrappedAuthenticationService.ResetPassword(username, secretAnswer, newPassword);
            }
            catch (Exception)
            {
                _failedLoginAttemptsRepository.Add(username, DateTime.Now);
                throw;
            }
        }

        private bool IsLocked(string username)
        {
            var oldTimes = DateTime.Now.Subtract(SlidingExpiration);
            return _failedLoginAttemptsRepository
                       .Get(username)
                       .Count(attempt => attempt > oldTimes) >= MaxAttemptsBeforeLockout;
        }
    }
}