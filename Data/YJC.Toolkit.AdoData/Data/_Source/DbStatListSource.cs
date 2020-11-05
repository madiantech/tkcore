using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DbStatListSource : DbListSource
    {
        private List<StatFieldConfigItem> fStatFields;

        protected DbStatListSource(StatConfigItem stat)
        {
            InitStat(stat);
        }

        public DbStatListSource(IStatListDbConfig config)
            : base(config)
        {
            InitStat(config.Stat);
        }

        public StatCalcMode CalcMode { get; set; }

        public bool UseSubTotal { get; set; }

        private void InitStat(StatConfigItem stat)
        {
            if (stat != null)
            {
                CalcMode = stat.CalcMode;
                UseSubTotal = stat.UseSubTotal;
                fStatFields = stat.StatFieldList;
            }
            else
            {
                CalcMode = StatCalcMode.None;
                UseSubTotal = false;
                fStatFields = new List<StatFieldConfigItem>();
            }
        }

        private bool IsCalTotalData(FilledListEventArgs e)
        {
            switch (CalcMode)
            {
                case StatCalcMode.First:
                    if (e.PageNumber == 0)
                        return true;
                    break;

                case StatCalcMode.Last:
                    if ((e.PageSize * (e.PageNumber + 1)) >= e.Count)
                        return true;
                    break;

                case StatCalcMode.PerPage:
                    return true;
            }
            return false;
        }

        private void CalSubTotalData(FilledListEventArgs e)
        {
            if (fStatFields.Count == 0)
                return;
            DataTable mainTableData = DataSet.Tables[FillTableName];

            DataTable subTotalTable = new DataTable("_SubTotal");
            var columns = subTotalTable.Columns;
            foreach (var field in fStatFields)
                columns.Add(field.NickName, typeof(double));

            var row = subTotalTable.NewRow();
            foreach (StatFieldConfigItem field in fStatFields)
                row[field.NickName] = CalSubTotalNum(mainTableData, field.NickName, field.Method);
            subTotalTable.Rows.Add(row);
            DataSet.Tables.Add(subTotalTable);
        }

        private double CalSubTotalNum(DataTable mainTableData, String nickName, StatMethod method)
        {
            double res = 0;
            if (method == StatMethod.Count)
                return mainTableData.Rows.Count;
            foreach (DataRow rowData in mainTableData.Rows)
            {
                var rawValue = rowData[nickName];

                if (rawValue == DBNull.Value)
                    continue;
                double value = rowData[nickName].Value<double>();
                if (res == 0)
                {
                    res = value;
                    continue;
                }

                switch (method)
                {
                    case StatMethod.Sum:
                        res += value;
                        break;

                    case StatMethod.Avg:
                        res += value;
                        break;

                    case StatMethod.Max:
                        if (res < value)
                            res = value;
                        break;

                    case StatMethod.Min:
                        if (res > value)
                            res = value;
                        break;

                    case StatMethod.Count:
                        TkDebug.ThrowImpossibleCode(this);
                        break;

                    default:
                        break;
                }
            }
            if (res != 0 && method == StatMethod.Avg)
                res = res / mainTableData.Rows.Count;
            return res;
        }

        private string CreateStatSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            bool appendBefore = false;
            bool subAfter = false;
            for (int i = 0; i < fStatFields.Count; ++i)
            {
                StatFieldConfigItem field = fStatFields[i];
                string nickName = field.NickName;
                string fieldName = Context.EscapeName(MainResolver.GetFieldInfo(nickName).FieldName);

                if (appendBefore)
                {
                    sql.Append(",");
                }
                switch (field.Method)
                {
                    case StatMethod.Count:
                        appendBefore = false;
                        subAfter = true;
                        break;

                    default:
                        sql.AppendFormat("{0}({1}) {2}", field.Method, fieldName, nickName);
                        appendBefore = true;
                        subAfter = false;
                        break;
                }
            }
            if (subAfter)
                sql = sql.Remove(sql.Length - 1, 1);
            sql.Append(" FROM ").Append(GetTableName(Context));
            return sql.ToString();
        }

        private void AppendCountPart(DataTable table, int count)
        {
            foreach (var field in fStatFields)
            {
                if (field.Method == StatMethod.Count)
                {
                    table.Columns.Add(field.NickName);
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                            row[field.NickName] = count;
                    }
                    else
                    {
                        DataRow row = table.NewRow();
                        row[field.NickName] = count;
                        table.Rows.Add(row);
                    }
                }
            }
        }

        private DataTable CalTotalData(FilledListEventArgs e)
        {
            if (fStatFields.All(item => item.Method == StatMethod.Count))
                DataSet.Tables.Add(new DataTable("_Total"));
            else
                SqlSelector.Select(Context, DataSet, "_Total", CreateStatSql(), e.Condition);
            DataTable table = DataSet.Tables["_Total"];
            AppendCountPart(table, e.Count);
            return table;
        }

        protected override void OnFilledListTables(FilledListEventArgs e)
        {
            base.OnFilledListTables(e);

            if (e.Count > 0)
            {
                if (IsCalTotalData(e))
                    CalTotalData(e);
                if (UseSubTotal)
                    CalSubTotalData(e);
            }
        }
    }
}