using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class DateRangeConstraint : RangeConstraint<DateTime>
    {
        private static readonly DateTime DEFAULT_MIN_DATE = new DateTime(1800, 1, 1);
        private static readonly DateTime DEFAULT_MAX_DATE = new DateTime(3000, 1, 1);

        internal DateRangeConstraint(IFieldInfo field)
            : this(field, DEFAULT_MIN_DATE, DEFAULT_MAX_DATE)
        {
        }

        public DateRangeConstraint(IFieldInfo field, DateTime lowValue, DateTime highValue)
            : base(field, lowValue, highValue, DateTime.MinValue, DateTime.MaxValue)
        {
        }

        protected override DateTime ParseValue(string value)
        {
            return DateTime.Parse(value, ObjectUtil.SysCulture);
        }

        protected override string ToString(DateTime value)
        {
            return value.ToString(ToolkitConst.DATE_FMT_STR, ObjectUtil.SysCulture);
        }
    }
}
