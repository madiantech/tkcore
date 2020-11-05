using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    [Source(Author = "YJC", CreateDate = "2015-06-23",
        Description = "导入Excel数据（未完成）")]
    internal sealed class PrepareImportDataSource : EmptyDbDataSource, ISupportMetaData, ISource
    {
        private readonly MetaDataTableResolver fResolver;
        private Tk5ListMetaData fMetaData;

        //public ImportDataSource(MetaDataTableResolver resolver)
        //{
        //    fResolver = resolver;
        //}

        public PrepareImportDataSource(IConfigCreator<TableResolver> creator)
        {
            fResolver = creator.CreateObject(this).Convert<MetaDataTableResolver>();
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData as Tk5ListMetaData;
        }

        #endregion ISupportMetaData 成员

        public OutputData DoAction(IInputData input)
        {
            try
            {
                FileInfo info = input.PostObject.Convert<FileInfo>();
                if (string.IsNullOrEmpty(info.FileName))
                    throw new WebPostException(string.Empty,
                        new FieldErrorInfo("Import", "FileName", "没有文件，请上传Excel文件"));

                ImportResultData result = new ImportResultData(fMetaData);
                ExcelImporter.ExcelImport(Context, info.ServerPath, fMetaData, result);
                var errorCollection = fResolver.Import(result.ImportDataSet, input);
                result.ImportFieldInfoError(errorCollection);
                result.SaveTemp();

                return OutputData.CreateObject(result);
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
            catch (Exception ex)
            {
                return OutputData.CreateToolkitObject(new WebErrorResult(ex.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fResolver.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}