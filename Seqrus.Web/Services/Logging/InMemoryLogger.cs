using System;
using System.Collections.Generic;
using System.Linq;

namespace Seqrus.Web.Services.Logging
{
    public class InMemoryLogger : ILoggingService
    {
        private readonly List<LogEntry> _logEntries = new List<LogEntry>();

        public void LoginFailed(string username, string sourceIp)
        {
            _logEntries.Add(new LogEntry
            {
                Category = nameof(LoginFailed),
                AdditionalInfo = $"User: {username}, Ip: {sourceIp}",
                Timestamp = DateTime.Now
            });
        }

        public void PasswordResetFailed(string username, string sourceIp)
        {
            _logEntries.Add(new LogEntry
            {
                Category = nameof(PasswordResetFailed),
                AdditionalInfo = $"User: {username}, Ip: {sourceIp}",
                Timestamp = DateTime.Now
            });
        }

        public void ApplicationStarted()
        {
            _logEntries.Add(new LogEntry
            {
                Category = nameof(ApplicationStarted),
                AdditionalInfo = "and it was a great success",
                Timestamp = DateTime.Now
            });
        }

        public IQueryable<LogEntry> Logs => _logEntries.AsQueryable();
    }
}