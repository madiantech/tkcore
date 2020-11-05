namespace YJC.Toolkit.Sys
{
    public sealed class QueryStringOutput
    {
        public static readonly QueryStringOutput Default = new QueryStringOutput(false, false);

        public static readonly QueryStringOutput WeixinOutput = new QueryStringOutput(true, true);

        public QueryStringOutput(bool sortKey, bool ignoreEmpty)
        {
            SortKey = sortKey;
            IgnoreEmpty = ignoreEmpty;
        }

        public bool SortKey { get; private set; }

        public bool IgnoreEmpty { get; private set; }
    }
}
