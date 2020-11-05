using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class DbParameterExtension
    {
        public static IDbDataParameter[] CreateParameters(this DbParameterList paramList, TkDbContext context)
        {
            if (paramList == null || paramList.Count == 0)
                return new IDbDataParameter[0];

            TkDebug.AssertArgumentNull(context, "context", paramList);

            IDbDataParameter[] result = new IDbDataParameter[paramList.Count];
            int i = 0;
            foreach (var item in paramList)
            {
                IDbDataParameter param = context.CreateParameter(item.DataType);
                param.ParameterName = context.GetParamName(item.FieldName);
                param.SourceColumn = item.FieldName;
                object fieldValue;
                if (item.FieldValue != null)
                    fieldValue = item.FieldValue.Value(MetaDataUtil.ConvertDataTypeToType(item.DataType));
                else
                    fieldValue = null;
                param.Value = fieldValue;
                result[i++] = param;
            }
            return result;
        }
    }
}