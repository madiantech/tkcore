using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Excel
{
    internal class ImportPostPageMaker : IPageMaker
    {
        public static readonly IPageMaker PAGE_MAKER = new ImportPostPageMaker();

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            IPageMaker jsonPageMaker = new JsonObjectPageMaker();
            if (outputData.Data is WebErrorResult)
                return jsonPageMaker.WritePage(source, pageData, outputData);

            ImportResultData resultData = outputData.Data.Convert<ImportResultData>();
            //WebGlobalVariable.Session[ImportResultData.SESSION_KEY] = resultData;

            string url = string.Format(ObjectUtil.SysCulture, "c/import/C{1}/{0}?Key={2}",
                pageData.SourceInfo.Source,
                resultData.ErrorTable.Rows.Count > 0 ? ImportConst.ERROR_IMPORT : ImportConst.IMPORT,
                resultData.Key);
            WebSuccessResult result = new WebSuccessResult(url.AppVirutalPath());
            return jsonPageMaker.WritePage(source, pageData, OutputData.CreateToolkitObject(result));
        }

        #endregion IPageMaker 成员
    }
}