using BCrypt.Net;

namespace Seqrus.Web.Services.Authentication
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 11; // YMMV - see https://github.com/BcryptNet/bcrypt.net for more info

        public string GetHash(string plainpassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainpassword, WorkFactor, true);
        }

        public bool VerifyHash(string plainpassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(plainpassword, passwordHash);
        }
    }
}