using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal class TestInputData : IInputData
    {
        private class EmptyCallerInfo : ICallerInfo
        {
            public void AddInfo(DataSet dataSet)
            {
            }

            public void AddInfo(StringBuilder builder)
            {
            }

            public void AddInfo(XElement element)
            {
            }

            public void AddInfo(dynamic data)
            {
            }
        }

        public TestInputData(IPageStyle style, IQueryString query, string source)
        {
            Style = style;
            QueryString = query;
            SourceInfo = new PageSourceInfo("c", null, style, source, true);
            CallerInfo = new EmptyCallerInfo();
        }

        public IPageStyle Style { get; }

        public bool IsPost => false;

        public IQueryString QueryString { get; }

        public object PostObject => null;

        public ICallerInfo CallerInfo { get; }

        public string QueryStringText => null;

        public PageSourceInfo SourceInfo { get; }
    }
}