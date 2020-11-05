using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public static class ExcelImporter
    {
        #region 根据模板sheet和元数据，将Excel文件的内容导入到DataSet中

        public static void ExcelImport(TkDbContext context, string strFileName,
            Tk5ListMetaData metaData, ImportResultData result)
        {
            string sheetName = metaData.Table.TableDesc;
            ISheet sheet = null;

            string fileExt = Path.GetExtension(strFileName).ToLower(ObjectUtil.SysCulture);
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheet(sheetName);
                }
                else if (fileExt == ".xlsx")
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheet(sheetName);
                }
                else
                    throw new WebPostException("上传的文件不是Excel文件，请确认上传文件的格式");
            }
            if (sheet != null)
                SheetImport(context, metaData, sheet, result);
        }

        private static void SheetImport(TkDbContext context, Tk5ListMetaData metaInfos,
            ISheet sheet, ImportResultData result)
        {
            Dictionary<string, Tk5FieldInfoEx> dicOfInfo = new Dictionary<string, Tk5FieldInfoEx>();
            foreach (Tk5FieldInfoEx info in metaInfos.Table.TableList)
            {
                dicOfInfo.Add(info.DisplayName, info);
            }

            IRow headerRow = sheet.GetRow(0);
            int firstCell = headerRow.FirstCellNum;
            int colLength = headerRow.LastCellNum - firstCell;
            List<BaseColumnReader> readers = new List<BaseColumnReader>(colLength);
            string[] colNames = new string[colLength];
            for (int i = 0; i < colLength; i++)
            {
                int cellIndex = i + firstCell;
                string name = headerRow.GetCell(cellIndex).ToString();
                Tk5FieldInfoEx fieldInfo = dicOfInfo[name];
                if (fieldInfo != null)
                {
                    BaseColumnReader reader = null;
                    if (fieldInfo.InternalControl != null && fieldInfo.InternalControl.SrcControl == ControlType.CheckBox)
                        reader = new BoolColumnReader(fieldInfo, cellIndex);
                    else if (fieldInfo.Decoder != null && fieldInfo.Decoder.Type == DecoderType.CodeTable)
                        reader = new CodeTableColumnReader(fieldInfo, context, cellIndex);
                    else if (fieldInfo.Decoder != null && fieldInfo.Decoder.Type == DecoderType.EasySearch)
                        reader = new EasySearchColumnReader(fieldInfo, cellIndex);
                    else
                        reader = new NormalColumnReader(fieldInfo, cellIndex);
                    readers.Add(reader);
                }
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = result.ImportTable.NewRow();
                try
                {
                    dataRow.BeginEdit();
                    try
                    {
                        foreach (var reader in readers)
                            reader.ReadColumn(dataRow, row, context);
                        dataRow[ImportResultData.ROW_INDEX] = i;
                    }
                    finally
                    {
                        dataRow.EndEdit();
                    }
                    result.ImportTable.Rows.Add(dataRow);
                }
                catch (ImportConvertException ex)
                {
                    result.AddErrorItem(ex.Row, ex.NickName, ex.Message);
                    DataRow errorRow = result.ErrorTable.NewRow();
                    errorRow.BeginEdit();
                    try
                    {
                        foreach (var reader in readers)
                            reader.ReadErrorColumn(errorRow, row);
                        errorRow[ImportResultData.ROW_INDEX] = i;
                    }
                    finally
                    {
                        errorRow.EndEdit();
                    }
                }
            }
        }

        // 设置DataTable的值
        private static void ReadColumn(DataRow dataRow, string columnName,
            Tk5FieldInfoEx fieldInfo, string strValue, int indexOfRow, ImportResultData result)
        {
            ImportWarningItem imResult = null;
            string asgValue = null;
            if (fieldInfo != null)
            {
                bool valueError = false;
                if (fieldInfo.Decoder != null && fieldInfo.Decoder.Type == DecoderType.CodeTable)
                {
                    IEnumerable<IDecoderItem> data = ExcelUtil.GetDecoderItem(fieldInfo, null);

                    if (string.IsNullOrEmpty(strValue))
                    {
                        valueError = true;
                    }
                    else
                    {
                        foreach (IDecoderItem item in data)
                        {
                            if (item.Name == strValue)
                            {
                                asgValue = item.Value;
                                valueError = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (fieldInfo.InternalControl != null && fieldInfo.InternalControl.SrcControl == ControlType.CheckBox)
                    {
                        if (strValue == "√")
                        {
                            asgValue = ((fieldInfo.Extension == null) ? "1" : fieldInfo.Extension.CheckValue);
                            valueError = true;
                        }
                        if (strValue == "X" || string.IsNullOrEmpty(strValue))
                        {
                            asgValue = ((fieldInfo.Extension == null) ? "0" : fieldInfo.Extension.UnCheckValue);
                            valueError = true;
                        }
                    }
                    else
                    {
                        asgValue = strValue;
                        valueError = true;
                    }
                }

                try
                {
                    if (!valueError)
                    {
                        throw new Exception("value in the cell is invalid");
                    }
                    else
                        dataRow[fieldInfo.NickName] = asgValue;
                }
                catch (Exception ex)
                {
                    imResult = new ImportWarningItem(indexOfRow, columnName, asgValue, ex.Message);
                }
            }
            //return imResult;
        }

        #endregion 根据模板sheet和元数据，将Excel文件的内容导入到DataSet中
    }
}