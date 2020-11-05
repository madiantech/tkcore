using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class TableConfigItem : IReadObjectCallBack, IActiveData
    {
        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            DataList = (from item in FieldList
                        where item.Kind == FieldKind.Data
                        select item).ToArray();
        }

        #endregion

        #region IActiveData 成员

        public IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            if (FakeDeleteInfo == null)
                return null;

            return FakeDeleteInfo.CreateParamBuilder(context, indexer);
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string NameField { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText TableDesc { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public FakeDeleteInfo FakeDeleteInfo { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Field", Required = true)]
        public RegNameList<FieldConfigItem> FieldList { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DbTreeDefinition Tree { get; private set; }

        public FieldConfigItem[] DataList { get; private set; }
    }
}