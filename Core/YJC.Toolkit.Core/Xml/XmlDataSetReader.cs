using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    public sealed class XmlDataSetReader : CustomElementXmlReader
    {
        private readonly Type DateTimeType = typeof(DateTime);

        private int fTableIndex;
        private readonly int fTableCount;
        private int fColumnIndex;
        private int fColumnCount;
        private int fRowIndex;
        private int fRowCount;

        //private bool fEmptyTable;
        //private bool fEmptyRow;
        private DataTable fCurrentTable;

        private DataRow fCurrentRow;
        private readonly string fRoot;
        private readonly bool fCloseInput;
        private readonly bool fIgnoreEmptyField;

        public XmlDataSetReader(DataSet dataSet)
            : this(dataSet, false)
        {
        }

        public XmlDataSetReader(DataSet dataSet, bool closeInput)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", null);

            DataSet = dataSet;
            fRoot = NameTable.Add(dataSet.DataSetName);
            fTableCount = dataSet.Tables.Count;
            fCloseInput = closeInput;
            fIgnoreEmptyField = true;
        }

        public XmlDataSetReader(DataSet dataSet, bool closeInput, bool ignoreEmptyField)
            : this(dataSet, closeInput)
        {
            fIgnoreEmptyField = ignoreEmptyField;
        }

        private void SetTableInfo()
        {
            fCurrentTable = DataSet.Tables[fTableIndex];
            fColumnCount = fCurrentTable.Columns.Count;
            fRowCount = fCurrentTable.Rows.Count;
        }

        private void CheckTableIndex()
        {
            if (fTableIndex < fTableCount)
            {
                SetTableInfo();
                while (fRowCount == 0)
                {
                    ++fTableIndex;
                    if (fTableIndex < fTableCount)
                        SetTableInfo();
                    else
                    {
                        State = XmlReaderState.EndRoot;
                        return;
                    }
                }
                State = XmlReaderState.Row;
                fRowIndex = 0;
            }
            else
                State = XmlReaderState.EndRoot;
        }

        public DataSet DataSet { get; private set; }

        public override string BaseURI
        {
            get
            {
                return EmptyString;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (fCloseInput)
                    DataSet.DisposeObject();
            }
        }

        public override void Close()
        {
            State = XmlReaderState.Close;
        }

        public override bool IsEmptyElement
        {
            get
            {
                //if (State == XmlReaderState.Row && (fEmptyTable || fEmptyRow))
                //    return true;
                return false;
            }
        }

        public override string LocalName
        {
            get
            {
                switch (State)
                {
                    case XmlReaderState.Field:
                    case XmlReaderState.EndField:
                        string columnName = fCurrentTable.Columns[fColumnIndex].ColumnName;
                        if (State == XmlReaderState.Field)
                            return NameTable.Add(columnName);
                        else
                            return NameTable.Get(columnName);

                    case XmlReaderState.Root:
                    case XmlReaderState.EndRoot:
                        return fRoot;

                    case XmlReaderState.Row:
                        return NameTable.Add(fCurrentTable.TableName);

                    case XmlReaderState.EndRow:
                        return NameTable.Get(fCurrentTable.TableName);
                }
                return EmptyString;
            }
        }

        private void ReadFieldValue()
        {
            fCurrentRow = fCurrentTable.Rows[fRowIndex];
            if (fIgnoreEmptyField)
            {
                while (fColumnIndex < fColumnCount)
                {
                    object value = fCurrentRow[fColumnIndex];
                    if (value != DBNull.Value)
                        if (!string.IsNullOrEmpty(value.ToString()))
                            break;
                    ++fColumnIndex;
                }
            }
            if (fColumnIndex < fColumnCount)
                State = XmlReaderState.Field;
            else
                State = XmlReaderState.EndRow;
        }

        public override bool Read()
        {
            switch (State)
            {
                case XmlReaderState.Initial:
                    State = XmlReaderState.Root;
                    return true;

                case XmlReaderState.Root:
                    CheckTableIndex();
                    return true;

                case XmlReaderState.Row:
                    if (fRowIndex < fRowCount)
                    {
                        fColumnIndex = 0;
                        ReadFieldValue();
                    }
                    else
                    {
                        if (fRowCount > 0)
                        {
                            ++fTableIndex;
                            CheckTableIndex();
                        }
                        else
                            State = XmlReaderState.EndRow;
                    }
                    return true;

                case XmlReaderState.Field:
                    State = XmlReaderState.FieldValue;
                    return true;

                case XmlReaderState.FieldValue:
                    State = XmlReaderState.EndField;
                    return true;

                case XmlReaderState.EndField:
                    ++fColumnIndex;
                    ReadFieldValue();
                    return true;

                case XmlReaderState.EndRow:
                    ++fRowIndex;
                    if (fRowIndex < fRowCount)
                        State = XmlReaderState.Row;
                    else
                    {
                        ++fTableIndex;
                        CheckTableIndex();
                    }
                    return true;

                case XmlReaderState.EndRoot:
                    State = XmlReaderState.Eof;
                    return true;

                case XmlReaderState.Eof:
                    return false;
            }
            return false;
        }

        public override string Value
        {
            get
            {
                if (State == XmlReaderState.FieldValue)
                {
                    DataColumn column = fCurrentTable.Columns[fColumnIndex];
                    object obj = fCurrentRow[column];
                    if (obj == DBNull.Value)
                        return string.Empty;

                    if (column.DataType == DateTimeType)
                        return ((DateTime)obj).ToString(ToolkitConst.DATETIME_FMT_STR,
                            ObjectUtil.SysCulture);
                    else if (column.DataType == typeof(byte[]))
                        return Convert.ToBase64String((byte[])obj);
                    else
                        return obj.ToString();
                }
                return null;
            }
        }
    }
}