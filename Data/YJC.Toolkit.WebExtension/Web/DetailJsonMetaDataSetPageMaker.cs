using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class DetailJsonMetaDataSetPageMaker : JsonMetaDataSetPageMaker
    {
        internal class DetailOper
        {
            [SimpleAttribute]
            public string Content { get; set; }
        }

        public DetailJsonMetaDataSetPageMaker()
        {
            //QuoteChar = '\'';
        }

        protected override void ProcessResult(DataSet resultSet)
        {
            INormalMetaData list = MetaData.Convert<INormalMetaData>();
            DataTable table = resultSet.Tables[list.TableData.TableName];
            if (table == null || table.Rows.Count == 0)
                return;
            DataRow row = table.Rows[0];

            var opers = OperatorUtil.CreateOperators(resultSet, "DetailOperator");
            if (opers == null)
                return;

            DataRowFieldValueProvider provider = new DataRowFieldValueProvider(row, resultSet);
            HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
            StringBuilder builder = new StringBuilder();
            foreach (var oper in opers)
                builder.Append(OperatorUtil.CreateRowOperator(provider,
                    attrBuilder, oper));

            DetailOper detailOper = new DetailOper { Content = builder.ToString() };
            DataTable operTable = EnumUtil.Convert(detailOper).CreateTable("DetailOperator");
            resultSet.Tables.Remove("DetailOperator");
            resultSet.Tables.Add(operTable);
        }
    }
}