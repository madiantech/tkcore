using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public abstract class BaseXmlPlatformSettingCreator<T> : IPlatformSettingCreator<T>
    {
        protected BaseXmlPlatformSettingCreator(string fileName)
        {
            FileName = fileName;
        }

        #region IPlatformSettingCreator<T> 成员

        public abstract T Create(string tenantId);

        public virtual AccessToken ReadToken(string tenantId, object customData)
        {
            if (File.Exists(FileName))
            {
                AccessToken token = new AccessToken();
                token.ReadXmlFromFile(FileName);

                return token;
            }
            return null;
        }

        public virtual void SaveToken(string tenantId, object customData, AccessToken token)
        {
            FileUtil.VerifySaveFile(FileName, token.WriteXml(), ToolkitConst.UTF8);
        }

        #endregion

        public string FileName { get; private set; }
    }
}
