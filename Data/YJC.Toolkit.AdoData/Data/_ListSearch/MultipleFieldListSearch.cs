using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleFieldListSearch : BaseListSearch
    {
        private readonly IFieldInfo[] fFields;

        public MultipleFieldListSearch(params IFieldInfo[] fields)
        {
            TkDebug.AssertArgumentNull(fields, "fields", null);
            TkDebug.AssertEnumerableArgumentNull(fields, "fields", null);

            fFields = fields;
        }

        public bool WordSplit { get; set; }

        private IParamBuilder CreateItemParamBuilder(string value, bool isEqual, int index = 0)
        {
            IParamBuilder[] builders = new IParamBuilder[fFields.Length];

            for (int i = 0; i < fFields.Length; ++i)
            {
                if (isEqual)
                    builders[i] = SqlParamBuilder.CreateEqualSql(Context, fFields[i], value);
                else
                {
                    if (index == 0)
                        builders[i] = LikeParamBuilder.CreateLikeSql(Context, fFields[i], value);
                    else
                        builders[i] = LikeParamBuilder.CreateLikeSql(Context, fFields[i],
                            fFields[i].FieldName + index, value);
                }
            }
            return SqlParamBuilder.CreateParamBuilderWithOr(builders);
        }

        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            if (IsEqual)
                return CreateItemParamBuilder(fieldValue, true);
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
                            IParamBuilder builder = CreateItemParamBuilder(newValue, false, index);
                            items.Add(builder);
                        }
                    }
                    return ParamBuilder.CreateParamBuilder(items);
                }
                else
                    return CreateItemParamBuilder(fieldValue, false);
            }
        }
    }
}
