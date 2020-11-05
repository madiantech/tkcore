using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class OperatorUtil
    {
        public static string ListGlobalOperator(IEnumerable<VueOperator> operators, IListMetaData metaData)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);

            StringBuilder builder = new StringBuilder();
            foreach (var oper in operators)
            {
                builder.AppendLine(GlobalOperator(oper, tableScheme));
            }
            return builder.ToString();
        }

        public static string DetailGlobalOperator(IEnumerable<VueOperator> operators, INormalMetaData metaData)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);

            StringBuilder builder = new StringBuilder();
            foreach (var oper in operators)
            {
                builder.AppendLine(GlobalOperator(oper, tableScheme));
            }
            return builder.ToString();
        }

        public static string ListRowOperator(IEnumerable<VueOperator> operators, IListMetaData metaData)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);

            StringBuilder builder = new StringBuilder();
            foreach (var oper in operators)
            {
                builder.AppendLine(RowOperator(oper, tableScheme));
            }
            return builder.ToString();
        }

        public static string AppendKey(IListMetaData metaData)
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);
            return GetKey(tableScheme, '*');
        }

        public static string AppendKey(INormalMetaData metaData, char spliter = '%')
        {
            ITableSchemeEx tableScheme = metaData.GetTableScheme(metaData.TableData.TableName);
            return GetKey(tableScheme, spliter);
        }

        private static string RowOperator(VueOperator vueOperator, ITableSchemeEx scheme)
        {
            string directive = GetDirective(vueOperator);
            List<string> list = CreateAttribute(vueOperator, scheme);

            return $"<el-button v-if=\"hasOperatorRight(scope.row, '{vueOperator.Id}')\"  class=\"button-item\" " +
                $"size=\"mini\" type=\"primary\" {directive}=\"{{{string.Join(',', list)}, parseSource:scope.row}}\">" +
                $"{vueOperator.Caption}</el-button>";
        }

        private static string GlobalOperator(VueOperator vueOperator, ITableSchemeEx scheme)
        {
            string directive = GetDirective(vueOperator);
            List<string> list = CreateAttribute(vueOperator, scheme);

            string icon = string.IsNullOrEmpty(vueOperator.IconClass) ? string.Empty : $"icon=\"{vueOperator.IconClass}\"";
            return $"<el-button {directive}=\"{{ {string.Join(',', list)} }}\" class=\"button-item\"" +
                $" type=\"success\" {icon}>{vueOperator.Caption}</el-button>";
        }

        private static List<string> CreateAttribute(VueOperator vueOperator, ITableSchemeEx scheme)
        {
            List<string> list = new List<string>();
            list.Add($"url:'{GetContent(vueOperator, scheme)}'");
            if (!string.IsNullOrEmpty(vueOperator.ConfirmData))
                list.Add($"confirm:'{vueOperator.ConfirmData}'");
            if (vueOperator.IsInInfo("Post"))
                list.Add("method:'post'");
            return list;
        }

        private static string GetDirective(VueOperator vueOperator)
        {
            string vurl;
            if (vueOperator.IsInInfo("Dialog"))
            {
                vurl = "v-tk-dialog-url";
            }
            else if (vueOperator.IsInInfo("AjaxUrl"))
            {
                vurl = "v-tk-ajax-url";
            }
            else
                vurl = "v-tk-drawer-url";
            return vurl;
        }

        private static string GetContent(VueOperator vueOperator, ITableSchemeEx scheme)
        {
            string content = string.Empty;
            if (string.IsNullOrEmpty(vueOperator.Content))
            {
                if (vueOperator.IsInInfo("Insert"))
                    content = "add";
                else if (vueOperator.IsInInfo("Update"))
                    content = "edit";
                else if (vueOperator.IsInInfo("Delete"))
                    content = "delete";
            }
            else
                content = vueOperator.Content;

            if (string.IsNullOrEmpty(content))
                return content;
            if (vueOperator.UseKey)
            {
                string keyStr = GetKey(scheme, '*');
                if (!string.IsNullOrEmpty(keyStr))
                {
                    string link = content.Contains("?") ? "&" : "?";
                    content += $"{link}{keyStr}";
                }
            }
            return content;
        }

        private static string GetKey(ITableSchemeEx scheme, char oper)
        {
            var keys = from item in scheme.Fields
                       where item.IsKey
                       select $"{item.NickName}={oper}{item.NickName}{oper}";
            string keyStr = string.Join('&', keys);
            return keyStr;
        }

        public static bool IsInInfo(this VueOperator vueOperator, string data)
        {
            return vueOperator.Info?.Contains(data) == true;
        }

        public static List<List<Tk5FieldInfoEx>> Arrange(INormalTableData table)
        {
            List<List<Tk5FieldInfoEx>> result = new List<List<Tk5FieldInfoEx>>();
            var fields = table.DataList;
            if (fields.Count() == 0)
                return result;

            int currentCol = 0;
            List<Tk5FieldInfoEx> row = new List<Tk5FieldInfoEx>();
            foreach (var item in fields)
            {
                SimpleFieldLayout layout = (SimpleFieldLayout)item.Layout;
                switch (layout.Layout)
                {
                    case FieldLayout.PerUnit:
                        if (layout.UnitNum >= table.ColumnCount)
                        {
                            goto case FieldLayout.PerLine;
                        }
                        if (layout.UnitNum + currentCol <= table.ColumnCount)
                        {
                            currentCol += layout.UnitNum;
                            row.Add(item);
                        }
                        else
                        {
                            row = NewLine(result, row);
                            row.Add(item);
                            currentCol = layout.UnitNum;
                        }
                        break;

                    case FieldLayout.PerLine:
                        if (currentCol != 0)
                        {
                            row = NewLine(result, row);
                            currentCol = 0;
                        }
                        row.Add(item);
                        row = NewLine(result, row);
                        break;
                }
            }

            return result;
        }

        private static List<Tk5FieldInfoEx> NewLine(List<List<Tk5FieldInfoEx>> result, List<Tk5FieldInfoEx> row)
        {
            result.Add(row);
            row = new List<Tk5FieldInfoEx>();
            return row;
        }

        public static string GetStyle(IPageStyle style)
        {
            if (style == null)
                return string.Empty;
            if (style.Style == PageStyle.Custom)
            {
                string operation = style.Operation;
                if (operation.EndsWith("Vue"))
                    return operation.Substring(0, operation.Length - 3).ToLower();
                else
                    return operation;
            }
            else
                return style.Style.ToString().ToLower();
        }

        private static Dictionary<string, string> GetTableInitJson(INormalTableData table)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var field in table.HiddenList)
                result.Add(field.NickName, string.Empty);
            foreach (var field in table.DataList)
                result.Add(field.NickName, string.Empty);

            return result;
        }

        public static string GetInitJson(INormalMetaData metaData)
        {
            Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
            if (metaData.Single)
                data.Add(metaData.TableData.TableName, GetTableInitJson(metaData.TableData));
            else
            {
                foreach (var table in metaData.TableDatas)
                {
                    data.Add(table.TableName, GetTableInitJson(table));
                }
            }
            return data.WriteJson();
        }
    }
}