using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ExceptionData : IReadObjectCallBack
    {
        private readonly List<ExceptionInfo> fList;

        internal ExceptionData()
        {
            fList = new List<ExceptionInfo>();
        }

        public ExceptionData(string source, string errorPage, string url, Exception ex)
            : this()
        {
            TkDebug.AssertArgumentNull(ex, "ex", null);

            Exception = ex;
            Message = ex.Message;
            Exception temp = ex;
            do
            {
                FullExceptionInfo info = new FullExceptionInfo();
                IExceptionInfo intf = temp as IExceptionInfo;
                if (intf != null)
                    intf.FillExceptionInfo(info);
                else
                    info.FillInfo(temp);
                fList.Add(info);
                temp = temp.InnerException;
            } while (temp != null);

            Error = new ErrorInfo(source, errorPage, url);
        }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Infos.Count > 0)
                Message = Infos[0].Message;
        }

        #endregion IReadObjectCallBack 成员

        [ObjectElement(IsMultiple = true, LocalName = "Exception", UseConstructor = true, Order = 20)]
        public List<ExceptionInfo> Infos
        {
            get
            {
                return fList;
            }
        }

        [ObjectElement(Order = 10, UseConstructor = true)]
        public ErrorInfo Error { get; private set; }

        public Exception Exception { get; private set; }

        public string Message { get; private set; }
    }
}