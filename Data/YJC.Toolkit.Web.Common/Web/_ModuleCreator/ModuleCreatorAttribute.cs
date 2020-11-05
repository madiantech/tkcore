using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ModuleCreatorAttribute : BasePlugInAttribute
    {
        private string fRegName;

        public ModuleCreatorAttribute()
        {
            Suffix = "ModuleCreator";
        }

        public override string RegName
        {
            get
            {
                return fRegName;
            }
            set
            {
                if (fRegName != value)
                {
                    fRegName = value;
                    if (!string.IsNullOrEmpty(fRegName))
                        fRegName = fRegName.ToLower(ObjectUtil.SysCulture);
                }
            }
        }

        public override string FactoryName
        {
            get
            {
                return ModuleCreatorPlugInFactory.REG_NAME;
            }
        }

        protected override string CalRegName(string typeName)
        {
            string name = base.CalRegName(typeName);
            return name.ToLower(ObjectUtil.SysCulture);
        }
    }
}
