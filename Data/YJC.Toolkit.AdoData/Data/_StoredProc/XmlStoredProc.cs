using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    //[XmlBaseClass(XmlStoredProc.BASE_CLASS)]
    internal sealed class XmlStoredProc : StoredProc
    {
        internal const string BASE_CLASS = "";

        public XmlStoredProc(StoredProcConfigItem item)
            : base(item.Name)
        {
            TkDebug.AssertArgumentNull(item, "item", this);
            ConfigItem = item;
            if (ConfigItem.Params != null)
                foreach (ParamConfigItem param in ConfigItem.Params)
                    AddParameter(param.Name, param.Kind, param.Type, param.Size);
        }

        public StoredProcConfigItem ConfigItem { get; private set; }

        protected override void PrepareParameters()
        {
            base.PrepareParameters();
            if (ConfigItem.Params == null)
                return;

            int i = 0;
            List<StoredProcParameter> parameters = Parameters;
            foreach (ParamConfigItem param in ConfigItem.Params)
            {
                if (param.DefaultValue != null)
                {
                    StoredProcParameter parameter = parameters[i];
                    parameter.SetDefaultValue(Expression.Execute(param.DefaultValue, Context));
                }
                ++i;
            }
        }
    }
}
