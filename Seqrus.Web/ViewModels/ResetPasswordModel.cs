namespace Seqrus.Web.ViewModels
{
    public class ResetPasswordModel
    {
        public string Username { get; set; }
        public string SecretAnswer { get; set; }
        public string NewPassword { get; set; }
        public string Message { get; set; }
    }
}