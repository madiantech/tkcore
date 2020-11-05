using System;
using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public abstract class BaseBootcssData : IRazorField
    {
        private readonly IList<RazorField> fDisplayFields;

        protected BaseBootcssData()
        {
            fDisplayFields = new RegNameList<RazorField>();
        }

        #region IRazorField 成员

        public RazorField GetDisplayField(IFieldInfo field)
        {
            if (field == null)
                return null;
            return ((RegNameList<RazorField>)fDisplayFields)[field.NickName];
        }

        public RazorField GetDisplayField(string tableName, IFieldInfo field)
        {
            if (field == null)
                return null;

            if (string.IsNullOrEmpty(tableName))
                return GetDisplayField(field);
            string key = GetKey(tableName, field.NickName);
            return ((RegNameList<RazorField>)fDisplayFields)[key];
        }

        #endregion IRazorField 成员

        public RazorOutputData Header { get; set; }

        public RazorOutputData Footer { get; set; }

        private void AddDisplayField(string nickName, RazorContentType contentType, string content)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            fDisplayFields.Add(new RazorField(nickName, contentType, content));
        }

        protected internal static string GetKey(string tableName, string nickName)
        {
            return string.Format(ObjectUtil.SysCulture, "{0}.{1}", tableName, nickName);
        }

        public void AddDisplayField(RazorField field)
        {
            fDisplayFields.Add(field);
        }

        public void AddDisplayFieldSection(string nickName, string sectionName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, "sectionName", this);

            AddDisplayField(nickName, RazorContentType.Section, sectionName);
        }

        public void AddDisplayFieldText(string nickName, string text)
        {
            TkDebug.AssertArgumentNull(text, "text", this);

            AddDisplayField(nickName, RazorContentType.Text, text);
        }

        public void AddDisplayFieldFile(string nickName, string localFile)
        {
            TkDebug.AssertArgumentNull(localFile, "localFile", this);

            AddDisplayField(nickName, RazorContentType.RazorFile, localFile);
        }
    }
}