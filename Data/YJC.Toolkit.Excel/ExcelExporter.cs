using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public class ExcelExporter
    {
        internal enum Model
        {
            Content,
            Header
        }

        private readonly bool fUseBorder;
        private readonly ExcelContentFormat fHeader;
        private readonly ExcelContentFormat fContent;
        private readonly Tk5ListMetaData fMetaData;

        public ExcelExporter(bool useBorder, ExcelContentFormat header,
            ExcelContentFormat content, Tk5ListMetaData metaData)
        {
            TkDebug.AssertArgumentNull(header, "header", null);
            TkDebug.AssertArgumentNull(content, "content", null);
            TkDebug.AssertArgumentNull(metaData, "metaData", null);

            fUseBorder = useBorder;
            fHeader = header;
            fContent = content;
            fMetaData = metaData;
        }

        #region 边框、字体以及对齐设置
        private ICellStyle BorderAndFontSetting(IWorkbook wb, Tk5FieldInfoEx metaInfo, Model model)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            if (fUseBorder)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
            }

            if (model == Model.Content)
            {
                IFont fontContent = ExcelUtil.FontSetting(wb, fContent);

                cellStyle.SetFont(fontContent);

                if (metaInfo.Extension != null)
                {
                    ExcelUtil.AlignSetting(metaInfo.Extension.Align, cellStyle);
                }
                else
                {
                    ExcelUtil.AlignSetting(fContent.Align, cellStyle);
                }
            }
            else
            {
                ExcelUtil.AlignSetting(fHeader.Align, cellStyle);
                IFont fontHeader = ExcelUtil.FontSetting(wb, fHeader);
                cellStyle.SetFont(fontHeader);
            }
            return cellStyle;
        }

        #endregion

        #region 内容格式创建

        //生成Dictionary<NickName, ICellStyle>， 每一列对应每一个ICellStyle，通过NickName来获取ICellStyle
        private Dictionary<string, ICellStyle> GetContentStyles(IWorkbook workbook)
        {
            Dictionary<string, ICellStyle> NameToStyle = new Dictionary<string, ICellStyle>();
            IDataFormat format = workbook.CreateDataFormat();

            foreach (Tk5FieldInfoEx fieldInfo in fMetaData.Table.TableList)
            {
                ICellStyle cellStyle = BorderAndFontSetting(workbook, fieldInfo, Model.Content);
                if (fieldInfo.Extension != null && !string.IsNullOrEmpty(fieldInfo.Extension.Format))
                    cellStyle.DataFormat = format.GetFormat("@");
                else
                {
                    switch (fieldInfo.DataType)
                    {
                        case TkDataType.Long:
                        case TkDataType.Int:
                        case TkDataType.Short:
                        case TkDataType.Byte:
                        case TkDataType.Double:
                        case TkDataType.Decimal:
                        case TkDataType.Money:
                            if (fieldInfo.DataType == TkDataType.Money)
                                cellStyle.DataFormat = format.GetFormat("￥#,##0");
                            else
                                cellStyle.DataFormat = format.GetFormat("0");
                            break;
                        case TkDataType.String:
                        case TkDataType.Text:
                        case TkDataType.Guid:
                        case TkDataType.Xml:
                            cellStyle.DataFormat = format.GetFormat("@");
                            break;
                        case TkDataType.DateTime:
                        case TkDataType.Date:
                            if (fieldInfo.DataType == TkDataType.DateTime)
                                cellStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm");
                            else
                                cellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");
                            break;
                    }
                }
                NameToStyle.Add(fieldInfo.NickName, cellStyle);
            }
            return NameToStyle;
        }

        #endregion

        // 表格头部设置
        private void HeaderSetting(IWorkbook workbook, ISheet sheet)
        {
            IRow dataRow = sheet.CreateRow(0);

            int index = 0;
            foreach (Tk5FieldInfoEx fieldInfo in fMetaData.Table.TableList)
            {
                ICell cell = dataRow.CreateCell(index);
                ICellStyle styleHeader = BorderAndFontSetting(workbook, fieldInfo, Model.Header);
                cell.SetCellValue(fieldInfo.DisplayName);
                cell.CellStyle = styleHeader;
                int colWith = ExcelUtil.GetColWidth(fieldInfo);
                sheet.SetColumnWidth(index, colWith << 8);
                index++;
            }
        }

        // 由微信类生成Excel
        private void ObjectExport(Dictionary<string, ICellStyle> ContentStyles,
            ISheet sheet, ObjectListModel objectLM)
        {
            int rowIndex = 1;
            foreach (ObjectContainer container in objectLM.List)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int columnIndex = 0;
                object receiver = container.MainObject;
                string strValue = string.Empty;
                foreach (Tk5FieldInfoEx fieldInfo in fMetaData.Table.TableList)
                {
                    ICell cell = dataRow.CreateCell(columnIndex);

                    if (fieldInfo != null)
                    {
                        if (fieldInfo.Decoder == null || fieldInfo.Decoder.Type == DecoderType.None)
                        {
                            strValue = receiver.MemberValue(fieldInfo.NickName).ConvertToString();
                            ExcelUtil.CellPadding(strValue, cell, fieldInfo);
                        }
                        else
                            strValue = container.Decoder.GetNameString(fieldInfo.NickName);

                        cell.CellStyle = ContentStyles[fieldInfo.NickName];
                    }
                    columnIndex++;
                }
                rowIndex++;
            }
        }

        // 由DataTable生成Excel
        private void DataTableExport(Dictionary<string, ICellStyle> ContentStyles,
            ISheet sheet, DataTable dt)
        {
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int columnIndex = 0;
                foreach (Tk5FieldInfoEx fieldInfo in fMetaData.Table.TableList)
                {
                    ICell cell = dataRow.CreateCell(columnIndex);

                    if (fieldInfo != null)
                    {
                        string strValue = string.Empty;
                        Tk5ExtensionConfig ex = fieldInfo.Extension;
                        SimpleFieldControl sfctrl = fieldInfo.InternalControl;
                        if (fieldInfo.Decoder == null || fieldInfo.Decoder.Type == DecoderType.None)
                        {
                            strValue = (row[fieldInfo.NickName]).ToString();

                            if (!string.IsNullOrEmpty(strValue))
                            {
                                if (sfctrl != null && sfctrl.SrcControl == ControlType.CheckBox)
                                {
                                    if ((ex != null && strValue == ex.CheckValue) ||
                                        (ex == null && strValue == "1"))
                                        cell.SetCellValue("√");
                                }
                                else
                                    ExcelUtil.CellPadding(strValue, cell, fieldInfo);
                            }
                        }
                        else
                        {
                            strValue = row[fieldInfo.NickName + "_Name"].ToString();

                            if (!string.IsNullOrEmpty(strValue))
                            {
                                if (sfctrl != null &&
                                    (sfctrl.SrcControl == ControlType.CheckBoxList ||
                                    sfctrl.SrcControl == ControlType.MultipleEasySearch))
                                {
                                    MultipleDecoderData data = MultipleDecoderData.ReadFromString(strValue);
                                    cell.SetCellValue(string.Join(", ", data));
                                }
                                else
                                    cell.SetCellValue(strValue);
                            }
                        }
                        cell.CellStyle = ContentStyles[fieldInfo.NickName];
                    }
                    columnIndex++;
                }
                rowIndex++;
            }
        }

        public byte[] CreateExcelTemplate()
        {
            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                HSSFWorkbook workbook = CreateWorkbookTemplate();
                workbook.Write(ms);
                ms.Flush();
                return ms.ToArray();
            }
        }

        public HSSFWorkbook CreateWorkbookTemplate()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(fMetaData.Table.TableDesc);
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            int index = 0;

            foreach (Tk5FieldInfoEx fieldInfo in fMetaData.Table.TableList)
            {
                int colWith = ExcelUtil.GetColWidth(fieldInfo);
                sheet.SetColumnWidth(index, colWith << 8);
                ICellStyle styleContent = BorderAndFontSetting(workbook, fieldInfo, Model.Content);
                HSSFDataValidation dataValidate = ExcelUtil.CreateDataValidation(index, fieldInfo, styleContent, workbook);
                sheet.SetDefaultColumnStyle(index, styleContent);
                if (dataValidate != null)
                {
                    ((HSSFSheet)sheet).AddValidationData(dataValidate);
                }

                ICell cell = dataRow.CreateCell(index);
                ICellStyle styleHeader = BorderAndFontSetting(workbook, fieldInfo, Model.Header);
                cell.SetCellValue(fieldInfo.DisplayName);
                cell.CellStyle = styleHeader;
                index++;
            }
            return workbook;
        }


        public byte[] Export(DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);

            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(fMetaData.Table.TableDesc);
                HeaderSetting(workbook, sheet);
                Dictionary<string, ICellStyle> ContentStyles = GetContentStyles(workbook);

                DataTable dt = dataSet.Tables[fMetaData.Table.TableName];
                if (dt != null)
                    DataTableExport(ContentStyles, sheet, dt);

                workbook.Write(ms);
                ms.Flush();
                return ms.ToArray();
            }
        }

        public byte[] Export(ObjectListModel listModel)
        {
            TkDebug.AssertArgumentNull(listModel, "listModel", null);

            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(fMetaData.Table.TableDesc);
                HeaderSetting(workbook, sheet);
                Dictionary<string, ICellStyle> ContentStyles = GetContentStyles(workbook);

                if (listModel.List != null)
                    ObjectExport(ContentStyles, sheet, listModel);

                workbook.Write(ms);
                ms.Flush();
                return ms.ToArray();
            }
        }
    }
}
