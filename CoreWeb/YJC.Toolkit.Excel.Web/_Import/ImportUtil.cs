using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal static class ImportUtil
    {
        public static ImportResultData GetResultData(IInputData input)
        {
            string key = input.QueryString[ExcelUtil.KEY_NAME];
            //ImportResultData result = WebGlobalVariable.Session[ImportResultData.SESSION_KEY]
            //    as ImportResultData;
            ImportResultData result = null;
            if (result == null || result.Key != key)
                throw new ErrorPageException("非法操作", "系统检测出你尝试非法侵入上传系统，请按照正常步骤操作！");

            return result;
        }
    }
}