using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    internal class XmlTemplateInitData : IRazorPath
    {
        private Type fDataType;

        internal XmlTemplateInitData(RazorTemplateAttribute attribute, string xmlFile)
        {
            string fullPath = Path.Combine(BaseAppSetting.Current.XmlPath, "razortemplate",
                attribute.TemplateFile);
            Initalize(attribute, xmlFile, fullPath);
        }

        internal XmlTemplateInitData(RazorTemplateAttribute attribute, string templateFile, string xmlFile)
        {
            string fullPath = Path.Combine(BaseAppSetting.Current.XmlPath, "razortemplate",
                attribute.TemplateFile);
            fullPath = Path.Combine(Path.GetDirectoryName(fullPath), templateFile);
            Initalize(attribute, xmlFile, fullPath);
        }

        #region IRazorPath 成员

        public string LocalPath { get; private set; }

        public string LayoutPath { get; private set; }

        public string LayoutFile { get; private set; }

        public void ClearLayoutFile()
        {
            LayoutFile = null;
        }

        #endregion

        public string FileName { get; private set; }

        internal object BagData
        {
            get
            {
                if (fDataType != null)
                    return ObjectUtil.CreateObject(fDataType);
                return null;
            }
        }

        private void Initalize(RazorTemplateAttribute attribute, string xmlFile, string fullPath)
        {
            if (string.IsNullOrEmpty(xmlFile))
                SetLocalPath(fullPath);
            else
            {
                FileName = Path.Combine(BaseAppSetting.Current.XmlPath, "razor", xmlFile);
                if (File.Exists(FileName))
                {
                    LocalPath = Path.GetDirectoryName(FileName);
                    LayoutFile = fullPath;
                    LayoutPath = Path.GetDirectoryName(fullPath);
                }
                else
                    SetLocalPath(fullPath);
            }
            fDataType = attribute.PageDataType;
        }

        private void SetLocalPath(string fullPath)
        {
            LocalPath = Path.GetDirectoryName(fullPath);
            FileName = fullPath;
        }
    }
}
