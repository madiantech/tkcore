namespace YJC.Toolkit.Decoder
{
    internal class InternalTk5DbCodeTableEasySearch : BaseSchemeEasySearch
    {
        public InternalTk5DbCodeTableEasySearch(BaseCodeTableEasySearchConfig config)
            : base(new StandardCodeTableScheme(config.TableName))
        {
            ContextName = config.Context;
            if (string.IsNullOrEmpty(config.OrderBy))
                OrderBy = StandardDbCodeTable.DEFAULT_ORDER;
            else
                OrderBy = config.OrderBy;
            if (!string.IsNullOrEmpty(config.NameExpression))
                NameExpression = config.NameExpression;
            if (!string.IsNullOrEmpty(config.DisplayNameExpression))
                DisplayNameExpression = config.DisplayNameExpression;

            PinyinField = DecoderConst.PY_FIELD;
            ActiveData = CodeTableActiveData.Instance;
            SearchMethod = CodeTableSearch.Instance;
        }
    }
}
