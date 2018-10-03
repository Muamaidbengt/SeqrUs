using System.Linq;

namespace Seqrus.Web.Services.Logging
{
    public interface ILoggingService
    {
        void LoginFailed(string username, string sourceIp);
        void PasswordResetFailed(string username, string sourceIp);
        void ApplicationStarted();

        IQueryable<LogEntry> Logs { get; }
    }
}