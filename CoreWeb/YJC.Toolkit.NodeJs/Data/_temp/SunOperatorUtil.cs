using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class SunOperatorUtil
    {
        private static string GetDirective(VueOperator vueOperator)
        {
            string vurl;
            if (vueOperator.IsInInfo("Dialog"))
            {
                vurl = "v-tk-dialog";
            }
            else if (vueOperator.IsInInfo("AjaxUrl"))
            {
                vurl = "v-tk-ajax-url";
            }
            else
                vurl = "v-tk-drawer";
            return vurl;
        }

        private static string GlobalOperator(VueOperator vueOperator, ITableSchemeEx scheme, string source)
        {
            string directive = GetDirective(vueOperator);
            List<string> list = CreateAttribute(vueOperator, scheme, source);

            string icon = string.IsNullOrEmpty(vueOperator.IconClass) ? string.Empty : $"icon=\"{vueOperator.IconClass}\"";
            return $"<el-button {directive}=\"{{ {string.Join(',', list)} }}\" class=\"filter-item\"" +
                $" size=\"medium\" type=\"primary\" {icon}>{vueOperator.Caption}</el-button>";
        }

        private static List<string> CreateAttribute(VueOperator vueOperator,
            ITableSchemeEx scheme, string source)
        {
            List<string> list = new List<string>();
            list.Add($"url:`{GetContent(vueOperator, scheme, source)}`");
            if (!string.IsNullOrEmpty(vueOperator.ConfirmData))
                list.Add($"confirm:'{vueOperator.ConfirmData}'");
            if (vueOperator.IsInInfo("Post"))
                list.Add("method:'post'");
            return list;
        }

        private static string GetContent(VueOperator vueOperator, ITableSchemeEx scheme, string source)
        {
            string content = string.Empty;
            if (string.IsNullOrEmpty(vueOperator.Content))
            {
                if (vueOperator.IsInInfo("Insert"))
                    content = "/add";
                else if (vueOperator.IsInInfo("Update"))
                    content = "/edit";
                else if (vueOperator.IsInInfo("Delete"))
                    content = $"/c/xml/delete/{source}";
            }
            else
                content = vueOperator.Content;

            if (string.IsNullOrEmpty(content))
                return content;
            if (vueOperator.UseKey)
            {
                string keyStr = GetKey(scheme);
                if (!string.IsNullOrEmpty(keyStr))
                {
                    string link = content.Contains("?") ? "&" : "?";
                    content += $"{link}{keyStr}";
                }
            }
            return content;
        }

        private static string GetKey(ITableSchemeEx scheme)
        {
            var keys = from item in scheme.Fields
                       where item.IsKey
                       select $"{item.NickName}=${{row.{item.NickName}}}";
            string keyStr = string.Join('&', keys);
            return keyStr;
        }

        private static string RowOperator(VueOperator vueOperator, ITableSchemeEx scheme, string source)
        {
            string directive = GetDirective(vueOperator);
            List<string> list = CreateAttribute(vueOperator, scheme, source);

            return $"<el-button v-if=\"buttonShow('{vueOperator.Id}', row._OPERATOR_RIGHT)\" " +
                $"size=\"small\" type=\"text\" {directive}=\"{{{string.Join(',', list)}}}\">" +
                $"{vueOperator.Caption}</el-button>";
        }

        public static string ListGlobalOperator(IEnumerable<VueOperator> operators,
            IListMetaData metaData, string source)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);

            StringBuilder builder = new StringBuilder();
            foreach (var oper in operators)
            {
                builder.AppendLine(GlobalOperator(oper, tableScheme, source));
            }
            return builder.ToString();
        }

        public static string ListRowOperator(IEnumerable<VueOperator> operators,
            IListMetaData metaData, string source)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);

            StringBuilder builder = new StringBuilder();
            foreach (var oper in operators)
            {
                builder.AppendLine(RowOperator(oper, tableScheme, source));
            }
            return builder.ToString();
        }

        public static string AppendKey(IListMetaData metaData)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);
            return GetKey(tableScheme);
        }
    }
}