using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class AccessToken : BaseResult, ICacheDependency, ICacheDependencyCreator
    {
        #region ICacheDependency 成员

        public bool HasChanged
        {
            get
            {
                return !IsValid;
            }
        }

        #endregion ICacheDependency 成员

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return this;
        }

        #endregion ICacheDependencyCreator 成员

        [SimpleElement]
        internal DateTime ExpireTime { get; set; }

        [SimpleElement(LocalName = "access_token", Order = 30)]
        public string Token { get; protected set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 40)]
        internal int ExpiresIn { get; set; }

        public bool IsValid
        {
            get
            {
                return ExpireTime >= DateTime.Now;
            }
        }

        public void SetExpireTime()
        {
            // 比过期时间少60秒
            ExpireTime = DateTime.Now.AddSeconds(ExpiresIn - 60);
        }

        public void SetExpireTime(int expiresIn)
        {
            ExpiresIn = expiresIn;
            SetExpireTime();
        }

        public void Assign(AccessToken token)
        {
            if (token == null)
                return;

            Token = token.Token;
            ExpiresIn = token.ExpiresIn;
            ExpireTime = token.ExpireTime;
        }

        public override string ToString()
        {
            return Token;
        }
    }
}