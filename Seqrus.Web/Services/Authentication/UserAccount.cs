namespace Seqrus.Web.Services.Authentication
{
    public class UserAccount
    {
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsAdmin { get; private set; }
        public string SecurityAnswer { get; private set; }

        protected UserAccount(){}

        public UserAccount(string userName, string passwordHash, string securityAnswer, bool isAdmin)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            SecurityAnswer = securityAnswer;
            IsAdmin = isAdmin;
        }
    }
}