using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class StartLikeListSearch : BaseListSearch
    {
        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            if (IsEqual)
                return SqlParamBuilder.CreateEqualSql(Context, field, fieldValue);
            else
                return SqlParamBuilder.CreateSingleSql(Context, field, "LIKE", fieldValue + "%");
        }
    }
}
