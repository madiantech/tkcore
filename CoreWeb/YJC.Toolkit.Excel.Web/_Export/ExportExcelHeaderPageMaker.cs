using YJC.Toolkit.Excel;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ExportExcelHeaderPageMaker : BaseExportExcelPageMaker
    {
        public ExportExcelHeaderPageMaker()
        {
            UserBorder = false;
        }

        public ExportExcelHeaderPageMaker(ExcelContentFormat header, ExcelContentFormat content)
            : base(header, content)
        {
            UserBorder = false;
        }

        protected override string GetFileNameWithoutExtension(string fileName)
        {
            return fileName + "模板";
        }

        public override bool CanUseMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)ImportConst.TEMPLATE);
        }

        protected sealed override byte[] CreateExcelData(ExcelExporter exporter, OutputData outputData)
        {
            byte[] data = exporter.CreateExcelTemplate();
            return data;
        }
    }
}
