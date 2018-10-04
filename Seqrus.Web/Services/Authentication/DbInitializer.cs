using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Seqrus.Web.Services.Authentication
{
    public static class DbInitializer
    {
        public static void CreateAndSeedDatabase(IServiceProvider services)
        {
            var accountDb = services.GetRequiredService<AccountContext>();
            var hasher = services.GetRequiredService<IPasswordHasher>();
            
            accountDb.Database.EnsureDeleted();
            accountDb.Database.EnsureCreated();

            if (accountDb.UserAccounts.Any())
                return;

            accountDb.UserAccounts.Add(new UserAccount("admin", hasher.GetHash("passw0rd"), "Yokel", true));
            accountDb.UserAccounts.Add(new UserAccount("kim", hasher.GetHash("s3cr3t"), "Hacker", false));
            accountDb.SaveChanges();
        }
    }
}