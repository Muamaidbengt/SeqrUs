using System.Linq;

namespace Seqrus.Web.Services
{
    public interface ILoggingService
    {
        void LoginFailed(string username, string sourceIp);
        void ApplicationStarted();

        IQueryable<LogEntry> Logs { get; }
    }
}