using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    class TreeSelector : TableSelector
    {
        public TreeSelector(ITableScheme scheme, DbTreeDefinition treeDef, IDbDataSource source)
            : base(new TreeScheme(scheme, treeDef), source)
        {
            SetFakeDelete(this, scheme);
        }

        public static void SetFakeDelete(TableSelector selector, ITableScheme scheme)
        {
            Tk5DataXml dataXml = scheme as Tk5DataXml;
            if (dataXml != null)
                selector.FakeDelete = dataXml.FakeDeleteInfo;
        }
    }
}
