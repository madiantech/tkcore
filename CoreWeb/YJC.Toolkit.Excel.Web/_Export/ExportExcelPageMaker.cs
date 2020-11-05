using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Excel;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ExportExcelPageMaker : BaseExportExcelPageMaker
    {
        public ExportExcelPageMaker()
            : base()
        {
        }

        public ExportExcelPageMaker(ExcelContentFormat header, ExcelContentFormat content)
            : base(header, content)
        {
        }

        internal ExportExcelPageMaker(ExportExcelPageMakerConfig exportExcelPageMakerConfig)
        {
            UserBorder = exportExcelPageMakerConfig.UserBorder;
            Header = exportExcelPageMakerConfig.Header;
            Content = exportExcelPageMakerConfig.Content;
        }

        protected sealed override byte[] CreateExcelData(ExcelExporter exporter, OutputData outputData)
        {
            byte[] data = null;
            switch (outputData.OutputType)
            {
                case SourceOutputType.ToolkitObject:
                case SourceOutputType.Object:
                    ObjectListModel model = outputData.Data as ObjectListModel;
                    if (model != null)
                        data = exporter.Export(model);
                    break;
                case SourceOutputType.DataSet:
                    data = exporter.Export(outputData.Data.Convert<DataSet>());
                    break;
            }

            return data;
        }
    }
}
