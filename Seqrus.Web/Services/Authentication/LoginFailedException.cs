using System;
using System.Runtime.Serialization;

namespace Seqrus.Web.Services.Authentication
{
    [Serializable]
    public sealed class LoginFailedException : Exception
    {
        public LoginFailedException(string message) : base(message)
        {
        }

        private LoginFailedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}