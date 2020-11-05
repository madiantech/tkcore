using System.Collections.Generic;
using YJC.Toolkit.Collections;

namespace YJC.Toolkit.MetaData
{
    internal class StdCodeTableScheme : ITableSchemeEx, ITableScheme
    {
        private bool fShowSort;
        private bool fShowCodeValue;
        private bool fShowPy;
        private readonly RegNameList<CodeTableFieldInfoEx> fFields;
        private readonly CodeTableFieldInfoEx fValueField;
        private readonly CodeTableFieldInfoEx fSortField;
        private readonly CodeTableFieldInfoEx fPyField;

        public StdCodeTableScheme(string tableName, bool showCodeValue, bool showSort, bool showPy, string pyCaption)
        {
            TableName = tableName;
            fValueField = CodeTableFieldInfoEx.CreateValueField();
            fSortField = CodeTableFieldInfoEx.CreateSortField();
            fPyField = CodeTableFieldInfoEx.CreatePyField(pyCaption);
            var nameField = CodeTableFieldInfoEx.CreateNameField();
            NameField = nameField;
            fFields = new RegNameList<CodeTableFieldInfoEx>() {
                fValueField, nameField, fPyField, fSortField,
                CodeTableFieldInfoEx.CreateActiveField()
            };
            ShowSort = showSort;
            ShowPy = showPy;
            ShowCodeValue = showCodeValue;
        }

        #region ITableSchemeEx 成员

        public string TableName { get; private set; }

        public string TableDesc
        {
            get
            {
                return TableName;
            }
        }

        public IFieldInfoEx NameField { get; }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fFields;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fFields;
            }
        }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fFields[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region ITableScheme 成员

        IEnumerable<IFieldInfo> ITableScheme.Fields
        {
            get
            {
                return Fields;
            }
        }

        IEnumerable<IFieldInfo> ITableScheme.AllFields
        {
            get
            {
                return Fields;
            }
        }

        #endregion ITableScheme 成员

        public bool ShowCodeValue
        {
            get
            {
                return fShowCodeValue;
            }
            private set
            {
                if (fShowCodeValue != value)
                {
                    fShowCodeValue = value;
                    if (fShowCodeValue)
                        fValueField.SetControl(ControlType.Text);
                    else
                        fValueField.SetControl(ControlType.Hidden);
                }
            }
        }

        public bool ShowSort
        {
            get
            {
                return fShowSort;
            }
            private set
            {
                if (fShowSort != value)
                {
                    fShowSort = value;
                    if (fShowSort)
                        fSortField.SetDefaultShow(PageStyle.All);
                    else
                        fSortField.SetDefaultShow(PageStyle.None);
                }
            }
        }

        public bool ShowPy
        {
            get
            {
                return fShowPy;
            }
            private set
            {
                if (fShowPy != value)
                {
                    fShowPy = value;
                    if (fShowPy)
                        fPyField.SetDefaultShow(PageStyle.All);
                    else
                        fPyField.SetDefaultShow(PageStyle.Update | PageStyle.List);
                }
            }
        }
    }
}