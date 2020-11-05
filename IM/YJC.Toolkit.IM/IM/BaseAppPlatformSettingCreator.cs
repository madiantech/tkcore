using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public abstract class BaseAppPlatformSettingCreator<T>
        : IPlatformSettingCreator<T>
    {
        private readonly AppAccessTokenXml fAccessTokenXml;
        private bool fInitXml;

        protected BaseAppPlatformSettingCreator()
        {
            fAccessTokenXml = new AppAccessTokenXml();
        }

        #region IPlatformSettingCreator<WeCorpSettings> 成员

        public abstract T Create(string tenantId);

        public AccessToken ReadToken(string tenantId, object customData)
        {
            InitXml();
            string appName = GetAppName(tenantId, customData);
            return fAccessTokenXml.GetToken(appName);
        }

        public void SaveToken(string tenantId, object customData, AccessToken token)
        {
            InitXml();
            string appName = GetAppName(tenantId, customData);
            fAccessTokenXml.Add(appName, token);
            SaveTokenXml(fAccessTokenXml);
        }

        #endregion IPlatformSettingCreator<WeCorpSettings> 成员

        private void InitXml()
        {
            if (!fInitXml)
            {
                ReadTokenXml(fAccessTokenXml);
                fInitXml = true;
            }
        }

        protected abstract string GetAppName(string tenantId, object customData);

        protected abstract void ReadTokenXml(AppAccessTokenXml tokenXml);

        protected abstract void SaveTokenXml(AppAccessTokenXml tokenXml);
    }
}