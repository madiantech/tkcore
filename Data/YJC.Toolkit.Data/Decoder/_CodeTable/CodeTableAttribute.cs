using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CodeTableAttribute : BasePlugInAttribute
    {
        public CodeTableAttribute()
        {
            Suffix = "CodeTable";
        }

        public CodeTableAttribute(IAuthor config)
            : this()
        {
            TkDebug.AssertArgumentNull(config, "config", this);

            RegName = config.Convert<IRegName>().RegName;
            Author = config.Author;
            Description = config.Description;
            CreateDate = config.CreateDate;
        }

        internal void SetDefaultValue(string regName)
        {
            RegName = regName;
            Description = string.Format(ObjectUtil.SysCulture, "标准代码表（{0}）", regName);
            Author = ToolkitConst.TOOLKIT;
            CreateDate = DateTime.Today.ToString(ToolkitConst.DATE_FMT_STR, ObjectUtil.SysCulture);
        }

        public override string FactoryName
        {
            get
            {
                return CodeTablePlugInFactory.REG_NAME;
            }
        }
    }
}