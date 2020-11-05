using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class NormalContentXml : ContentXml
    {
        private readonly ContentXml fSource;
        private static readonly TableMetaDataConfig fStep;
        private static readonly TableMetaDataConfig fHisStep;
        private readonly List<TableMetaDataConfig> fTableList;

        static NormalContentXml()
        {
            fStep = CreateMetaDataConfig(XmlSegment.StepTableXml);
            fHisStep = CreateMetaDataConfig(XmlSegment.StepHisTableXml);
        }

        public NormalContentXml(ContentXml source)
            : this(source, ProcessDisplay.Normal, false)
        {
        }

        public NormalContentXml(ContentXml source, ProcessDisplay display, bool isHis)
        {
            fSource = source;
            if (fSource.TableList != null)
                fTableList = new List<TableMetaDataConfig>(fSource.TableList);
            else
                fTableList = new List<TableMetaDataConfig>(1);
            if (display != ProcessDisplay.None)
            {
                if (isHis)
                    fTableList.Add(fHisStep);
                else
                    fTableList.Add(fStep);
            }
            PageMaker = fSource.PageMaker;
            TableList = fTableList;
        }

        private static TableMetaDataConfig CreateMetaDataConfig(string xml)
        {
            TableMetaDataConfig fStep = new TableMetaDataConfig();
            fStep.ReadXml(xml, ReadSettings.Default, WorkflowWebConst.ROOT_TABLE);
            return fStep;
        }
    }
}