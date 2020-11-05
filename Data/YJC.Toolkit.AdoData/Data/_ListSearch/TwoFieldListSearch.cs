using System.Collections.Generic;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public class TwoFieldListSearch : BaseListSearch
    {
        private readonly IFieldInfo fField1;
        private readonly IFieldInfo fField2;

        public TwoFieldListSearch(IFieldInfo field1, IFieldInfo field2)
        {
            fField1 = field1;
            fField2 = field2;
        }

        public bool WordSplit { get; set; }

        private IParamBuilder GetLikeParamBuilder(string value, int index = 0)
        {
            return SqlParamBuilder.CreateParamBuilderWithOr(
                LikeParamBuilder.CreateLikeSql(Context, fField1, fField1.FieldName + index, value),
                LikeParamBuilder.CreateLikeSql(Context, fField2, fField2.FieldName + index, value));
        }

        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            if (IsEqual)
            {
                return SqlParamBuilder.CreateParamBuilderWithOr(
                    SqlParamBuilder.CreateEqualSql(Context, fField1, fieldValue),
                    SqlParamBuilder.CreateEqualSql(Context, fField2, fieldValue));
            }
            else
            {
                if (WordSplit)
                {
                    string[] values = fieldValue.Split(' ');
                    List<IParamBuilder> items = new List<IParamBuilder>(values.Length);
                    int index = 0;
                    foreach (var value in values)
                    {
                        string newValue = value.Trim();
                        if (!string.IsNullOrEmpty(newValue))
                        {
                            ++index;
                            IParamBuilder builder = GetLikeParamBuilder(newValue, index);
                            items.Add(builder);
                        }
                    }
                    return ParamBuilder.CreateParamBuilder(items);
                }
                else
                    return GetLikeParamBuilder(fieldValue);
            }
        }
    }
}
