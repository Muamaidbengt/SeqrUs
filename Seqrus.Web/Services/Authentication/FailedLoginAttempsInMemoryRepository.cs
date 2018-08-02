using System;
using System.Collections.Generic;
using Seqrus.Web.Services.Authentication;

namespace Seqrus.Web.Services
{
    public class FailedLoginAttempsInMemoryRepository : IFailedLoginAttemptsRepository
    {
        private readonly Dictionary<string, List<DateTime>> _failedAttempts = new Dictionary<string, List<DateTime>>();

        public void Add(string username, DateTime failureTimestamp)
        {
            if (!_failedAttempts.TryAdd(username, new List<DateTime> {failureTimestamp}))
                _failedAttempts[username].Add(failureTimestamp);
        }

        public IEnumerable<DateTime> Get(string username)
        {
            if(!_failedAttempts.TryGetValue(username, out var attempts))
                attempts = new List<DateTime>();
            return attempts;
        }
    }
}