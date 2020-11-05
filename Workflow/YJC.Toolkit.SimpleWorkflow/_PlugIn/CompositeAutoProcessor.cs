using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class CompositeAutoProcessor : AutoProcessor
    {
        private readonly List<AutoProcessor> fList = new List<AutoProcessor>();

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow)
        {
            IEnumerable<TableResolver>[] result = new IEnumerable<TableResolver>[fList.Count];
            int i = 0;
            foreach (var item in fList)
            {
                item.FillMode = FillMode;
                var itemResult = item.Execute(workflowRow);
                result[i++] = itemResult;
            }
            IsCreateSubWorkflow = fList.Any(item => item.IsCreateSubWorkflow);

            var realResult = EnumUtil.Convert(result).Distinct();
            return realResult;
        }

        public void Add(AutoProcessor processor)
        {
            TkDebug.AssertArgumentNull(processor, "processor", null);

            fList.Add(processor);
        }
    }
}