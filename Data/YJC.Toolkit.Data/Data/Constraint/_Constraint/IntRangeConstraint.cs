using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class IntRangeConstraint : RangeConstraint<int>
    {
        public IntRangeConstraint(IFieldInfo field, int lowValue, int highValue)
            : base(field, lowValue, highValue, int.MinValue, int.MaxValue)
        {
        }

        protected override int ParseValue(string value)
        {
            return int.Parse(value, ObjectUtil.SysCulture);
        }

        protected override string ToString(int value)
        {
            return value.ToString(ObjectUtil.SysCulture);
        }
    }
}
