using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class EsModelData
    {
        private readonly Dictionary<string, List<FileCacheInfo>> fDictionary;

        public EsModelData(IEsModel model)
        {
            Model = model;
            fDictionary = new Dictionary<string, List<FileCacheInfo>>(StringComparer.OrdinalIgnoreCase);
            string modelPath = Path.Combine(EsModelUtil.EsTemplatePath, model.FileDirectory);
            if (!Directory.Exists(modelPath))
                return;
            var dirs = Directory.GetDirectories(modelPath);
            foreach (var dir in dirs)
            {
                string newDir = Path.GetRelativePath(modelPath, dir);
                var files = EsModelUtil.CreateModelCache(dir);
                fDictionary.Add(newDir, files);
            }
        }

        public IEsModel Model { get; }

        public Dictionary<string, List<FileCacheInfo>> Dictionary { get => fDictionary; }
    }
}