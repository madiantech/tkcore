using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class UnionFieldInfoEx : IFieldInfoEx, IFieldInfo, IRegName
    {
        private readonly IFieldInfoEx fSourceField;

        public UnionFieldInfoEx(IFieldInfoEx field, string newNickName)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            fSourceField = field;
            FieldName = field.FieldName;
            NickName = string.IsNullOrEmpty(newNickName) ? field.NickName : newNickName;
            DataType = field.DataType;
            IsKey = field.IsKey;
            IsAutoInc = field.IsAutoInc;
            Length = field.Length;
            IsEmpty = field.IsEmpty;
            Precision = field.Precision;
            DisplayName = field.DisplayName;
            Expression = field.Expression;
            Kind = field.Kind;
        }

        #region IFieldInfo 成员

        public string FieldName { get; private set; }

        public string DisplayName { get; private set; }

        public string NickName { get; private set; }

        public TkDataType DataType { get; private set; }

        public bool IsKey { get; private set; }

        public bool IsAutoInc { get; private set; }

        #endregion IFieldInfo 成员

        #region IFieldInfoEx 成员

        public int Length { get; private set; }

        public bool IsEmpty { get; private set; }

        public int Precision { get; private set; }

        public FieldKind Kind { get; private set; }

        public string Expression { get; private set; }

        public IFieldLayout Layout
        {
            get
            {
                return fSourceField.Layout;
            }
        }

        public IFieldControl Control
        {
            get
            {
                return fSourceField.Control;
            }
        }

        public IFieldDecoder Decoder
        {
            get
            {
                return fSourceField.Decoder;
            }
        }

        public IFieldUpload Upload
        {
            get
            {
                return fSourceField.Upload;
            }
        }

        public bool IsShowInList(IPageStyle style, bool isInTable)
        {
            return fSourceField.IsShowInList(style, isInTable);
        }

        #endregion IFieldInfoEx 成员

        public IFieldInfoEx SourceField
        {
            get
            {
                return fSourceField;
            }
        }

        public string RegName
        {
            get
            {
                return NickName;
            }
        }
    }
}