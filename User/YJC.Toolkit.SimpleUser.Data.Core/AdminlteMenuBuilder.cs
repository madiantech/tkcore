using System.Data;
using System.Text;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class AdminlteMenuBuilder : IMenuScriptBuilder
    {
        private const string HEAD = "<li class=\"treeview\"><a href=\"#\"><i class=\"fa fa-files-o\"></i>"
            + "<span> {0}</span><i class=\"fa fa-angle-left pull-right\"></i></a><ul class=\"treeview-menu\">";
        private const string END = "</ul></li>\n";
        private const string MENU = "<li><a href=\"#\" data-menu=\"{0}\"><i class=\"fa fa-circle-o\"></i> {1}</a></li>\n";
        //private const string SUB_MENU = "<li class=\"dropdown-submenu\"><a href=\"#\" tabindex=\"-1\">" +
        //    "{0}</a><ul class=\"dropdown-menu\">";


        #region IMenuScriptBuilder 成员

        public string GetMenuScript(DataSet menuDataSet)
        {
            if (menuDataSet == null)
                return string.Empty;
            DataTable table = menuDataSet.Tables["SYS_FUNCTION"];
            if (table == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            var rootRows = BootstrapMenuBuilder.GetRows(table, -1);
            foreach (DataRow rootRow in rootRows)
            {
                builder.AppendFormat(HEAD, rootRow["FN_NAME"]);
                var childRows = BootstrapMenuBuilder.GetRows(table, rootRow["FN_ID"].Value<int>());
                foreach (DataRow menuRow in childRows)
                {
                    //var subRows = GetRows(table, menuRow["FN_ID"].Value<int>()).ToArray();
                    //if (subRows.Length == 0)
                    //{
                    string url = WebUtil.ResolveUrl(menuRow["FN_URL"].ToString());
                    builder.AppendFormat(MENU, url, menuRow["FN_NAME"]);
                    //}
                    //else
                    //{
                    //    builder.AppendFormat(SUB_MENU, menuRow["FN_NAME"]);
                    //    foreach (DataRow subMenuRow in subRows)
                    //    {
                    //        string url = WebUtil.ResolveUrl(subMenuRow["FN_URL"].ToString());
                    //        builder.AppendFormat(MENU, url, subMenuRow["FN_NAME"]);
                    //    }
                    //    builder.AppendFormat(END);
                    //}
                }
                builder.AppendFormat(END);
            }
            return builder.ToString();
        }

        #endregion
    }
}
