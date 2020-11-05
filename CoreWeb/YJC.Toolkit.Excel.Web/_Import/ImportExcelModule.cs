using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;
using System.Collections.Generic;

namespace YJC.Toolkit.Excel
{
    internal class ImportExcelModule : IModule
    {
        private static readonly IEnumerable<Tk5FieldInfoEx> Index = CreateIndex();

        private class IndexField : IFieldInfoEx
        {
            #region IFieldInfoEx 成员

            public int Length
            {
                get
                {
                    return 0;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    return false;
                }
            }

            public int Precision
            {
                get
                {
                    return 0;
                }
            }

            public FieldKind Kind
            {
                get
                {
                    return FieldKind.Virtual;
                }
            }

            public string Expression
            {
                get
                {
                    return null;
                }
            }

            public IFieldLayout Layout
            {
                get
                {
                    return new FieldLayoutAttribute();
                }
            }

            public IFieldControl Control
            {
                get
                {
                    return new FieldControlAttribute(ControlType.Text);
                }
            }

            public IFieldDecoder Decoder
            {
                get
                {
                    return null;
                }
            }

            public IFieldUpload Upload
            {
                get
                {
                    return null;
                }
            }

            public bool IsShowInList(IPageStyle style, bool isInTable)
            {
                var ctrl = Control.Convert<FieldControlAttribute>();
                if ((ctrl.DefaultShow & PageStyle.List) != PageStyle.List)
                    return false;

                if (isInTable)
                    return ctrl.Control != ControlType.Hidden;
                return true;
            }

            #endregion IFieldInfoEx 成员

            #region IFieldInfo 成员

            public string FieldName
            {
                get
                {
                    return ImportResultData.ROW_INDEX;
                }
            }

            public string DisplayName
            {
                get
                {
                    return "原文件行号";
                }
            }

            public string NickName
            {
                get
                {
                    return FieldName;
                }
            }

            public TkDataType DataType
            {
                get
                {
                    return TkDataType.Int;
                }
            }

            public bool IsKey
            {
                get
                {
                    return false;
                }
            }

            public bool IsAutoInc
            {
                get
                {
                    return false;
                }
            }

            #endregion IFieldInfo 成员
        }

        private readonly ImportConfigXml fConfig;

        public ImportExcelModule(ImportConfigXml config)
        {
            fConfig = config;
        }

        #region IModule 成员

        public string Title
        {
            get
            {
                return fConfig.Import.Title.ToString();
            }
        }

        public IMetaData CreateMetaData(IPageData pageData)
        {
            string oper = CheckPageStyle(pageData);
            switch (oper)
            {
                case ImportConst.TEMPLATE:
                    return CreateImportMetaData(pageData);

                case ImportConst.PREVIEW_SUCCESS:
                case ImportConst.ERROR_IMPORT:
                    return CreateMetaDataWithIndex(pageData);

                case ImportConst.IMPORT:
                case ImportConst.DEFAULT:
                    if (pageData.IsPost)
                        return CreateImportMetaData(pageData);
                    else
                        return null;

                default:
                    return null;
            }
        }

        private IMetaData CreateMetaDataWithIndex(IPageData pageData)
        {
            IMetaData metaData = CreateImportMetaData(pageData);
            Tk5ListMetaData listMeta = metaData.Convert<Tk5ListMetaData>();
            Tk5ListMetaData result = new Tk5ListMetaData(listMeta, Index);

            return result;
        }

        private IMetaData CreateImportMetaData(IPageData pageData)
        {
            PageDataProxy data = new PageDataProxy(pageData,
                (PageStyleClass)PageStyle.List);
            BaseSingleMetaDataConfig config = fConfig.Import.MetaData as BaseSingleMetaDataConfig;
            if (config != null)
                config.DisableAutoDetailLink = true;
            IMetaData metaData = fConfig.Import.MetaData.CreateObject(data);
            if (metaData != null)
                metaData.Title = Title;
            return metaData;
        }

        public ISource CreateSource(IPageData pageData)
        {
            string oper = CheckPageStyle(pageData);
            switch (oper)
            {
                case ImportConst.DEFAULT:
                    if (!pageData.IsPost)
                        return new DefaultImportSource(fConfig);
                    else
                        return new PrepareImportDataSource(fConfig.Import.Resolver);

                case ImportConst.TEMPLATE:
                    return new EmptySource(true);

                case ImportConst.IMPORT:
                    if (pageData.IsPost)
                        return new ImportDataSource(fConfig.Import.Resolver);
                    else
                        return new ImportSuccessSource();

                case ImportConst.PREVIEW_SUCCESS:
                    return new PreviewSuccessDataSource(fConfig.Import.Resolver);

                case ImportConst.ERROR_IMPORT:
                    if (pageData.IsPost)
                        return new ImportDataSource(fConfig.Import.Resolver);
                    else
                        return new ImportErrorSource();
            }
            return null;
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            string oper = CheckPageStyle(pageData);
            if (oper == ImportConst.DEFAULT)
                return new CustomJsonObjectCreator(typeof(FileInfo), "Import");

            return null;
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (IsShowSource(pageData))
                return XmlPageMaker.DEFAULT;
            if (IsShowMetaData(pageData))
            {
                var metaMaker = "<tk:MetaDataPageMaker />".ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                    PageMakerConfigFactory.REG_NAME);
                return metaMaker.CreateObject(pageData);
            }

            string oper = CheckPageStyle(pageData);
            switch (oper)
            {
                case ImportConst.TEMPLATE:
                    return new ExportExcelHeaderPageMaker();

                case ImportConst.ERROR_IMPORT:
                    return CreateFreeRazorMaker(@"Razor\Excel\ImportError.cshtml", true);

                case ImportConst.IMPORT:
                    return CreateFreeRazorMaker(@"Razor\Excel\ImportSuccess.cshtml", true);

                case ImportConst.PREVIEW_SUCCESS:
                    return CreateFreeRazorMaker(@"Razor\Excel\PreviewSuccess.cshtml", true);

                case ImportConst.DEFAULT:
                default:
                    if (!pageData.IsPost)
                        return CreateFreeRazorMaker(@"Razor\Excel\Import.cshtml", false);
                    else
                        return ImportPostPageMaker.PAGE_MAKER;
            }
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            return null;
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return false;
        }

        #endregion IModule 成员

        private static IPageMaker CreateFreeRazorMaker(string path, bool useAssembly)
        {
            string defaultFileName = FileUtil.GetRealFileName(path,
                FilePathPosition.Xml);
            FreeRazorPageMaker pageMaker = new FreeRazorPageMaker(defaultFileName);
            //if (useAssembly)
            //    pageMaker.Assemblies = new List<string>
            //    {
            //        "YJC.Toolkit.Excel.dll", "YJC.Toolkit.Web.Excel.dll"
            //    };
            return pageMaker;
        }

        private string CheckPageStyle(IPageData pageData)
        {
            IPageStyle style = pageData.Style;
            TkDebug.Assert(style.Style == PageStyle.Custom, string.Format(ObjectUtil.SysCulture,
                "地址错误，导入地址均为Custom类型，当前页面类型为{0}", style), this);

            string oper = style.Operation;
            if (string.IsNullOrEmpty(oper))
                return ImportConst.DEFAULT;
            return oper;
        }

        private static bool IsShowSource(IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return setting.IsDebug && !pageData.IsPost
                && setting.IsShowSource(pageData.QueryString);
        }

        private static bool IsShowMetaData(IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return setting.IsDebug && !pageData.IsPost
                && setting.IsShowMetaData(pageData.QueryString);
        }

        private static IEnumerable<Tk5FieldInfoEx> CreateIndex()
        {
            Tk5FieldInfoEx field = new Tk5FieldInfoEx(new IndexField(), (PageStyleClass)PageStyle.List);
            return EnumUtil.Convert(field);
        }
    }
}