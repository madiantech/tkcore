using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class SingleValueConstraint : BaseConstraint
    {
        private string fMessage;

        public SingleValueConstraint(IFieldInfo field)
            : base(field)
        {
            fMessage = field.DisplayName + TkWebApp.SingleValueCMsg;
        }

        public string Message
        {
            get
            {
                return fMessage;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    fMessage = value;
            }
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            string original = null;
            try
            {
                DataRow row = HostDataSet.Tables[TableName].Rows[position];
                original = row[Field.NickName, DataRowVersion.Original].ToString();
            }
            catch
            {
            }
            if (!string.IsNullOrEmpty(original) && original == value)
                return null;
            else
            {
                TkDbContext context = ObjectUtil.ConfirmQueryObject<TkDbContext>(this, args);

                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, Field, value);
                int count = DbUtil.ExecuteScalar("SELECT COUNT(*) FROM " + TableName,
                    context, builder).Value<int>();
                if (count > 0)
                    return CreateErrorObject(Message);
                return null;
            }
        }
    }
}
