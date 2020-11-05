using System;
using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorFileTemplate : RazorBaseTemplate
    {
        private string fLocalPath;
        private string fLayoutPath;

        public RazorFileTemplate()
        {
            Encoding = Encoding.UTF8;
            BaseType = typeof(RazorFileTemplate);
        }

        public object ConfigurationData { get; private set; }

        public string LocalPath
        {
            get
            {
                TkDebug.AssertNotNullOrEmpty(fLocalPath,
                    "LocalPath没有被正确初始化，请确认传入的configurationData", this);

                return fLocalPath;
            }
        }

        public string Layout { get; private set; }

        public string LayoutPath
        {
            get
            {
                TkDebug.AssertNotNullOrEmpty(fLayoutPath,
                    "LayoutPath没有被正确初始化，请确认传入的configurationData", this);

                return fLayoutPath;
            }
        }

        public Encoding Encoding { get; set; }

        public Type BaseType { get; set; }

        public override void InitializeTemplate(object configurationData)
        {
            IRazorPath path = configurationData as IRazorPath;
            if (path != null)
            {
                fLocalPath = path.LocalPath;
                Layout = path.LayoutFile;
                if (!string.IsNullOrEmpty(Layout))
                {
                    fLayoutPath = path.LayoutPath;
                    path.ClearLayoutFile();
                }
                else
                {
                    if (string.IsNullOrEmpty(path.LayoutPath))
                        fLayoutPath = fLocalPath;
                    else
                        fLayoutPath = path.LayoutPath;
                }

                ConfigurationData = configurationData;
            }
        }

        protected override void RenderLayout()
        {
            if (!string.IsNullOrEmpty(Layout))
                RenderFile(LayoutPath, Layout, Model);
        }

        private static string GetPath(string basePath, string path)
        {
            if (Path.IsPathRooted(path))
                return path;
            return Path.GetFullPath(Path.Combine(basePath, path));
        }

        private string RenderFile(string basePath, string fileName, object model)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            string path = GetPath(basePath, fileName);
            if (!File.Exists(path))
                return string.Empty;

            model = model ?? Model;
            FileData data = new FileData(BaseType, path, Layout, Encoding, ConfigurationData);
            RazorBaseTemplate template = TemplateService.GetTemplate(data.RazorTemplate, data.BaseType,
                model, ViewBag, data.CacheName, data.ConfigurationData);
            string content = template.Run(Context);
            return content;
        }

        private static string RenderTextFile(string basePath, string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            path = GetPath(basePath, path);
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        public virtual string RenderLocalPartial(string fileName, object model)
        {
            return RenderFile(LocalPath, fileName, model);
        }

        public virtual string RenderLayoutPartial(string fileName, object model)
        {
            return RenderFile(LayoutPath, fileName, model);
        }

        public virtual string RenderSectionIfDefined(string name, string layoutFileName, object model)
        {
            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return RenderLayoutPartial(layoutFileName, model);
        }

        public virtual string RenderSectionOrDefault(string name, string defaultName)
        {
            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return RenderSection(defaultName);
        }

        public virtual string RenderLocalTextFile(string fileName)
        {
            return RenderTextFile(LocalPath, fileName);
        }

        public virtual string RenderLayoutTextFile(string fileName)
        {
            return RenderTextFile(LayoutPath, fileName);
        }
    }
}
