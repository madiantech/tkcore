using System;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    public class ExceptionInfo
    {
        public class ContextInfo
        {
            public ContextInfo()
            {
            }

            public ContextInfo(string type, string content)
            {
                Type = type;
                Content = content;
            }

            public string Type { get; private set; }

            public string Content { get; private set; }
        }

        protected internal ExceptionInfo()
        {
            OtherInfos = new Dictionary<string, string>();
        }

        [SimpleElement]
        public string Message { get; protected set; }

        [SimpleElement]
        public string ErrorSource { get; protected set; }

        [SimpleElement]
        public string StackTrace { get; protected set; }

        [SimpleElement]
        public string Type { get; protected set; }

        [SimpleElement]
        public string TargetSite { get; protected set; }

        [SimpleElement]
        public string ErrorObjType { get; set; }

        [SimpleElement]
        public string ErrorObj { get; set; }

        [SimpleElement]
        public string Argument { get; set; }

        [Dictionary]
        public Dictionary<string, string> OtherInfos { get; protected set; }

        [ObjectElement(IsMultiple = true)]
        public List<ContextInfo> Context { get; private set; }

        protected virtual void SetInfo(Exception exception)
        {
            TkDebug.AssertArgumentNull(exception, "exception", this);

            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Type = exception.GetType().ToString();
        }

        public void FillInfo(Exception exception)
        {
            SetInfo(exception);
        }

        public void FillInfo(ToolkitException exception)
        {
            SetInfo(exception);

            if (exception.ErrorObject != null)
            {
                ErrorObj = exception.ErrorObject.ToString();
                ErrorObjType = exception.ErrorObject.GetType().ToString();
            }
            if (exception.Context != null)
            {
                Context = (from item in exception.Context
                           where item != null
                           select new ContextInfo(item.GetType().ToString(), item.ToString())).ToList();
            }
            ArgumentException argExpt = exception as ArgumentException;
            if (argExpt != null)
                Argument = argExpt.Argument;
        }
    }
}
