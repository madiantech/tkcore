using System.Data;
using System.Text.RegularExpressions;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ParamExpression(REG_NAME, SqlInject = true, Author = "YJC",
        CreateDate = "2009-04-13", Description = "查询数据表中的字段值($)，" +
        "格式为TableName.FieldName[#EasySearchRegName][.(number | n [ - number])]")]
    internal sealed class DataTableParamExpression : IParamExpression, ICustomData
    {
        internal const string REG_NAME = "$";
        internal const string REG_STR = @"^(?<Table>\w+)\.(?<Column>\w+)(\#(?<RegName>\w+))?"
            + @"(\.((?<First>\d+) | (?<Last>n(\s*-\s*(?<Count>\d+))?)))?$";
        private readonly static Regex fExpr = new Regex(REG_STR,
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        private DataSet fDataSet;
        private TkDbContext fContext;

        #region INeedCustomData 成员

        void ICustomData.SetData(params object[] args)
        {
            IDbDataSource data = ObjectUtil.QueryObject<IDbDataSource>(args);
            if (data != null)
            {
                fDataSet = data.DataSet;
                fContext = data.Context;
            }
            if (fDataSet == null)
                fDataSet = ObjectUtil.QueryObject<DataSet>(args);
            if (fContext == null)
                fContext = ObjectUtil.QueryObject<TkDbContext>(args);
            TkDebug.AssertNotNull(fDataSet,
                "参数宏($)需要DataSet对象，但是没有从外部对象中找到", this);
        }

        #endregion

        #region IParamExpression 成员

        string IParamExpression.Execute(string parameter)
        {
            Match match = fExpr.Match(parameter);
            TkDebug.AssertNotNull(match, string.Format(ObjectUtil.SysCulture,
                "参数{0}格式不正确，正确格式为TableName.FieldName[.(number | n [ - number])]" +
                "[#EasySearchRegName]，请检查", parameter), this);
            string tableName = match.Groups["Table"].Value;
            string fieldName = match.Groups["Column"].Value;
            string regName = match.Groups["RegName"].Value;
            int count = 0;
            bool isLast = true;
            string first = match.Groups["First"].Value;
            if (!string.IsNullOrEmpty(first))
            {
                isLast = false;
                count = int.Parse(first, ObjectUtil.SysCulture);
            }
            else
            {
                string lastCount = match.Groups["Count"].Value;
                if (!string.IsNullOrEmpty(lastCount))
                    try
                    {
                        count = int.Parse(lastCount, ObjectUtil.SysCulture);
                    }
                    catch
                    {
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "[n - number]格式中number必须是数字，但是现在不是，它的值是{0}", lastCount), this);
                    }
            }
            DataTable table = fDataSet.Tables[tableName];
            TkDebug.AssertNotNull(table, string.Format(ObjectUtil.SysCulture,
                "宏{0}中定义的表{1}在DataSet中不存在", parameter, tableName), this);
            TkDebug.Assert(table.Columns.Contains(fieldName), string.Format(ObjectUtil.SysCulture,
                "宏{0}中定义的字段{1}在表中不存在", parameter, fieldName), this);
            int rowCount = table.Rows.Count;
            int rowNumber = isLast ? rowCount - 1 - count : count;
            TkDebug.Assert(rowNumber >= 0 && rowNumber < rowCount, string.Format(ObjectUtil.SysCulture,
                "行号定义错误，当前表中有{0}行记录，而现在行号为{1}", rowCount, rowNumber), this);
            string value = table.Rows[rowNumber][fieldName].ToString();
            if (!string.IsNullOrEmpty(regName))
            {
                //EasySearch easySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                //    "EasySearch", regName);
                //TkDebug.AssertNotNull(fContext,
                //    "参数宏($)需要Context对象，但是没有从外部对象中找到", this);
                //value = easySearch.Decode(fContext, value);
            }
            return value;
        }

        #endregion
    }
}
