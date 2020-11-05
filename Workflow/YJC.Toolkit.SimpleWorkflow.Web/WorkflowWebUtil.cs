using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    public static class WorkflowWebUtil
    {
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
                        output = new TableOutput(true)
                        {
                            IsFix = tableData.IsFix
                        };
                        break;

                    case TableShowStyle.Normal:
                        output = new TableNormalOutput(true)
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