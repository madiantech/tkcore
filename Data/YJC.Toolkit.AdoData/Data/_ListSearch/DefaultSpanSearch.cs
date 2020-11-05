using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class DefaultSpanSearch
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
                return SqlParamBuilder.CreateSingleSql(Context, fieldName, "<=", fieldName.FieldName + "END", fieldValue);
            }
        }

        private DefaultSpanSearch()
        {
        }

        public static void Add(ListSearchCollection searches, IFieldInfo field)
        {
            searches.AddSpan(field, new SmallSearch(), new LargeSearch());
        }
    }
}
