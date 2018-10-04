using Microsoft.EntityFrameworkCore;

namespace Seqrus.Web.Services.Authentication
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
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
    }
}