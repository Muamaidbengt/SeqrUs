namespace Seqrus.Web.Services.Authentication
{
    public interface IPasswordHasher
    {
        string GetHash(string plainpassword);
        bool VerifyHash(string plainpassword, string passwordHash);
    }
}