using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class Tk5DbEasySearch : BaseSchemeEasySearch
    {
        public Tk5DbEasySearch(Tk5DataXml scheme)
            : base(scheme)
        {
            SetActiveData(scheme);
        }

        internal Tk5DbEasySearch(BaseEasySearchConfig config)
            : base(MetaDataUtil.ConvertToTableScheme(config.CreateScheme()), config.IdField, config.NameField)
        {
            ContextName = config.Context;
            OrderBy = config.OrderBy;
            if (config.TopCount > 0)
                TopCount = config.TopCount;
            if (!string.IsNullOrEmpty(config.PyField))
                PinyinField = this[config.PyField];
            if (!string.IsNullOrEmpty(config.NameExpression))
                NameExpression = config.NameExpression;
            if (!string.IsNullOrEmpty(config.DisplayNameExpression))
                DisplayNameExpression = config.DisplayNameExpression;

            FilterSql = config.FilterSql;
            if (config.DataRight != null)
                DataRight = config.DataRight.CreateObject(this);

            SetActiveData(SourceScheme.Convert<Tk5DataXml>());
            SearchMethod = SearchPlugInFactory.CreateSearch(config.SearchMethod, false);
        }

        private void SetActiveData(Tk5DataXml scheme)
        {
            ActiveData = scheme.FakeDeleteInfo;
        }
    }
}