using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal abstract class BasePlugInTemplateSuite : BaseNormalTemplateSuite
    {
        protected BasePlugInTemplateSuite(string basePath)
            : base(basePath)
        {
        }

        public override RazorSuiteItem GetStyleTemplate(RazorTemplateStyle style, bool isNormal)
        {
            string fileName = GetTemplateFile(style, isNormal);
            if (string.IsNullOrEmpty(fileName))
                return DefaultRazorTemplateSuite.Default.GetStyleTemplate(style, isNormal);

            fileName = GetPath(style, isNormal, fileName);

            Tuple<Type, Type> pageData = GetDefaultPageDataType(style, isNormal);
            RazorSuiteItem result = new RazorSuiteItem(pageData.Item1, pageData.Item2, fileName);
            return result;
        }

        public bool UseNamePath { get; protected set; }

        private string GetPath(RazorTemplateStyle style, bool isNormal, string fileName)
        {
            if (UseNamePath)
            {
                if (isNormal)
                    return Path.Combine(BasePath, style.ToString(), fileName);
                else
                    return Path.Combine(BasePath, style.ToString() + "Object", fileName);
            }
            else
                return Path.Combine(BasePath, fileName);
        }
    }
}
