namespace Seqrus.Web.Services.Authentication
{
    public interface IAuthenticationService
    {
        void Authenticate(string username, string password);
    }
}
