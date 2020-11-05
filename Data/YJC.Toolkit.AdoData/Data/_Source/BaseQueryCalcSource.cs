using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseQueryCalcSource : BaseDbSource
    {
        private BaseListSearch fDefaultSearch;
        private readonly HashSet<string> fIgnoreFields;
        private readonly Dictionary<string, BaseListSearch> fSearchMethod;

        protected BaseQueryCalcSource()
        {
            fDefaultSearch = new DefaultListSearch();
            fIgnoreFields = new HashSet<string>();
            fSearchMethod = new Dictionary<string, BaseListSearch>();
        }

        public BaseListSearch DefaultSearch
        {
            get
            {
                return fDefaultSearch;
            }
            set
            {
                if (value != null)
                    fDefaultSearch = value;
            }
        }

        protected abstract void CalcResult(QueryConditionObject obj);

        protected void AddIgnoreFields(params string[] fields)
        {
            if (fields == null)
                return;

            foreach (string field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                    fIgnoreFields.Add(field);
            }
        }

        protected bool AddSearchMethod(string fieldName, BaseListSearch search)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", this);
            TkDebug.AssertArgumentNull(search, "search", this);

            if (!fSearchMethod.ContainsKey(fieldName))
            {
                fSearchMethod.Add(fieldName, search);
                return true;
            }
            return false;
        }

        public override OutputData DoAction(IInputData input)
        {
            TkDebug.Assert(input.IsPost, "此Source支持Post状态，当前是Get", this);
            QueryConditionObject obj = input.PostObject.Convert<QueryConditionObject>();
            CalcResult(obj);

            return OutputData.Create(DataSet);
        }

        public IParamBuilder GetCondition(QueryConditionObject condition)
        {
            TkDebug.AssertArgumentNull(condition, "condition", this);

            ParamBuilderContainer container = new ParamBuilderContainer();
            foreach (var item in condition.Condition)
            {
                if (string.IsNullOrEmpty(item.Value))
                    continue;
                if (fIgnoreFields.Contains(item.Key))
                    continue;

                FieldItem field = new FieldItem(item.Key);
                BaseListSearch search = ObjectUtil.TryGetValue(fSearchMethod, item.Key);
                if (search == null)
                    search = fDefaultSearch;
                search.Context = Context;
                search.ConditionData = condition.Condition;
                search.IsEqual = false;
                search.FieldName = field;

                var builder = search.GetCondition(field, item.Value);
                container.Add(builder);
            }
            return container;
        }
    }
}
