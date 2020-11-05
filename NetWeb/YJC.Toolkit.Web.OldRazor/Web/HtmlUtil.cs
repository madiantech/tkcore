using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static partial class HtmlUtil
    {
        private const string DOWNLOAD_URL = "source/C/DownloadAttachment.c?NoFileName={2}&Cache={1}&Id={0}";

        public static string AppVirtualPath
        {
            get
            {
                return BaseAppSetting.Current.AppVirtualPath;
            }
        }

        public static string GetTitle(string format, string title)
        {
            if (string.IsNullOrEmpty(format))
                return title;
            return string.Format(ObjectUtil.SysCulture, format, title);
        }

        public static string GetEditTitle(DataSet dataSet, dynamic viewBag)
        {
            string id = dataSet.GetFieldValue<string>("Info", "Style");
            string title = viewBag.Title;
            BootcssEditData pageData = viewBag.PageData;
            switch (id)
            {
                case "Insert":
                    return GetTitle(pageData.InsertFormat, title);

                case "Update":
                    return GetTitle(pageData.EditFormat, title);

                default:
                    return string.Empty;
            }
        }

        public static string GetDynamicEditTitle(dynamic model, dynamic viewBag)
        {
            try
            {
                PageInfo page = model.Info;
                string title = viewBag.Title;
                BootcssEditData pageData = viewBag.PageData;
                switch (page.Style.Style)
                {
                    case PageStyle.Insert:
                        return GetTitle(pageData.InsertFormat, title);

                    case PageStyle.Update:
                        return GetTitle(pageData.EditFormat, title);

                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetPageSource(DataSet dataSet)
        {
            string source = dataSet.GetFieldValue<string>("Info", "Source");
            return source;
        }

        public static string GetPageId(DataSet dataSet)
        {
            try
            {
                string id = dataSet.GetFieldValue<string>("Info", "Style");
                return string.Format(ObjectUtil.SysCulture, "Web{0}XmlPage", id);
            }
            catch
            {
                return "WebPage";
            }
        }

        public static string GetSelectedTab(DataSet dataSet)
        {
            try
            {
                DataTable table = dataSet.Tables["TabSheet"];
                if (table == null)
                    return string.Empty;

                foreach (DataRow row in table.Rows)
                {
                    if (row["Selected"].Value<bool>())
                        return row["Id"].ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetDownloadUrl(string id, bool useCache, bool noFileName)
        {
            return string.Format(ObjectUtil.SysCulture, DOWNLOAD_URL,
                id, useCache, noFileName).AppVirutalPath();
        }

        public static string GetDownloadUrl(string id, bool useCache, bool noFileName, string contextName)
        {
            string url = string.Format(ObjectUtil.SysCulture, DOWNLOAD_URL,
                id, useCache, noFileName);
            if (!string.IsNullOrEmpty(contextName))
                url += "&Context=" + contextName;
            return url.AppVirutalPath();
        }

        public static string MergeClass(params string[] args)
        {
            if (args == null)
                return string.Empty;

            var datas = from item in args
                        let trimItem = TrimString(item)
                        where !string.IsNullOrEmpty(trimItem)
                        select trimItem;
            return string.Join(" ", datas);
        }

        private static string TrimString(string item)
        {
            if (item == null)
                return null;
            return item.Trim();
        }

        public static string DisplayField(DataRow row, IFieldInfo field)
        {
            try
            {
                return row[field.NickName].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static DataRow GetMainRow(DataSet model, dynamic viewBag)
        {
            IMetaData metaData = viewBag.MetaData;

            if (metaData is Tk5SingleNormalMetaData)
            {
                string tableName = (metaData.Convert<Tk5SingleNormalMetaData>()).Table.TableName;
                return model.GetRow(tableName);
            }
            else if (metaData is Tk5MultipleMetaData)
            {
                string tableName = (metaData.Convert<Tk5MultipleMetaData>()).Tables[0].TableName;
                return model.GetRow(tableName);
            }

            return null;
        }

        //public static object GetMainObject(object model, dynamic viewBag)
        //{
        //    IMetaData metaData = viewBag.MetaData;

        //    if (metaData is Tk5SingleNormalMetaData)
        //    {
        //        string tableName = (metaData.Convert<Tk5SingleNormalMetaData>()).Table.TableName;
        //        return model.MemberValue(tableName);
        //    }

        //    return null;
        //}

        internal static string ResolveObjectString(object receiver, string content)
        {
            string linkUrl;
            HRefParser parser = HRefParser.ParseExpression(content);
            object[] dataArray = new object[parser.ParamArray.Count];
            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = receiver.MemberValue(parser.ParamArray[i]).ConvertToString();
            linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
            return linkUrl;
        }

        internal static string ResolveString(DataRow row, string content)
        {
            string linkUrl;
            HRefParser parser = HRefParser.ParseExpression(content);
            object[] dataArray = new object[parser.ParamArray.Count];
            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = row.GetString(parser.ParamArray[i]);
            linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
            return linkUrl;
        }

        public static string ParseLinkUrl(DataRow row, string content)
        {
            string linkUrl = ResolveString(row, content);
            return WebUtil.ResolveUrl(linkUrl);
        }

        //public static string ParseLinkUrl(object receiver, string content)
        //{
        //    string linkUrl = ResolveObjectString(receiver, content);
        //    return WebUtil.ResolveUrl(linkUrl);
        //}

        public static string GetRetUrl(DataSet dataSet)
        {
            string homePage = WebAppSetting.WebCurrent.HomePath;
            if (dataSet == null)
                return homePage;

            string retUrl = dataSet.GetFieldValue<string>("URL", "DRetURL");
            if (string.IsNullOrEmpty(retUrl))
                return homePage;
            return WebUtil.ResolveUrl(retUrl);
        }

        public static string GetSelfUrl(DataSet dataSet)
        {
            string retUrl = dataSet.GetFieldValue<string>("URL", "DSelfURL");
            return WebUtil.ResolveUrl(retUrl);
        }

        public static string HiddenKey(Tk5NormalTableData table, DataRow row, bool needId)
        {
            TkDebug.AssertArgumentNull(table, "table", null);

            var keys = from item in table.HiddenList.Union(table.TableList)
                       where item.IsKey
                       select item;
            StringBuilder builder = new StringBuilder();
            HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
            foreach (Tk5FieldInfoEx item in keys)
            {
                attrBuilder.Clear();
                string value = row.GetString(item.NickName);
                attrBuilder.Add("value", value);
                attrBuilder.Add("type", "hidden");
                string name = "OLD_" + item.NickName;
                if (needId)
                    attrBuilder.Add("id", name);
                attrBuilder.Add("name", name);

                builder.AppendFormat("<input {0}/>", attrBuilder.CreateAttribute());
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static string HiddenKey(Tk5NormalTableData table, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(table, "table", null);

            var keys = from item in table.HiddenList.Union(table.TableList)
                       where item.IsKey
                       select item;
            StringBuilder builder = new StringBuilder();
            HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
            foreach (Tk5FieldInfoEx item in keys)
            {
                attrBuilder.Clear();
                string value = provider == null ? string.Empty : provider[item.NickName].ToString();
                attrBuilder.Add("value", value);
                attrBuilder.Add("type", "hidden");
                string name = "OLD_" + item.NickName;
                if (needId)
                    attrBuilder.Add("id", name);
                attrBuilder.Add("name", name);

                builder.AppendFormat("<input {0}/>", attrBuilder.CreateAttribute());
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}