using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Web
{
    internal class EsModelSettings
    {
        private readonly Dictionary<(string modelName, string templateName), int> fVersionDict;
        private static EsModelSettings fCurrent;

        public EsModelSettings()
        {
            fVersionDict = new Dictionary<(string modelName, string templateName), int>();
            fCurrent = this;
        }

        public static EsModelSettings Current { get => fCurrent; }

        private static List<IEsModel> GetModels(BaseGlobalVariable globalVariable)
        {
            EsModelPlugInFactory modelFactory = globalVariable.FactoryManager.GetCodeFactory(
                EsModelPlugInFactory.REG_NAME).Convert<EsModelPlugInFactory>();
            List<IEsModel> modelList = new List<IEsModel>();
            modelFactory.EnumableCodePlugIn((regName, type, attr)
                => modelList.Add(modelFactory.CreateInstance<IEsModel>(regName)));

            return modelList;
        }

        private static List<IEsTemplate> GetTemplates(BaseGlobalVariable globalVariable)
        {
            EsTemplatePlugInFactory templateFactory = globalVariable.FactoryManager.GetCodeFactory(
                EsTemplatePlugInFactory.REG_NAME).Convert<EsTemplatePlugInFactory>();
            List<IEsTemplate> templateList = new List<IEsTemplate>();
            templateFactory.EnumableCodePlugIn((regName, type, attr)
                => templateList.Add(templateFactory.CreateInstance<IEsTemplate>(regName)));

            return templateList;
        }

        public void Initialize(BaseGlobalVariable globalVariable)
        {
            var models = GetModels(globalVariable);
            var templates = GetTemplates(globalVariable);

            using (EmptyDbDataSource source = new EmptyDbDataSource
            {
                Context = JsCacheContext.GetDbContext()
            })
            using (JsModelResolver resolver = new JsModelResolver(source))
            {
                List<EsModelData> list = (from item in models select new EsModelData(item)).ToList();
                foreach (var data in list)
                {
                    foreach (var template in templates)
                    {
                        if (data.Dictionary.TryGetValue(template.TemplatePath, out var files))
                        {
                            string modelName = data.Model.Name;
                            string templateName = template.Name;
                            int version = resolver.GetVersion(modelName, templateName, files);
                            fVersionDict.Add((modelName, templateName), version);
                        }
                    }
                }
            }
        }

        public int GetVersion(string modelName, string templateName)
        {
            if (fVersionDict.TryGetValue((modelName, templateName), out int version))
                return version;

            string msg = $"模式{modelName}下不存在{templateName}，请确认配置";
            TkDebug.ThrowToolkitException(msg, this);

            return 0;
        }

        public int GetVersion(IEsModel model, IEsTemplate template)
        {
            if (fVersionDict.TryGetValue((model.Name, template.Name), out int version))
                return version;

            string msg = $"模式{model.Name}下不存在目录{template.TemplatePath}，请确认配置";
            TkDebug.ThrowToolkitException(msg, this);

            return 0;
        }
    }
}