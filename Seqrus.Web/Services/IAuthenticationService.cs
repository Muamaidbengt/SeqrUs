namespace Seqrus.Web.Services
{
    public interface IAuthenticationService
    {
        void Authenticate(string username, string password);
    }
}
