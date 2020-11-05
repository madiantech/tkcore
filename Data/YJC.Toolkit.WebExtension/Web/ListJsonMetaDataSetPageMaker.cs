using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Web
{
    internal class ListJsonMetaDataSetPageMaker : JsonMetaDataSetPageMaker
    {
        public ListJsonMetaDataSetPageMaker()
        {
            //QuoteChar = '\'';
        }

        protected override void ProcessResult(DataSet resultSet)
        {
            IListMetaData list = MetaData.Convert<IListMetaData>();
            DataTable table = resultSet.Tables[list.TableData.TableName];
            if (table == null || table.Rows.Count == 0)
                return;

            var opers = OperatorUtil.CreateOperators(resultSet, "RowOperator");
            if (opers != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRowFieldValueProvider provider = new DataRowFieldValueProvider(row, resultSet);

                    var operRights = row["_OPERATOR_RIGHT"].Value<ObjectOperatorCollection>();
                    if (provider.Operators == null || provider.Operators.Count == 0)
                        continue;

                    HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<ul class=\"flexRow flexCenter\">");
                    foreach (var oper in opers)
                    {
                        if (!operRights.Contains(oper.Id))
                            continue;
                        builder.Append(OperatorUtil.CreateRowOperator(provider,
                            attrBuilder, oper));
                    }
                    builder.Append("</ul>");
                    row["_OPERATOR_RIGHT"] = builder.ToString();
                }
            }

            var listOpers = OperatorUtil.CreateOperators(resultSet, "ListOperator");
            if (listOpers != null)
            {
                DataRowFieldValueProvider provider = new DataRowFieldValueProvider(null, resultSet);

                HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
                StringBuilder builder = new StringBuilder();
                builder.Append("<ul class=\"flexRow flexRight\">");
                foreach (var oper in listOpers)
                {
                    builder.Append(OperatorUtil.CreateRowOperator(provider,
                        attrBuilder, oper));
                }
                builder.Append("</ul>");

                DataTable listOperTable = DataSetUtil.CreateDataTable("ListOperator", "Content");
                DataRow row = listOperTable.NewRow();
                row["Content"] = builder.ToString();
                listOperTable.Rows.Add(row);

                resultSet.Tables.Remove("ListOperator");
                resultSet.Tables.Add(listOperTable);
            }
        }

        public override IContent WritePage(ISource source, IPageData pageData,
            OutputData outputData)
        {
            return base.WritePage(source, pageData, outputData);
        }
    }
}