using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Seqrus.Web.Services.Authentication
{
    public class ParametrizedAccountRepository : IAccountRepository
    {
        private readonly AccountContext _context;

        public ParametrizedAccountRepository(AccountContext context)
        {
            _context = context;
        }

        public virtual bool TryGetAccountByName(string username, out UserAccount userAccount)
        {
            try
            {
                var accounts = _context.UserAccounts.FromSql(@"
SELECT * 
FROM UserAccounts 
WHERE UserName = '{0}'", 
                    username);
                userAccount = accounts.FirstOrDefault();
                return userAccount != null;
            }
            catch (Exception)
            {
                userAccount = null;
                return false;
            }
        }

        public virtual bool TryUpdatePassword(string username, string secretAnswer, string newPasswordHash)
        {
            try
            {
                var result = _context.Database.ExecuteSqlCommand(@"
UPDATE UserAccounts SET PasswordHash = '{0}' 
WHERE (UserName = '{1}' 
AND SecurityAnswer = '{2}')",
                    newPasswordHash, username, secretAnswer);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}