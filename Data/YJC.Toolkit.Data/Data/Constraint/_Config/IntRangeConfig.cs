using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "在指定整数范围的校验")]
    [ObjectContext]
    internal sealed class IntRangeConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute(DefaultValue = int.MinValue)]
        public int LowValue { get; private set; }

        [SimpleAttribute(DefaultValue = int.MaxValue)]
        public int HighValue { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new IntRangeConstraint(ConstraintUtil.GetFieldInfo(args), LowValue, HighValue);
        }

        #endregion
    }
}
