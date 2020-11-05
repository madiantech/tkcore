using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class AppAccessTokenXml
    {
        private class TokenData : IRegName
        {
            public TokenData()
            {
            }

            public TokenData(string key, AccessToken token)
            {
                Key = key;
                Token = token;
            }

            #region IRegName 成员

            public string RegName
            {
                get
                {
                    return Key;
                }
            }

            #endregion IRegName 成员

            [SimpleAttribute]
            public string Key { get; private set; }

            [ObjectElement]
            public AccessToken Token { get; private set; }
        }

        public AppAccessTokenXml()
        {
            TokenList = new RegNameList<TokenData>();
        }

        [ObjectElement(IsMultiple = true)]
        private RegNameList<TokenData> TokenList { get; set; }

        public void Add(string appName, AccessToken token)
        {
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", this);

            TokenList.Add(new TokenData(appName, token));
        }

        public AccessToken GetToken(string appName)
        {
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", this);

            var item = TokenList[appName];
            return item == null ? null : item.Token;
        }
    }
}