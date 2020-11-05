using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ApproveProcessXml : ProcessXml
    {
        private static readonly ProcessTableDataConfig fApprove;

        static ApproveProcessXml()
        {
            fApprove = new ProcessTableDataConfig();
            fApprove.ReadXml(XmlSegment.ApproveProcessXml, ReadSettings.Default,
                WorkflowWebConst.ROOT_TABLE);
        }

        public ApproveProcessXml(ProcessXml source)
        {
            List<ProcessTableDataConfig> tableList;
            if (source == null)
            {
                tableList = new List<ProcessTableDataConfig>(1);
                FillMode = FillContentMode.All;
            }
            else
            {
                FillMode = source.FillMode;
                PageMaker = source.PageMaker;
                ContentList = source.ContentList;
                if (source.TableList != null)
                    tableList = new List<ProcessTableDataConfig>(source.TableList);
                else
                    tableList = new List<ProcessTableDataConfig>(1);
            }
            tableList.Add(fApprove);
            TableList = tableList;
            if (source.RecordLogs != null)
                RecordLogs = source.RecordLogs;
            if (source.Relations != null)
                Relations = source.Relations;
        }
    }
}