using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal class VueFrameSource : ISource
    {
        public VueFrameSource(string model, string template)
        {
            Model = model;
            Template = template;
        }

        public OutputData DoAction(IInputData input)
        {
            dynamic result = new ExpandoObject();
            JsCacheInfo info = null;
            if (CompareModelVersion(input))
                info = JsCacheUtil.TryGetCacheInfo(input.SourceInfo, Model, Template);
            if (info != null && JsCacheUtil.VerfiyJsCache(info))
            {
                result.Ticks = info.Ticks;
                result.Id = info.Id;
            }
            else
            {
                result.Ticks = null;
                result.Id = null;
            }
            result.Model = info?.Model ?? Model;
            result.Template = info?.Template ?? Template;

            input.CallerInfo.AddInfo(result);
            return OutputData.CreateObject(result);
        }

        public string Model { get; }

        public string Template { get; }

        private bool CompareModelVersion(IInputData input)
        {
            IEsModel model = PlugInFactoryManager.CreateInstance<IEsModel>(
                EsModelPlugInFactory.REG_NAME, Model);
            string destPath = EsModelUtil.GetDestPath(model, input.SourceInfo);
            if (Directory.Exists(destPath))
            {
                int version = EsModelUtil.TryGetVersion(destPath);
                if (version == -1)
                    return false;

                int modVersion = EsModelSettings.Current.GetVersion(Model, Template);
                if (modVersion == -1)
                    return false;

                return version == modVersion;
            }
            return false;
        }
    }
}