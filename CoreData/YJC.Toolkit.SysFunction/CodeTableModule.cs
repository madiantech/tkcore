using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SysFunction
{
    internal class CodeTableModule : IModule
    {
        private readonly string fTableName;
        private readonly IConfigCreator<ISource> fSource;
        private readonly IConfigCreator<IMetaData> fMetaData;
        private readonly IConfigCreator<IPageMaker> fPageMaker;

        public CodeTableModule(DataRow row)
        {
            Title = row["Description"].ToString();
            fTableName = row["TableName"].ToString();
            string keyCreator;
            if (GetBoolValue(row, "IsAutoKey"))
                keyCreator = string.Format(ObjectUtil.SysCulture, DataString.KeyCreator, row["CodeLength"]);
            else
                keyCreator = string.Empty;

            string xml = string.Format(ObjectUtil.SysCulture, DataString.MetaData, fTableName,
                GetBoolValue(row, "IsValue"), GetBoolValue(row, "IsPinyin"),
                GetBoolValue(row, "IsSort"), row["PinyinCaption"]);
            fMetaData = xml.ReadXmlFromFactory<IConfigCreator<IMetaData>>(MetaDataConfigFactory.REG_NAME);

            xml = string.Format(ObjectUtil.SysCulture, DataString.Source, fTableName, 
                keyCreator, GetBoolValue(row, "AutoPinyin"));
            fSource = xml.ReadXmlFromFactory<IConfigCreator<ISource>>(SourceConfigFactory.REG_NAME);

            xml = string.Format(ObjectUtil.SysCulture, DataString.PageMaker,
                row["DialogHeight"].Value<int>(200));
            fPageMaker = xml.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(PageMakerConfigFactory.REG_NAME);
        }

        #region IModule 成员

        public IMetaData CreateMetaData(IPageData pageData)
        {
            return fMetaData.CreateObject(pageData);
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            return fPageMaker.CreateObject(pageData);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return JsonPostDataSetObjectCreator.Creator;
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            return null;
        }

        public ISource CreateSource(IPageData pageData)
        {
            return fSource.CreateObject(pageData);
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return true;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public string Title { get; private set; }

        #endregion

        private static bool GetBoolValue(DataRow row, string fieldName)
        {
            return row[fieldName].Value<short>() == 1;
        }
    }
}
