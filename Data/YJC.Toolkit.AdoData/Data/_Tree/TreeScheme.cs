using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    internal class TreeScheme : ITableScheme
    {
        private readonly RegNameList<FieldItem> fFields;

        public TreeScheme(ITableScheme sourceScheme, DbTreeDefinition treeDef)
        {
            fFields = new RegNameList<FieldItem>();

            TableName = sourceScheme.TableName;
            fFields.Add(new KeyFieldItem(sourceScheme[treeDef.IdField]));
            fFields.Add(new FieldItem(sourceScheme[treeDef.NameField]));
            fFields.Add(new FieldItem(sourceScheme[treeDef.ParentIdField]));
            fFields.Add(new FieldItem(sourceScheme[treeDef.LeafField]));
            fFields.Add(new FieldItem(sourceScheme[treeDef.LayerField]));
            AddFakeField(fFields, sourceScheme);
        }

        #region ITableScheme 成员

        public string TableName { get; private set; }

        public IEnumerable<IFieldInfo> Fields
        {
            get
            {
                return fFields;
            }
        }

        public IEnumerable<IFieldInfo> AllFields
        {
            get
            {
                return fFields;
            }
        }

        #endregion ITableScheme 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fFields[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        public static void AddFakeField(RegNameList<FieldItem> fields, ITableScheme scheme)
        {
            Tk5DataXml dataXml = scheme as Tk5DataXml;
            if (dataXml != null)
            {
                if (dataXml.FakeDeleteInfo != null)
                    fields.Add(new FieldItem(scheme[dataXml.FakeDeleteInfo.FieldName]));
            }
        }
    }
}