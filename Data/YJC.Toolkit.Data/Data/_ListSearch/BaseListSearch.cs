using System.Collections.Generic;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public abstract class BaseListSearch
    {
        protected BaseListSearch()
        {
        }

        public bool IsEqual { get; internal set; }

        public Dictionary<string, string> ConditionData { get; internal set; }

        public TkDbContext Context { get; internal set; }

        internal IFieldInfo FieldName { get; set; }

        public abstract IParamBuilder GetCondition(IFieldInfo field, string fieldValue);
    }
}
