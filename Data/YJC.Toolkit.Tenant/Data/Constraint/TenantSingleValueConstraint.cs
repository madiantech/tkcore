using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class TenantSingleValueConstraint : BaseConstraint
    {
        private string fMessage;
        private readonly IFieldInfo fTenantField;

        public TenantSingleValueConstraint(IFieldInfo field, IFieldInfo tenantField)
            : base(field)
        {
            TkDebug.AssertArgumentNull(tenantField, "tenantField", null);

            fMessage = field.DisplayName + "不唯一";
            fTenantField = tenantField;
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

                IParamBuilder builder = ParamBuilder.CreateParamBuilder(
                    SqlParamBuilder.CreateEqualSql(context, Field, value),
                    TenantUtil.GetTenantParamBuilder(context, fTenantField));
                int count = DbUtil.ExecuteScalar("SELECT COUNT(*) FROM " + TableName,
                    context, builder).Value<int>();
                if (count > 0)
                    return CreateErrorObject(Message);
                return null;
            }
        }
    }
}