using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-04-15",
        Author = "YJC", Description = "在指定日期之间的校验")]
    [ObjectContext]
    internal sealed class DateRangeConfig : IConfigCreator<BaseConstraint>
    {
        [SimpleAttribute(DefaultValue = "1800/01/01")]
        public DateTime LowValue { get; private set; }

        [SimpleAttribute(DefaultValue = "3000/01/01")]
        public DateTime HighValue { get; private set; }

        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            return new DateRangeConstraint(ConstraintUtil.GetFieldInfo(args), LowValue, HighValue);
        }

        #endregion
    }
}
