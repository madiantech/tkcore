using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public abstract class RangeConstraint<T> : BaseConstraint where T : IComparable<T>, IEquatable<T>
    {
        private string fMessage;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected RangeConstraint(IFieldInfo field,
            T lowValue, T highValue, T minValue, T maxValue)
            : base(field)
        {
            HighValue = highValue;
            LowValue = lowValue;
            MaxValue = maxValue;
            MinValue = minValue;
            TkDebug.Assert(highValue.CompareTo(lowValue) >= 0, string.Format(ObjectUtil.SysCulture,
               "highValue参数的值应该比lowValue参数的值大，现在highValue的值为{0}，"
               + "lowValue的值为{1}", ToString(lowValue), ToString(highValue)), null);
            TkDebug.Assert(maxValue.CompareTo(minValue) >= 0, string.Format(ObjectUtil.SysCulture,
               "maxValue参数的值应该比minValue参数的值大，现在maxValue的值为{0}，"
               + "minValue的值为{1}", ToString(maxValue), ToString(minValue)), null);
            SetErrorMsg();
        }

        private void SetErrorMsg()
        {
            if (LowValue.Equals(MinValue) && HighValue.Equals(MaxValue))
                fMessage = string.Format(ObjectUtil.SysCulture, TkWebApp.RangeCBetween,
                    Field.DisplayName, ToString(LowValue), ToString(HighValue));
            else if (LowValue.Equals(MinValue))
                fMessage = string.Format(ObjectUtil.SysCulture, TkWebApp.RangeCLow,
                    Field.DisplayName, ToString(HighValue));
            else if (HighValue.Equals(MaxValue))
                fMessage = string.Format(ObjectUtil.SysCulture, TkWebApp.RangeCGreat,
                    Field.DisplayName, ToString(LowValue));
            else
                fMessage = string.Format(ObjectUtil.SysCulture, TkWebApp.RangeCBetween,
                    Field.DisplayName, ToString(LowValue), ToString(HighValue));
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            try
            {
                T currentValue = ParseValue(value);
                if (!(currentValue.CompareTo(LowValue) >= 0 && currentValue.CompareTo(HighValue) <= 0))
                    return CreateErrorObject(fMessage);
            }
            catch
            {
            }
            return null;
        }

        public T HighValue { get; private set; }

        public T LowValue { get; private set; }

        protected T MinValue { get; private set; }

        protected T MaxValue { get; private set; }

        protected abstract T ParseValue(string value);

        protected abstract string ToString(T value);

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "[{0}]的值必须在{1}和{2}之间的约束", Field.DisplayName,
                ToString(LowValue), ToString(HighValue));
        }
    }
}
