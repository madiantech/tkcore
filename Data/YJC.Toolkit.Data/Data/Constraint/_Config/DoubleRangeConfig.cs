using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "在指定浮点之间的校验")]
    [ObjectContext]
    internal sealed class DoubleRangeConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute(DefaultValue = double.MinValue)]
        public double LowValue { get; private set; }

        [SimpleAttribute(DefaultValue = double.MaxValue)]
        public double HighValue { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new DoubleRangeConstraint(ConstraintUtil.GetFieldInfo(args), LowValue, HighValue);
        }

        #endregion
    }
}
