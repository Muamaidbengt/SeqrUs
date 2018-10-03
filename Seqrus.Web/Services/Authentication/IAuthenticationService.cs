namespace Seqrus.Web.Services.Authentication
{
    public interface IAuthenticationService
    {
        UserAccount Authenticate(string username, string password);
        void ResetPassword(string username, string secretAnswer, string newPassword);
    }
}
