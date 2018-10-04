using System;
using System.Runtime.Serialization;

namespace Seqrus.Web.Services.Authentication
{
    [Serializable]
    public class PasswordResetFailedException : Exception
    {
        public PasswordResetFailedException(string message) : base(message)
        {
        }

        private PasswordResetFailedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}