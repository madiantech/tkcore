using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Source]
    internal class Test2PartSource : ISource
    {
        public OutputData DoAction(IInputData input)
        {
            dynamic result = new ExpandoObject();
            JsCacheInfo info = JsCacheUtil.TryGetCacheInfo(input.SourceInfo, "GaoVueDemo", "Single");
            if (info != null && JsCacheUtil.VerfiyJsCache(info))
            {
                result.Ticks = info.Ticks;
                result.Id = info.Id;
            }
            else
            {
                result.Ticks = null;
                result.Id = null;
                info = new JsCacheInfo { Model = "GaoVueDemo", Template = "Single" };
            }
            result.ModuleCreator = input.SourceInfo.ModuleCreator;
            result.Source = input.SourceInfo.Source;
            result.Model = info.Model;
            result.Template = info.Template;
            return OutputData.CreateObject(result);
        }
    }
}