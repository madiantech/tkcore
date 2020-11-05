using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ParamExpressionAttribute : BasePlugInAttribute
    {
        public ParamExpressionAttribute(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.Assert(regName.Length == 1,
                string.Format(ObjectUtil.SysCulture,
                "ParamExpression的注册名只能是一个字符，现在的注册名{0}不是", regName), null);
            TkDebug.Assert(!char.IsLetterOrDigit(regName[0]),
                string.Format(ObjectUtil.SysCulture,
                "ParamExpression的注册名要求不是字母或数字，现在的注册名{0}不是", regName), null);

            RegName = regName;
        }

        public bool SqlInject { get; set; }

        public override string FactoryName
        {
            get
            {
                return ParamExpressionPlugInFactory.REG_NAME;
            }
        }
    }
}
