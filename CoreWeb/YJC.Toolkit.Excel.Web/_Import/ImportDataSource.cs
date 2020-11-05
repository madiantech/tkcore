using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    [Source(Author = "Administrator", CreateDate = "2016/12/29 19:47:16",
        Description = "数据源")]
    internal class ImportDataSource : BaseDbSource
    {
        private readonly IConfigCreator<TableResolver> fCreator;

        public ImportDataSource(IConfigCreator<TableResolver> creator)
        {
            fCreator = creator;
        }

        public override OutputData DoAction(IInputData input)
        {
            ImportResultData importResult = ImportUtil.GetResultData(input);

            MetaDataTableResolver resolver = fCreator.CreateObject(this).
                Convert<MetaDataTableResolver>();
            using (resolver)
            {
                resolver.Import(importResult.ImportDataSet, input);
                resolver.UpdateDatabase();
            }
            WebSuccessResult result = new WebSuccessResult("CloseDialog")
            {
                AlertMessage = "导入成功"
            };
            return OutputData.CreateToolkitObject(result);
        }
    }
}