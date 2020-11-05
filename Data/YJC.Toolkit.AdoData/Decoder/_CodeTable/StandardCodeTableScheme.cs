using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class StandardCodeTableScheme : ITableScheme, IDisplayObject
    {
        private readonly RegNameList<FieldItem> fFields;
        private readonly FieldItem fCode;
        private readonly FieldItem fName;

        public StandardCodeTableScheme(string tableName)
        {
            TkDebug.AssertArgumentNull(tableName, "tableName", null);

            TableName = tableName;
            fCode = DecoderConst.CODE_FIELD;
            fName = DecoderConst.NAME_FIELD;
            fFields = new RegNameList<FieldItem>() {
                fCode, fName, DecoderConst.PY_FIELD,
                DecoderConst.ACTIVE_FIELD, DecoderConst.SORT_FIELD
            };
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

        #region IDisplayObject 成员

        public bool SupportDisplay
        {
            get
            {
                return true;
            }
        }

        public IFieldInfo Id
        {
            get
            {
                return fCode;
            }
        }

        public IFieldInfo Name
        {
            get
            {
                return fName;
            }
        }

        #endregion IDisplayObject 成员
    }
}