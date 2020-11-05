using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public abstract class BaseSingleMetaDataConfig : ISingleMetaData, IReadObjectCallBack
    {
        protected BaseSingleMetaDataConfig()
        {
        }

        #region IReadObjectCallBack 成员

        public virtual void OnReadObject()
        {
            // ColumnCount必须在1-5之间
            if (ColumnCount < 1)
                ColumnCount = 1;
            else if (ColumnCount > 5)
                ColumnCount = 5;
        }

        #endregion

        [SimpleAttribute(DefaultValue = 3)]
        public int ColumnCount { get; protected set; }

        [SimpleAttribute]
        public bool CommitDetail { get; protected set; }

        [SimpleAttribute]
        public string TableName { get; protected set; }

        [SimpleAttribute]
        public JsonObjectType JsonDataType { get; protected set; }

        [SimpleAttribute]
        public bool DisableAutoDetailLink { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText TableDesc { get; protected set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "Add")]
        [ObjectElement(NamespaceType.Toolkit, LocalName = "AddField", IsMultiple = true)]
        internal List<AddFieldConfig> AddFields { get; set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "Override")]
        [ObjectElement(NamespaceType.Toolkit, LocalName = "Field", IsMultiple = true)]
        internal RegNameList<OverrideFieldConfig> OverrideFields { get; set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "Except")]
        [ObjectElement(NamespaceType.Toolkit, LocalName = "DelField", IsMultiple = true)]
        internal List<BaseFieldConfig> DelFields { get; set; }

        public abstract ITableSchemeEx CreateSourceScheme(IInputData input);

        public abstract Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input);
    }
}
