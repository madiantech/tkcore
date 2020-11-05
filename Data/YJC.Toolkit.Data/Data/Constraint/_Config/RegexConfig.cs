using System.Text.RegularExpressions;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Description = "按照指定的正则表达式进行校验", Author = "YJC")]
    [ObjectContext]
    internal sealed class RegexConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute(Required = true)]
        public string Message { get; private set; }

        [SimpleAttribute(Required = true)]
        public string Pattern { get; private set; }

        [SimpleAttribute(DefaultValue = RegexOptions.None)]
        public RegexOptions Options { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new RegexConstraint(ConstraintUtil.GetFieldInfo(args), Message, Pattern, Options);
        }

        #endregion
    }
}
