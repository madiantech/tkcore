using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class DefaultEqualSearch : BaseListSearch
    {
        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            return SqlParamBuilder.CreateEqualSql(Context, field, fieldValue);
        }
    }
}