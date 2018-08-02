using System;
using System.Collections.Generic;

namespace Seqrus.Web.Services.Authentication
{
    public interface IFailedLoginAttemptsRepository
    {
        void Add(string username, DateTime failureTimestamp);
        IEnumerable<DateTime> Get(string username);
    }
}