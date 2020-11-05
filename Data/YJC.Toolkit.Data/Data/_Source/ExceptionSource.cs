using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ExceptionSource : ISource
    {
        private ExceptionData fData;

        protected ExceptionSource()
        {
        }

        public ExceptionSource(string source, string errorPage, string url, Exception ex)
        {
            fData = new ExceptionData(source, errorPage, url, ex);
        }

        #region ISource 成员

        public virtual OutputData DoAction(IInputData input)
        {
            return OutputData.CreateToolkitObject(fData);
        }

        #endregion

        public ExceptionData Data
        {
            get
            {
                return fData;
            }
            protected set
            {
                fData = value;
            }
        }

        public string FileName { get; set; }
    }
}
