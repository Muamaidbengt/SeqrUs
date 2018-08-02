namespace Seqrus.Web.Services.Authentication
{
    public class UserAccount
    {
        public string UserName { get; }
        public string PasswordHash { get; }

        public UserAccount(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }
}