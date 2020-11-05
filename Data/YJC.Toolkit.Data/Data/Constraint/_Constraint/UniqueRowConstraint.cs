using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class UniqueRowConstraint : BaseConstraint
    {
        public UniqueRowConstraint(IFieldInfo field)
            : base(field)
        {
            IsFirstCheck = true;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            DataSet postDataSet = inputData.PostObject as DataSet;
            if (postDataSet == null)
                return null;

            DataRowCollection rows = postDataSet.Tables[TableName].Rows;
            for (int i = 0; i < position; ++i)
            {
                if (rows[i][Field.NickName].ToString().Trim() == value.Trim())
                    return CreateErrorObject(string.Format(ObjectUtil.SysCulture,
                        TkWebApp.UniqueRowCMsg, Field.DisplayName, i + 1));
            }
            return null;
        }
    }
}
