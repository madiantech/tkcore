using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public abstract class BaseNormalTemplateSuite : ITemplateSuite
    {
        protected BaseNormalTemplateSuite(string basePath)
        {
            TkDebug.AssertArgumentNullOrEmpty(basePath, "basePath", null);

            BasePath = basePath;
        }

        #region ITemplateSuite 成员

        public virtual RazorSuiteItem GetStyleTemplate(RazorTemplateStyle style, bool isNormal)
        {
            string fileName = GetTemplateFile(style, isNormal);
            fileName = Path.Combine(BasePath, fileName);
            Tuple<Type, Type> pageData = GetDefaultPageDataType(style, isNormal);
            RazorSuiteItem result = new RazorSuiteItem(pageData.Item1, pageData.Item2, fileName);
            return result;
        }

        #endregion

        public string BasePath { get; private set; }

        protected abstract string GetTemplateFile(RazorTemplateStyle style, bool isNormal);

        protected virtual Tuple<Type, Type> GetDefaultPageDataType(RazorTemplateStyle style, bool isNormal)
        {
            return RazorConst.GetPageDataType(style, isNormal);
        }
    }
}
