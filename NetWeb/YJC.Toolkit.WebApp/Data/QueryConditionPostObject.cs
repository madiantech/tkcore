using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class QueryConditionPostObject
    {
        [ObjectElement]
        public QueryConditionObject Query { get; private set; }

        [SimpleElement]
        public string Resolver { get; private set; }
    }
}
