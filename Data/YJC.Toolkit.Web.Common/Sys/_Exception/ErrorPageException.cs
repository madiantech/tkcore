using System;

namespace YJC.Toolkit.Sys
{
    public class ErrorPageException : Exception, IExceptionInfo
    {
        /// <summary>
        /// Initializes a new instance of the ErrorPageException class.
        /// </summary>
        public ErrorPageException(string pageTitle, string errorTitle, string errorBody, Uri retUrl)
            : this(pageTitle, errorTitle, errorBody)
        {
            RetUrl = retUrl;
        }

        public ErrorPageException(string errorTitle, string errorBody, Uri retUrl)
            : this(errorTitle, errorTitle, errorBody, retUrl)
        {
        }

        public ErrorPageException(string pageTitle, string errorTitle, string errorBody)
            : base(errorTitle)
        {
            ErrorTitle = errorTitle;
            ErrorBody = errorBody;
            PageTitle = pageTitle;
        }

        public ErrorPageException(string title, string body)
            : this(title, title, body)
        {
        }

        #region IExceptionInfo 成员

        public void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);
        }

        #endregion IExceptionInfo 成员

        public string ErrorTitle { get; set; }

        public string ErrorBody { get; set; }

        public string PageTitle { get; set; }

        public Uri RetUrl { get; set; }
    }
}