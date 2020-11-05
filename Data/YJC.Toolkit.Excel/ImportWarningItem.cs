using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Excel
{
    public class ImportWarningItem
    {
        internal ImportWarningItem(int indexOfRow, string columnName, 
            string primitiveValue, string errorMsg)
        {
            IndexOfRow = indexOfRow;
            ColumnName = columnName;
            PrimitiveValue = primitiveValue;
            ErrorMsg = errorMsg;
        }

        public int IndexOfRow { get; private set; }

        public string ColumnName { get; private set; }

        public string PrimitiveValue { get; private set; }

        public string ErrorMsg { get; private set; }

        public override string ToString()
        {
            string stringView = string.Format("Row: {0}, Column: {1}, PrimitiveValue: {2}\nErrorMsg: {3}",
                IndexOfRow, ColumnName, PrimitiveValue, ErrorMsg);
            return stringView;
        }
    }
}
