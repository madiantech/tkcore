using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    [TableSchemeExConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-11-03",
        Author = "YJC", Description = "")]
    internal class UnionSchemeConfig : IConfigCreator<ITableSchemeEx>
    {
        #region IConfigCreator<ITableSchemeEx> 成员

        public ITableSchemeEx CreateObject(params object[] args)
        {
            string tableDesc = MultiLanguageText.ToString(TableDesc);

            var schemes = from item in Items
                          select item.CreateScheme();

            return new UnionScheme(TableName, tableDesc, NameField, schemes);
        }

        #endregion IConfigCreator<ITableSchemeEx> 成员

        [SimpleAttribute(Required = true)]
        public string TableName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText TableDesc { get; private set; }

        [SimpleAttribute]
        public string NameField { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item", Required = true)]
        public List<UnionSchemeItem> Items { get; private set; }
    }
}