using System.Configuration;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public abstract class BaseMobileConstraint : RegexConstraint
    {
        private readonly string fCorp;

        protected BaseMobileConstraint(IFieldInfo field, string corp, string config, string pattern)
            : base(field, string.Format(ObjectUtil.SysCulture,
            TkWebApp.BaseMobileCMsg, field.DisplayName, corp),
            GetPattern(config, pattern))
        {
            fCorp = corp;
        }

        private static string GetPattern(string config, string pattern)
        {
            string mobileConfig = BaseGlobalVariable.Current?.DefaultValue?.GetSimpleDefaultValue(config);
            return string.IsNullOrEmpty(mobileConfig) ? pattern : mobileConfig;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]的值必须是{1}号码的约束", Field.DisplayName, fCorp);
        }
    }
}