using System;

namespace YJC.Toolkit.Sys
{
    [Serializable]
    public class ErrorPageException : Exception, ISource, IExceptionInfo
    {
        //protected ErrorPageException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    ErrorTitle = info.GetString("ErrorTitle");
        //    ErrorBody = info.GetString("ErrorBody");
        //    PageTitle = info.GetString("PageTitle");
        //    RetUrl = info.GetValue("RetUrl", typeof(Uri)) as Uri;
        //}

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

        #region ISource 成员

        OutputData ISource.DoAction(IInputData input)
        {
            //return DoAction(input);
            return null;
        }

        #endregion

        #region IExceptionInfo 成员

        public void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);
        }

        #endregion

        public string ErrorTitle { get; set; }

        public string ErrorBody { get; set; }

        public string PageTitle { get; set; }

        public Uri RetUrl { get; set; }

        //[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    base.GetObjectData(info, context);

        //    info.AddValue("ErrorTitle", ErrorTitle);
        //    info.AddValue("ErrorBody", ErrorBody);
        //    info.AddValue("PageTitle", PageTitle);
        //    info.AddValue("RetUrl", RetUrl);
        //}

        //protected internal WebOutputData DoAction(WebInputData input)
        //{
        //    DataSet data = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };

        //    DataTable table = DataSetUtil.CreateDataTable("Error", "ErrorTitle", "ErrorBody", "PageTitle");
        //    table.Rows.Add(ErrorTitle, ErrorBody, PageTitle);
        //    data.Tables.Add(table);
        //    input.UrlInfo.AddToDataSet(this, data);
        //    return new WebOutputData(SourceOutputType.XmlReader, new XmlDataSetReader(data, true));
        //}
    }
}
