using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class DefaultDateSpanSearch
    {
        private class SmallSearch : BaseListSearch
        {
            public override IParamBuilder GetCondition(IFieldInfo fieldName, string fieldValue)
            {
                return SqlParamBuilder.CreateSingleSql(Context, fieldName, ">=", fieldName.FieldName, fieldValue);
            }
        }

        private class LargeSearch : BaseListSearch
        {
            public override IParamBuilder GetCondition(IFieldInfo fieldName, string fieldValue)
            {
                try
                {
                    DateTime date = DateTime.Parse(fieldValue, ObjectUtil.SysCulture);
                    return SqlParamBuilder.CreateSingleSql(Context, fieldName, "<", fieldName.FieldName + "END", date.AddDays(1));
                }
                catch
                {
                    return SqlParamBuilder.NoResult;
                }

            }
        }

        private DefaultDateSpanSearch()
        {
        }

        public static void Add(ListSearchCollection searches, IFieldInfo field)
        {
            searches.AddSpan(field, new SmallSearch(), new LargeSearch());
        }
    }
}
