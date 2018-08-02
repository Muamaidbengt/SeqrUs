using System;

namespace Seqrus.Web.Services.Logging
{
    public struct LogEntry
    {
        public string Category { get; set; }
        public DateTime Timestamp { get; set; }
        public string AdditionalInfo { get; set; }
    }
}