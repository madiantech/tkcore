using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ExceptionPostPageMaker : IPageMaker
    {
        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            ExceptionSource exSource = source.Convert<ExceptionSource>();
            string fileName = exSource.FileName;
            object result;
            if (string.IsNullOrEmpty(fileName))
                result = new WebErrorResult(exSource.Data.Exception.Message);
            else
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
                string url = AppUtil.ResolveUrl("~/c/plugin/C/Exception?FileName=" + fileName);
                result = new WebExceptionResult(url);
            }
            string json = result.WriteJson();

            return new SimpleContent(ContentTypeConst.JSON, json);
        }

        #endregion IPageMaker 成员
    }
}