using System.Collections.Generic;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class DefaultListSearch : BaseListSearch
    {
        public bool WordSplit { get; set; }

        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            if (IsEqual)
                return SqlParamBuilder.CreateEqualSql(Context, field, fieldValue);
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
                            IParamBuilder builder = LikeParamBuilder.CreateLikeSql(Context,
                                field, field.FieldName + index, newValue);
                            items.Add(builder);
                        }
                    }
                    return ParamBuilder.CreateParamBuilder(items);
                }
                else
                    return LikeParamBuilder.CreateLikeSql(Context, field, fieldValue);
            }
        }
    }
}
