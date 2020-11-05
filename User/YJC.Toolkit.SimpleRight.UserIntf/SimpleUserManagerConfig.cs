using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight.UserIntf
{
    [UserManagerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-03",
        Description = "基于SimpleRight封装的用户接口")]
    internal class SimpleUserManagerConfig : IConfigCreator<IUserManager>
    {
        #region IConfigCreator<IUserManager> 成员

        public IUserManager CreateObject(params object[] args)
        {
            DbContextConfig config = DbContextUtil.GetDbContextConfig(ContextConfig);
            TkDebug.Assert(string.IsNullOrEmpty(ContextConfig) || config.Name == ContextConfig,
                string.Format(ObjectUtil.SysCulture, "当前配置的ContextConfig名称是{0}，而获取的Context名称为{1}，请检查配置",
                ContextConfig, config.Name), this);

            return new UserManager(config);
        }

        #endregion IConfigCreator<IUserManager> 成员

        [SimpleAttribute]
        public string ContextConfig { get; private set; }
    }
}