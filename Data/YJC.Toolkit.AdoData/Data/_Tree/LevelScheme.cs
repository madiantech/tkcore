using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    internal class LevelScheme : ITableScheme
    {
        private readonly RegNameList<FieldItem> fFields;

        public LevelScheme(ITableScheme sourceScheme, LevelTreeDefinition levelDef)
        {
            fFields = new RegNameList<FieldItem>();

            fFields.Add(new KeyFieldItem(sourceScheme[levelDef.IdField]));
            fFields.Add(new FieldItem(sourceScheme[levelDef.NameField]));
            TreeScheme.AddFakeField(fFields, sourceScheme);
            TableName = sourceScheme.TableName;
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
    }
}