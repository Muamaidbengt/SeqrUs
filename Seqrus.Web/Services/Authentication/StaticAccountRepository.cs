using System.Collections.Generic;

namespace Seqrus.Web.Services.Authentication
{
    /// <summary>
    /// For testing purposes only, this class contains a set of user accounts and their password hashes.
    /// The account details are hard-coded
    /// </summary>
    public class StaticAccountRepository : IAccountRepository
    {
        private readonly IPasswordHasher _hasher;

        public StaticAccountRepository(IPasswordHasher hasher)
        {
            _hasher = hasher;

            _knownAccounts.Add("admin", new UserAccount("admin", _hasher.GetHash("passw0rd"), "Yokel", true));
        }

        private readonly Dictionary<string, UserAccount> _knownAccounts = new Dictionary<string, UserAccount>();

        public bool TryGetAccountByName(string username, out UserAccount userAccount)
        {
            return _knownAccounts.TryGetValue(username, out userAccount);
        }

        public bool TryUpdatePassword(string username, string secretAnswer, string newPasswordHash)
        {
            throw new System.NotImplementedException();
        }
    }
}