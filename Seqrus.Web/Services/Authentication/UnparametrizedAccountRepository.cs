using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Seqrus.Web.Services.Authentication
{
    public class UnparametrizedAccountRepository : IAccountRepository
    {
        private readonly AccountContext _context;

        public UnparametrizedAccountRepository(AccountContext context)
        {
            _context = context;
        }

        public virtual bool TryGetAccountByName(string username, out UserAccount userAccount)
        {
            try
            {
                var sql = $"SELECT * FROM UserAccounts WHERE UserName = '{username}'";
                var accounts = _context.UserAccounts.FromSql(sql);
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
                var sql = $"UPDATE UserAccounts SET PasswordHash = '{newPasswordHash}' WHERE (UserName = '{username}' AND SecurityAnswer = '{secretAnswer}')";
                var result = _context.Database.ExecuteSqlCommand(sql);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}