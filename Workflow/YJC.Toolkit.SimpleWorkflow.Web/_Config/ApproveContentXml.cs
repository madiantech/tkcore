using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ApproveContentXml : ContentXml
    {
        private static readonly TableMetaDataConfig fApprove;

        static ApproveContentXml()
        {
            fApprove = new TableMetaDataConfig();
            fApprove.ReadXml(XmlSegment.ApproveContentXml, ReadSettings.Default,
                WorkflowWebConst.ROOT_TABLE);
        }

        public ApproveContentXml(ContentXml source)
        {
            List<TableMetaDataConfig> tableList;
            if (source != null && source.TableList != null)
                tableList = new List<TableMetaDataConfig>(source.TableList);
            else
                tableList = new List<TableMetaDataConfig>(1);
            tableList.Add(fApprove);
            if (source != null)
                PageMaker = source.PageMaker;
            TableList = tableList;
        }
    }
}