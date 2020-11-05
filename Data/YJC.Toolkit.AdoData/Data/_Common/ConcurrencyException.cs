using System;
using System.Data;
using System.Runtime.Serialization;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(DBConcurrencyException exception)
            : base("T", exception) //base(TkCore.ConcurrentError, exception)
        {
            TkDebug.AssertArgumentNull(exception, "exception", null);
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public DBConcurrencyException Exception
        {
            get
            {
                return InnerException as DBConcurrencyException;
            }
        }
    }
}
