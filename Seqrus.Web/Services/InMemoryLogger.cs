using System;
using System.Collections.Generic;
using System.Linq;

namespace Seqrus.Web.Services
{
    public class InMemoryLogger : ILoggingService
    {
        private readonly List<LogEntry> _logEntries = new List<LogEntry>();

        public void LoginFailed(string username)
        {
            _logEntries.Add(new LogEntry
            {
                Category = nameof(LoginFailed),
                AdditionalInfo = username,
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

    public struct LogEntry
    {
        public string Category { get; set; }
        public DateTime Timestamp { get; set; }
        public string AdditionalInfo { get; set; }
    }
}