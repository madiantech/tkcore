using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class MultipleNotEmptyConstraint : BaseConstraint
    {
        private readonly IFieldInfo[] OtherFields;
        private readonly string fMessage;
        private readonly string fFields;

        public MultipleNotEmptyConstraint(IFieldInfo field, params IFieldInfo[] otherFields)
            : base(field)
        {
            OtherFields = otherFields;
            if (OtherFields == null)
                OtherFields = new IFieldInfo[0];
            StringBuilder builder = new StringBuilder();
            builder.Append(field.DisplayName);
            foreach (var item in OtherFields)
                builder.Append(" ").Append(item.DisplayName);
            fFields = builder.ToString();
            fMessage = fFields + TkWebApp.MultipleNotEmptyCMsg;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            DataRow row = ObjectUtil.QueryObject<DataRow>(args);
            if (row == null)
                return null;

            var otherValues = from item in OtherFields
                              let itemValue = row[item.NickName].ToString()
                              where !string.IsNullOrEmpty(itemValue)
                              select itemValue;
            if (string.IsNullOrEmpty(value) && otherValues.FirstOrDefault() == null)
                return CreateErrorObject(fMessage);

            return null;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{0}不能同时为空的约束", fFields);
        }
    }
}
