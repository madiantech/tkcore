using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ControlHtmlPlugInFactory : BaseInstancePlugInFactory
    {
        private Dictionary<String, Tuple<string, string>> fDictionary;
        public const string REG_NAME = "_tk_ControlHtml";
        private const string DESCRIPTION = "生成控件HTML的插件工厂";

        public ControlHtmlPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
            fDictionary = new Dictionary<string, Tuple<string, string>>();
        }

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            bool result = base.Add(regName, attribute, type);
            if (result)
            {
                ControlHtmlAttribute attr = attribute.Convert<ControlHtmlAttribute>();
                fDictionary.Add(regName, Tuple.Create(attr.SearchControl, attr.RangeControl));
            }
            return result;
        }

        public string GetSearchControl(string regName, object sender)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", sender);

            var item = ObjectUtil.TryGetValue(fDictionary, regName);
            TkDebug.AssertNotNull(item, string.Format(ObjectUtil.SysCulture, "{0}尚未注册", regName),
                    sender);

            String searchCtrl = item.Item1;
            if (searchCtrl == "~")
                TkDebug.ThrowToolkitException(
                        string.Format(ObjectUtil.SysCulture, "控件类型{0}不支持查询", regName), sender);
            if (string.IsNullOrEmpty(searchCtrl))
                searchCtrl = regName;

            return searchCtrl;
        }

        public string GetRangeControl(string regName, Object sender)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", sender);

            var item = ObjectUtil.TryGetValue(fDictionary, regName);
            TkDebug.AssertNotNull(item, string.Format(ObjectUtil.SysCulture, "{0}尚未注册", regName),
                sender);

            String rangeCtrl = item.Item2;
            if (string.IsNullOrEmpty(rangeCtrl))
                TkDebug.ThrowToolkitException(
                    string.Format(ObjectUtil.SysCulture, "控件类型{0}不支持区间查询", regName), sender);

            return rangeCtrl;
        }
    }
}