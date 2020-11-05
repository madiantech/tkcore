using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class ImportErrorSource : ISource
    {
        private readonly dynamic fBag = new ExpandoObject();

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            ImportResultData result = ImportUtil.GetResultData(input);

            fBag.Count = result.ImportTable.Rows.Count;
            string previewUrl = string.Format(ObjectUtil.SysCulture,
                "~/c/import/CPreviewSuccess/{0}?Key={1}", input.SourceInfo.Source,
                input.QueryString[ExcelUtil.KEY_NAME]);
            fBag.PreviewUrl = WebUtil.ResolveUrl(previewUrl);
            fBag.ErrorInfo = result;

            return OutputData.CreateObject(fBag);
        }

        #endregion ISource 成员
    }
}