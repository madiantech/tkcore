using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ControlHtmlCreatorAttribute : BasePlugInAttribute
    {
        public ControlHtmlCreatorAttribute(string modelName)
        {
            TkDebug.AssertArgumentNullOrEmpty(modelName, nameof(modelName), null);

            Suffix = "ControlHtmlCreator";
            ModelName = modelName;
        }

        public string SearchControl { get; set; }

        public string RangeControl { get; set; }

        public override string FactoryName
        {
            get
            {
                return ControlHtmlCreatorPlugInFactory.REG_NAME;
            }
        }

        public string ModelName { get; }
    }
}