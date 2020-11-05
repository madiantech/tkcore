using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public class MySqlRegexListSearch : BaseListSearch
    {
        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            return ParamBuilder.CreateSql($"{field.FieldName} REGEXP '{fieldValue}'");
        }
    }
}