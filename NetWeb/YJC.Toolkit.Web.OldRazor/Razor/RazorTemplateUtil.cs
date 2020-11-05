using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public static class RazorTemplateUtil
    {
        public static Type GetRegTemplateType(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.ThrowIfNoGlobalVariable();
            RazorTemplateTypeFactory factory = BaseGlobalVariable.Current.FactoryManager.
                GetCodeFactory(RazorTemplateTypeFactory.REG_NAME).Convert<RazorTemplateTypeFactory>();
            return factory.GetType(regName);
        }

        private static RazorTemplateAttribute GetAttribute(string regName, out Type type)
        {
            type = GetRegTemplateType(regName);
            TkDebug.AssertNotNull(type, string.Format(ObjectUtil.SysCulture,
                "没有找到注册名为{0}的数据类型", regName), null);

            RazorTemplateAttribute attr = Attribute.GetCustomAttribute(type,
                typeof(RazorTemplateAttribute)).Convert<RazorTemplateAttribute>();
            return attr;
        }

        private static string Execute(object model, DynamicObjectBag viewBag,
            Type type, XmlTemplateInitData initData)
        {
            object pageData = ((dynamic)viewBag).PageData;
            if (pageData == null)
            {
                pageData = initData.BagData;
                if (pageData != null)
                    viewBag.AddValue("PageData", pageData);
            }

            string content = RazorUtil.ParseFromFile(type, initData.FileName, initData.LayoutFile,
                null, model, viewBag, initData);

            return content;
        }

        public static string Execute(string regName, string templateFile, string localRazorFile,
            object model, DynamicObjectBag viewBag)
        {
            Type type;
            RazorTemplateAttribute attr = GetAttribute(regName, out type);
            XmlTemplateInitData initData = new XmlTemplateInitData(attr, templateFile, localRazorFile);

            return Execute(model, viewBag, type, initData);
        }

        public static string Execute(string regName, string localRazorFile,
            object model, DynamicObjectBag viewBag)
        {
            Type type;
            RazorTemplateAttribute attr = GetAttribute(regName, out type);
            XmlTemplateInitData initData = new XmlTemplateInitData(attr, localRazorFile);

            return Execute(model, viewBag, type, initData);
        }

        public static string Execute(RazorSuiteItem suiteItem,
            string localRazorFile, object model, DynamicObjectBag viewBag)
        {
            RazorTemplateAttribute attr = new RazorTemplateAttribute(suiteItem.TemplateFilePath)
            {
                PageDataType = suiteItem.RazorDataType
            };
            XmlTemplateInitData initData = new XmlTemplateInitData(attr, localRazorFile);

            return Execute(model, viewBag, suiteItem.RazorTemplateType, initData);
        }
    }
}
