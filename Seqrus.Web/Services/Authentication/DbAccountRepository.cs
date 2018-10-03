using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Seqrus.Web.Services.Authentication
{
    public class DbAccountRepository : DbContext, IAccountRepository
    {
        public DbAccountRepository(DbContextOptions<DbAccountRepository> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(acc => acc.UserName);
            });
        }

        public bool TryGetAccountByName(string username, out UserAccount userAccount)
        {
            try
            {
                var sql = $"SELECT * FROM UserAccounts WHERE UserName = '{username}'";
                var accounts = UserAccounts.FromSql(sql);
                userAccount = accounts.FirstOrDefault();
                return userAccount != null;
            }
            catch (Exception)
            {
                userAccount = null;
                return false;
            }
        }

        public bool UpdatePassword(string username, string secretAnswer, string newPasswordHash)
        {
            try
            {
                var sql = $"UPDATE UserAccounts SET PasswordHash = '{newPasswordHash}' WHERE (UserName = '{username}' AND SecurityAnswer = '{secretAnswer}')";
                var result = Database.ExecuteSqlCommand(sql);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}