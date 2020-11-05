using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class InternalUriUtil
    {
        private static readonly string URL_TABLE_NAME = "URL";
        private static readonly string INFO_TABLE_NAME = "Info";

        private static readonly string[] URL_ARRAY =
            new string[] { "DRetURL", "DSelfURL", "RetURL", "SelfURL" };

        private static readonly string[] INFO_ARRAY =
            new string[] { "UserID", "RoleID", "Source", "Module", "IsHttpPost", "Guid", "PageX", "SessionId" };

        private static void ParseQueryString(StringBuilder builder, string s, bool urlEncoded,
            Encoding encoding, Func<string, string, bool> paramFunc)
        {
            int length = (s != null) ? s.Length : 0;
            if (length == 0)
                return;
            int start = s[0] == '?' ? 1 : 0;
            for (int i = start; i < length; i++)
            {
                int startIndex = i;
                int equalIndex = -1;
                while (i < length)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (equalIndex < 0)
                            equalIndex = i;
                    }
                    else if (ch == '&')
                        break;
                    i++;
                }
                string name = null;
                string value = null;
                if (equalIndex >= 0)
                {
                    name = s.Substring(startIndex, equalIndex - startIndex);
                    value = s.Substring(equalIndex + 1, (i - equalIndex) - 1);
                }
                else
                    value = s.Substring(startIndex, i - startIndex);
                if (string.IsNullOrEmpty(name))
                    continue;
                if (urlEncoded)
                {
                    if (paramFunc != null)
                    {
                        bool result = paramFunc(HttpUtility.UrlDecode(name, encoding),
                            HttpUtility.UrlDecode(value, encoding));
                        if (result)
                            builder.AppendFormat(ObjectUtil.SysCulture, "{0}={1}&", name, value);
                    }
                }
                else
                {
                    if (paramFunc != null)
                    {
                        bool result = paramFunc(name, value);
                        if (result)
                            builder.AppendFormat(ObjectUtil.SysCulture, "{0}={1}&", name, value);
                    }
                }
                if ((i == (length - 1)) && (s[i] == '&'))
                {
                    //base.Add(null, string.Empty);
                }
            }
        }

        public static Uri ProcessUri(Uri url, Func<string, string, bool> paramFunc, string addition)
        {
            UriBuilder builder = new UriBuilder(url);
            if (paramFunc != null)
            {
                StringBuilder queryString = new StringBuilder();
                ParseQueryString(queryString, builder.Query, true, Encoding.UTF8, paramFunc);
                if (!string.IsNullOrEmpty(addition))
                    queryString.Append(addition).Append("&");
                builder.Query = queryString.ToString();
            }
            return builder.Uri;
        }

        private static void AddUrlToDataSet(DataSet data, Uri retUrl, Uri selfUrl)
        {
            AddUrlToDataSet(data, retUrl, selfUrl, selfUrl);
        }

        private static void AddUrlToDataSet(DataSet data, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl)
        {
            DataTable urlTable = DataSetUtil.CreateDataTable(URL_TABLE_NAME, URL_ARRAY);

            DataTableCollection tables = data.Tables;
            if (!tables.Contains(urlTable.TableName))
            {
                DataRow row = urlTable.NewRow();
                string returl = GetUrlValue(retUrl);
                string selfurl = GetUrlValue(selfUrl);
                string encodeUrl = GetUrlValue(encodeSelfUrl);
                DataSetUtil.SetRowValues(row, URL_ARRAY, returl, selfurl,
                    HttpUtility.UrlEncode(returl, Encoding.UTF8),
                    HttpUtility.UrlEncode(encodeUrl, Encoding.UTF8));
                urlTable.Rows.Add(row);
                tables.Add(urlTable);
            }
        }

        private static void AddInfoToDataSet(DataSet data, IUserInfo info, string source,
            bool isModule, Guid guid, string tkx, string sessionId)
        {
            DataTable infoTable = DataSetUtil.CreateDataTable(INFO_TABLE_NAME, INFO_ARRAY);
            DataTableCollection tables = data.Tables;
            if (!tables.Contains(infoTable.TableName))
            {
                DataRow row = infoTable.NewRow();
                DataSetUtil.SetRowValues(row, INFO_ARRAY, info.UserId, info.MainOrgId,
                    source, isModule ? 1 : 0, 0, guid, tkx, sessionId);
                infoTable.Rows.Add(row);
                tables.Add(infoTable);
            }
        }

        private static void AddQueryStringToDataSet(DataSet data, NameValueCollection queryString)
        {
            DataTable table = new DataTable("QueryString") { Locale = ObjectUtil.SysCulture };
            foreach (string columnName in queryString.Keys)
            {
                if (string.IsNullOrEmpty(columnName))
                    continue;
                table.Columns.Add(columnName, typeof(string));
            }
            DataRow row = table.NewRow();
            row.BeginEdit();
            try
            {
                foreach (DataColumn column in table.Columns)
                    row[column] = queryString[column.ColumnName];
            }
            finally
            {
                row.EndEdit();
            }
            table.Rows.Add(row);

            data.Tables.Add(table);
        }

        private static string GetUrlValue(Uri url)
        {
            return url == null ? string.Empty : url.ToString();
        }

        private static XElement CreateUrlXElement(Uri retUrl, Uri selfUrl, Uri encodeSelfUrl)
        {
            string returl = GetUrlValue(retUrl);
            string selfurl = GetUrlValue(selfUrl);
            string encodeUrl = GetUrlValue(encodeSelfUrl);
            XElement urlNode = new XElement(URL_TABLE_NAME, new XElement("RetURL", returl),
                new XElement("SelfURL", selfurl),
                new XElement("DRetURL", HttpUtility.UrlEncode(returl, Encoding.UTF8)),
                new XElement("DSelfURL", HttpUtility.UrlEncode(encodeUrl, Encoding.UTF8)));
            return urlNode;
        }

        //private static void AddUrlToStringBuilder(StringBuilder builder, Uri retUrl, Uri selfUrl)
        //{
        //    XElement urlNode = CreateUrlXElement(retUrl, selfUrl, selfUrl);
        //    builder.Append(urlNode.ToString());
        //}

        private static void AddUrlToStringBuilder(StringBuilder builder, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl)
        {
            XElement urlNode = CreateUrlXElement(retUrl, selfUrl, encodeSelfUrl);
            builder.Append(urlNode.ToString());
        }

        //private static void AddUrlToXElement(XElement element, Uri retUrl, Uri selfUrl)
        //{
        //    XElement urlNode = CreateUrlXElement(retUrl, selfUrl, selfUrl);
        //    element.Add(urlNode);
        //}

        private static void AddUrlToXElement(XElement element, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl)
        {
            XElement urlNode = CreateUrlXElement(retUrl, selfUrl, encodeSelfUrl);
            element.Add(urlNode);
        }

        private static XElement CreateInfoXElement(IUserInfo info, string source, bool isModule,
            ref Guid guid, string tkx, string sessionId)
        {
            XElement infoNode = new XElement(INFO_TABLE_NAME, new XElement("UserID", info.UserId),
                new XElement("RoleID", info.MainOrgId),
                new XElement("Source", source),
                new XElement("Module", isModule ? 1 : 0),
                new XElement("IsHttpPost", 0),
                new XElement("Guid", guid.ToString()),
                new XElement("PageX", tkx),
                new XElement("SessionId", sessionId));
            return infoNode;
        }

        private static void AddInfoToStringBuilder(StringBuilder builder, IUserInfo info,
            string source, bool isModule, Guid guid, string tkx, string sessionId)
        {
            XElement infoNode = CreateInfoXElement(info, source, isModule, ref guid, tkx, sessionId);
            builder.Append(infoNode.ToString());
        }

        private static void AddInfoToXElement(XElement element, IUserInfo info,
            string source, bool isModule, Guid guid, string tkx, string sessionId)
        {
            XElement infoNode = CreateInfoXElement(info, source, isModule, ref guid, tkx, sessionId);
            element.Add(infoNode);
        }

        private static XElement CreateQueryStringXElement(NameValueCollection queryString)
        {
            XElement queryNode = new XElement("QueryString");
            foreach (string columnName in queryString.Keys)
            {
                if (string.IsNullOrEmpty(columnName))
                    continue;
                XElement child = new XElement(columnName, queryString[columnName]);
                queryNode.Add(child);
            }
            return queryNode;
        }

        private static void AddQueryStringToStringBuilder(StringBuilder builder, NameValueCollection queryString)
        {
            XElement queryNode = CreateQueryStringXElement(queryString);
            builder.Append(queryNode.ToString());
        }

        private static void AddQueryStringToXElement(XElement element, NameValueCollection queryString)
        {
            XElement queryNode = CreateQueryStringXElement(queryString);
            element.Add(queryNode);
        }

        public static Uri GetSelfUrl(Uri baseUrl)
        {
            Uri selfUrl = InternalUriUtil.ProcessUri(baseUrl,
                (name, value) => name.ToLower(ObjectUtil.SysCulture) != "returl", null);
            return selfUrl;
        }

        //public static void AddToDataSet(DataSet data, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl)
        //{
        //    AddToDataSet(data, queryString, sessionGbl, retUrl, selfUrl, true, InternalUriUtil.GetUrlExtension(selfUrl));
        //}

        //public static void AddToDataSet(DataSet data, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, bool isModule, string pageExtension)
        //{
        //    AddToDataSet(data, queryString, sessionGbl, retUrl, selfUrl, selfUrl, isModule, pageExtension);
        //}

        //public static void AddToDataSet(DataSet data, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl, bool isModule,
        //    string pageExtension)
        //{
        //    AddUrlToDataSet(data, retUrl, selfUrl, encodeSelfUrl);
        //    AddInfoToDataSet(data, sessionGbl.Info, queryString["Source"], isModule,
        //        sessionGbl.TempIndentity, pageExtension, sessionGbl.SessionId);
        //    AddQueryStringToDataSet(data, queryString);
        //}

        //public static void AddToStringBuilder(StringBuilder builder, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl)
        //{
        //    AddToStringBuilder(builder, queryString, sessionGbl, retUrl, selfUrl, true, InternalUriUtil.GetUrlExtension(selfUrl));
        //}

        //public static void AddToStringBuilder(StringBuilder builder, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, bool isModule, string pageExtension)
        //{
        //    AddToStringBuilder(builder, queryString, sessionGbl, retUrl, selfUrl, selfUrl, isModule, pageExtension);
        //}

        //public static void AddToStringBuilder(StringBuilder builder, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl, bool isModule, string pageExtension)
        //{
        //    AddUrlToStringBuilder(builder, retUrl, selfUrl, encodeSelfUrl);
        //    AddInfoToStringBuilder(builder, sessionGbl.Info, queryString["Source"], isModule,
        //        sessionGbl.TempIndentity, pageExtension, sessionGbl.SessionId);
        //    AddQueryStringToStringBuilder(builder, queryString);
        //}

        //public static void AddToXElement(XElement element, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl)
        //{
        //    AddToXElement(element, queryString, sessionGbl, retUrl, selfUrl, true, InternalUriUtil.GetUrlExtension(selfUrl));
        //}

        //public static void AddToXElement(XElement element, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, bool isModule, string pageExtension)
        //{
        //    AddToXElement(element, queryString, sessionGbl, retUrl, selfUrl, selfUrl, isModule, pageExtension);
        //}

        //public static void AddToXElement(XElement element, NameValueCollection queryString,
        //    SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl, Uri encodeSelfUrl, bool isModule, string pageExtension)
        //{
        //    AddUrlToXElement(element, retUrl, selfUrl, encodeSelfUrl);
        //    AddInfoToXElement(element, sessionGbl.Info, queryString["Source"], isModule,
        //        sessionGbl.TempIndentity, pageExtension, sessionGbl.SessionId);
        //    AddQueryStringToXElement(element, queryString);
        //}
    }
}