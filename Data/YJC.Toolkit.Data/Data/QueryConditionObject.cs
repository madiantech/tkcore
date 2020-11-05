using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [RegClass(Author = "YJC", CreateDate = "2014-01-14", Description = "存储查询条件的对象")]
    public class QueryConditionObject : IReadObjectCallBack
    {
        public QueryConditionObject()
        {
        }

        public QueryConditionObject(bool isEqual, Dictionary<string, string> condition)
        {
            IsEqual = isEqual;
            Condition = condition;
        }

        [SimpleAttribute]
        public bool IsEqual { get; private set; }

        [Dictionary]
        public Dictionary<string, string> Condition { get; private set; }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Condition == null)
                return;
            var easySearchKeys = (from item in Condition.Keys
                                  let hiddenKey = "hd" + item
                                  where Condition.ContainsKey(hiddenKey)
                                  select hiddenKey).ToArray();
            foreach (var item in easySearchKeys)
                Condition.Remove(item);
        }

        #endregion
    }
}
