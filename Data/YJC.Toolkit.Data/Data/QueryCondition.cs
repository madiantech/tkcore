using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class QueryCondition
    {
        internal QueryCondition()
        {
        }

        public QueryCondition(Dictionary<string, string> queryData, IParamBuilder builder)
        {
            TkDebug.AssertArgumentNull(queryData, "queryData", null);
            TkDebug.AssertArgumentNull(builder, "builder", null);

            QueryData = queryData;
            Builder = new XmlParamBuilder(builder);
        }

        [Dictionary]
        public Dictionary<string, string> QueryData { get; private set; }

        [ObjectElement(ObjectType = typeof(XmlParamBuilder), UseConstructor = true)]
        public IParamBuilder Builder { get; set; }

        public string ToEncodeString()
        {
            string json = this.WriteJson();
            return Convert.ToBase64String(ToolkitConst.UTF8.GetBytes(json));
        }

        public static QueryCondition FromEncodeString(string data)
        {
            string json = ToolkitConst.UTF8.GetString(Convert.FromBase64String(data));
            QueryCondition condition = new QueryCondition();
            condition.ReadJson(json);

            return condition;
        }
    }
}
