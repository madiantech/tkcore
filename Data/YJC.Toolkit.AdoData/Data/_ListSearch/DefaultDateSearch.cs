using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class DefaultDateSearch : BaseListSearch
    {
        public override IParamBuilder GetCondition(IFieldInfo fieldName, string fieldValue)
        {
            try
            {
                DateTime date = DateTime.Parse(fieldValue, ObjectUtil.SysCulture);
                IParamBuilder builder1 = SqlParamBuilder.CreateSingleSql(Context, fieldName, ">=", fieldName.FieldName, date);
                IParamBuilder builder2 = SqlParamBuilder.CreateSingleSql(Context, fieldName, "<", fieldName.FieldName + "END", date.AddDays(1));
                return SqlParamBuilder.CreateParamBuilder(builder1, builder2);
            }
            catch
            {
                return SqlParamBuilder.NoResult;
            }
        }
    }
}
