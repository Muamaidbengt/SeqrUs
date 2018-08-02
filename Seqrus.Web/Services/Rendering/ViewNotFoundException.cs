using System;
using System.Runtime.Serialization;

namespace Seqrus.Web.Services.Rendering
{
    [Serializable]
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string message) : base(message)
        {
        }

        private ViewNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}