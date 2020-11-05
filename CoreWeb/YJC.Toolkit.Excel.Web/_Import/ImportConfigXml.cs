using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class ImportConfigXml : ToolkitConfig
    {

        //#region IModule 成员

        //public string Title
        //{
        //    get
        //    {
        //        return Import.Title.ToString();
        //    }
        //}

        //public IMetaData CreateMetaData(IPageData pageData)
        //{
        //    string oper = pageData.QueryString["Mode"];
        //    if (pageData.IsPost || oper == ImportConst.TEMPLATE || oper == ImportConst.ERROR_EXCEL)
        //        return Import.MetaData.CreateObject(pageData);
        //    else
        //        return null;
        //}

        //public ISource CreateSource(IPageData pageData)
        //{
        //    string oper = pageData.QueryString["Mode"];
        //    if (pageData.IsPost || oper == ImportConst.IMPORT)
        //    {
        //        EmptyDbDataSource source = new EmptyDbDataSource();
        //        TableResolver resolver = Import.Resolver.CreateObject(source);
        //        MetaDataTableResolver metaResolver = resolver as MetaDataTableResolver;
        //        return new ImportDataSource(metaResolver); 
        //    }
        //    else
        //    {
        //        switch (oper)
        //        {
        //            case ImportConst.ERROR_EXCEL:
        //                return new EmptySource(true);
        //            default:
        //                return new EmptySource(true);
        //        }
        //    }
        //}

        //public IPostObjectCreator CreatePostCreator(IPageData pageData)
        //{
        //    return new CustomJsonObjectCreator(typeof(FileInfo), "Import");
        //}

        //public IPageMaker CreatePageMaker(IPageData pageData)
        //{
        //    if (pageData.IsPost)
        //    {
        //        string postFileName = FileUtil.GetRealFileName(@"Razor\Excel\PostExcel.cshtml", 
        //            FilePathPosition.Xml);
        //        return new FreeRazorPageMaker(postFileName);
        //    }
        //    else
        //    {
        //        string oper = pageData.QueryString["Mode"];
        //        switch (oper)
        //        {
        //            case ImportConst.TEMPLATE:
        //                return new ExportExcelHeaderPageMaker();
        //            case ImportConst.ERROR_EXCEL:
        //                return new ExcelErrorDataPageMaker();
        //            case ImportConst.IMPORT:
        //                var result = new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
        //                    new CustomUrlConfig(false, false, "CloseDialog"));
        //                return result;
        //            default:
        //                string defaultFileName = FileUtil.GetRealFileName(@"Razor\Excel\Import.cshtml",
        //                    FilePathPosition.Xml);
        //                return new FreeRazorPageMaker(defaultFileName);
        //        }
        //    }
        //}

        //public IRedirector CreateRedirector(IPageData pageData)
        //{
        //    throw new NotSupportedException();
        //}

        //public bool IsSupportLogOn(IPageData pageData)
        //{
        //    return false;
        //}

        //public bool IsDisableInjectCheck(IPageData pageData)
        //{
        //    return false;
        //}

        //public bool IsCheckSubmit(IPageData pageData)
        //{
        //    return false;
        //}

        //#endregion

        [ObjectElement(NamespaceType.Toolkit)]
        public ImportConfigItem Import { get; private set; }
    }
}
