using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class PageTemplateInitData : IRazorPath
    {
        public PageTemplateInitData(string templateFile, string razorFile,
            string pageTemplateName, string modelCreatorName)
        {
            PageTemplateName = pageTemplateName;
            ModelCreatorName = modelCreatorName;
            string fullPath;
            if (string.IsNullOrEmpty(templateFile))
                fullPath = null;
            else
                fullPath = Path.Combine(BaseAppSetting.Current.XmlPath,
                    "razortemplate", templateFile);
            Initalize(razorFile, fullPath);
        }

        #region IRazorPath 成员

        public string LocalPath { get; private set; }

        public string LayoutPath { get; private set; }

        public string LayoutFile { get; private set; }

        public void ClearLayoutFile()
        {
            throw new NotImplementedException();
        }

        #endregion IRazorPath 成员

        public string FileName { get; private set; }

        public string PageTemplateName { get; private set; }

        public string ModelCreatorName { get; private set; }

        private void Initalize(string xmlFile, string fullPath)
        {
            if (string.IsNullOrEmpty(xmlFile))
                SetLocalPath(fullPath);
            else
            {
                FileName = Path.Combine(BaseAppSetting.Current.XmlPath, "razor", xmlFile);
                if (File.Exists(FileName))
                {
                    LocalPath = Path.GetDirectoryName(FileName);
                    if (!string.IsNullOrEmpty(fullPath))
                    {
                        LayoutFile = fullPath;
                        LayoutPath = Path.GetDirectoryName(fullPath);
                    }
                }
                else
                    SetLocalPath(fullPath);
            }
        }

        private void SetLocalPath(string fullPath)
        {
            LocalPath = Path.GetDirectoryName(fullPath);
            FileName = fullPath;
        }
    }
}