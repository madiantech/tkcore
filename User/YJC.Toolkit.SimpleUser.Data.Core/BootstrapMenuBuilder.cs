using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class BootstrapMenuBuilder : IMenuScriptBuilder
    {
        private const string HEAD = "<li class=\"dropdown\"><a href=\"#\" class=\"dropdown-toggle\" " +
            "data-toggle=\"dropdown\">{0} <span class=\"caret\"></span></a><ul class=\"dropdown-menu\" role=\"menu\">";
        private const string END = "</ul></li>";
        private const string MENU = "<li><a href=\"#\" data-menu=\"{0}\"><i class=\"fa fa-circle-o\"></i> {1}</a></li>";
        private const string SUB_MENU = "<li class=\"dropdown-submenu\"><a href=\"#\" tabindex=\"-1\">" +
            "{0}</a><ul class=\"dropdown-menu\">";

        #region IMenuScriptBuilder 成员

        public string GetMenuScript(DataSet menuDataSet)
        {
            if (menuDataSet == null)
                return string.Empty;
            DataTable table = menuDataSet.Tables["SYS_FUNCTION"];
            if (table == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            var rootRows = GetRows(table, -1);
            foreach (DataRow rootRow in rootRows)
            {
                builder.AppendFormat(HEAD, rootRow["FN_NAME"]);
                var childRows = GetRows(table, rootRow["FN_ID"].Value<int>());
                foreach (DataRow menuRow in childRows)
                {
                    var subRows = GetRows(table, menuRow["FN_ID"].Value<int>()).ToArray();
                    if (subRows.Length == 0)
                    {
                        string url = WebUtil.ResolveUrl(menuRow["FN_URL"].ToString());
                        builder.AppendFormat(MENU, url, menuRow["FN_NAME"]);
                    }
                    else
                    {
                        builder.AppendFormat(SUB_MENU, menuRow["FN_NAME"]);
                        foreach (DataRow subMenuRow in subRows)
                        {
                            string url = WebUtil.ResolveUrl(subMenuRow["FN_URL"].ToString());
                            builder.AppendFormat(MENU, url, subMenuRow["FN_NAME"]);
                        }
                        builder.AppendFormat(END);
                    }
                }
                builder.AppendFormat(END);
            }
            return builder.ToString();
        }

        #endregion

        internal static IEnumerable<DataRow> GetRows(DataTable funcTable, int parentId)
        {
            var result = from row in funcTable.AsEnumerable()
                         where row.Field<int>("FN_PARENT_ID") == parentId
                         select row;
            return result;
        }
    }
}
