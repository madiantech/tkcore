using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public abstract class BaseColumnReader
    {
        protected BaseColumnReader(Tk5FieldInfoEx fieldInfo, int cellIndex)
        {
            FieldInfo = fieldInfo;
            CellIndex = cellIndex;
        }

        public Tk5FieldInfoEx FieldInfo { get; private set; }

        public int CellIndex { get; private set; }

        protected abstract string ConvertValue(TkDbContext context, string value);

        public void ReadColumn(DataRow row, IRow cellRow, TkDbContext context)
        {
            try
            {
                row[FieldInfo.NickName] = ReadColumn(cellRow, context);
            }
            catch (Exception ex)
            {
                throw new ImportConvertException(FieldInfo.NickName, cellRow.RowNum, ex.Message, ex);
            }
        }

        public void ReadErrorColumn(DataRow row, IRow cellRow)
        {
            ICell cell = cellRow.GetCell(CellIndex);
            if (cell == null)
                row[FieldInfo.NickName] = DBNull.Value;
            else
                row[FieldInfo.NickName] = cell.ToString();
        }

        private object ReadColumn(IRow cellRow, TkDbContext context)
        {
            ICell cell = cellRow.GetCell(CellIndex);
            if (cell == null)
                return DBNull.Value;

            string value = cell.ToString();
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            return ConvertValue(context, value);
        }
    }
}
