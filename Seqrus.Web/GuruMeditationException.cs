using System;
using System.Runtime.Serialization;

namespace Seqrus.Web
{
    [Serializable]
    public sealed class GuruMeditationException : Exception
    {
        public GuruMeditationException()
        {
        }

        public GuruMeditationException(string message) : base(message)
        {
        }

        private GuruMeditationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
