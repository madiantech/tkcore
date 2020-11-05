using System;

namespace YJC.Toolkit.Sys
{
    public class ToolkitException : Exception, IExceptionInfo
    {
        private readonly object fErrorObject;

        protected ToolkitException()
        {
        }

        public ToolkitException(string message, object errorObject)
            : base(message)
        {
            TkDebug.AssertArgumentNullOrEmpty(message, "message", null);

            fErrorObject = errorObject;
            if (BaseGlobalVariable.Current != null)
                Context = BaseGlobalVariable.Current.ObjectContext.Clone();
        }

        public ToolkitException(string message, Exception innerException, object errorObject)
            : base(message, innerException)
        {
            TkDebug.AssertArgumentNullOrEmpty(message, "message", null);
            TkDebug.AssertArgumentNull(innerException, "innerException", null);

            fErrorObject = errorObject;
            if (BaseGlobalVariable.Current != null)
                Context = BaseGlobalVariable.Current.ObjectContext.Clone();
        }

        #region IExceptionInfo 成员

        public virtual void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);
        }

        #endregion

        public object ErrorObject
        {
            get
            {
                return fErrorObject;
            }
        }

        public TkObjectContext Context { get; private set; }

        public override string ToString()
        {
            if (fErrorObject == null)
                return base.ToString();
            else
                return string.Format(ObjectUtil.SysCulture,
                    "对象{0}的例外", fErrorObject);
        }
    }
}
