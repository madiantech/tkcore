using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class DoubleRangeConstraint : RangeConstraint<double>
    {
        public DoubleRangeConstraint(IFieldInfo field, double lowValue, double highValue)
            : base(field, lowValue, highValue, double.MinValue, double.MaxValue)
        {
        }

        protected override double ParseValue(string value)
        {
            return double.Parse(value, ObjectUtil.SysCulture);
        }

        protected override string ToString(double value)
        {
            return value.ToString(ObjectUtil.SysCulture);
        }
    }
}
