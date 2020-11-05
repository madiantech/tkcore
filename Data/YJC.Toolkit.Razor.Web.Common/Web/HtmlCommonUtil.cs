using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class HtmlCommonUtil
    {
        public static string AppVirtualPath
        {
            get
            {
                return BaseAppSetting.Current.AppVirtualPath;
            }
        }

        private static string TrimString(string item)
        {
            if (item == null)
                return null;
            return item.Trim();
        }

        public static string GetTitle(string format, string title)
        {
            if (string.IsNullOrEmpty(format))
                return title;
            return string.Format(ObjectUtil.SysCulture, format, title);
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

        public static string GetRangeCtrl(string startHtml, string endHtml)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(HtmlCommonExtension.SPAN_CTRL, startHtml).AppendLine();
            builder.AppendLine("<label>至</label>");
            builder.AppendFormat(HtmlCommonExtension.SPAN_CTRL, endHtml).AppendLine();

            return builder.ToString();
        }

        internal static string ResolveString(IFieldValueProvider provider, string content)
        {
            string linkUrl;
            HRefParser parser = HRefParser.ParseExpression(content);
            object[] dataArray = new object[parser.ParamArray.Count];
            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = provider[parser.ParamArray[i]].ToString();
            linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
            return linkUrl;
        }

        public static string HiddenKey(INormalTableData table, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(table, "table", null);

            var keys = from item in table.HiddenList.Union(table.DataList)
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

        public static string GetSelectedTab(IEnumerable<ListTabSheet> tabSheets)
        {
            if (tabSheets == null)
                return string.Empty;
            var result = (from item in tabSheets
                          where item.Selected
                          select item.Id).FirstOrDefault();
            return result ?? string.Empty;
        }

        public static string GetListJson(string tableName, IEnumerable<Tk5FieldInfoEx> queryFields)
        {
            JsonFieldList fieldList = new JsonFieldList(tableName, queryFields)
            {
                SearchMethod = SearchControlMethod.Id,
                JsonType = JsonObjectType.Object
            };

            return fieldList.ToJsonString();
        }

        public static ITableOutput GetTableOutput(IListTableData tableData)
        {
            TkDebug.AssertArgumentNull(tableData, nameof(tableData), null);

            if (tableData.Output == null)
                return new TableOutput();
            return tableData.Output;
        }

        public static ITableOutput GetTableOutput(INormalTableData tableData)
        {
            TkDebug.AssertArgumentNull(tableData, nameof(tableData), null);

            ITableOutput output = tableData.Output;
            if (output == null)
            {
                switch (tableData.ListStyle)
                {
                    case TableShowStyle.None:
                        output = new NormalOutput
                        {
                            ColumnCount = tableData.ColumnCount
                        };
                        break;

                    case TableShowStyle.Table:
                        output = new TableOutput()
                        {
                            IsFix = tableData.IsFix
                        };
                        break;

                    case TableShowStyle.Normal:
                        output = new TableNormalOutput()
                        {
                            IsFix = tableData.IsFix,
                            ColumnCount = tableData.ColumnCount
                        };
                        break;
                }
            }
            return output;
        }
    }
}