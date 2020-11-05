using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ControlHtmlCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_ControlHtmlCreator";
        private const string DESCRIPTION = "ControlHtmlCreator插件工厂";

        public ControlHtmlCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            ControlHtmlCreatorAttribute ctrlAttr = attribute.Convert<ControlHtmlCreatorAttribute>();
            string newRegName = $"{ctrlAttr.ModelName}-{regName}";
            return base.Add(newRegName, attribute, type);
        }

        public IControlHtmlCreator GetHtmlCreator(string modelName, string regName)
        {
            string newRegName = $"{modelName}-{regName}";
            if (Contains(newRegName))
                return CreateInstance<IControlHtmlCreator>(newRegName);
            return null;
        }
    }
}