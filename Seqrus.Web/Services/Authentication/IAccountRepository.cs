namespace Seqrus.Web.Services.Authentication
{
    public interface IAccountRepository
    {
        bool TryGetAccountByName(string username, out UserAccount userAccount);
    }
}