using System;
using System.Runtime.Serialization;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class RazorRuntimeException : Exception, IExceptionInfo
    {
        private readonly object fObject;

        internal RazorRuntimeException(object razorObj, Exception innerException)
            : base(innerException.Message, innerException)
        {
            fObject = razorObj;
        }

        protected RazorRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #region IExceptionInfo 成员

        public void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);

            try
            {
                Type objType = fObject.GetType();
                info.ErrorObjType = objType.ToString();
                info.ErrorObj = new Uri(objType.Assembly.CodeBase).LocalPath;
            }
            catch
            {
            }
        }

        #endregion
    }
}
