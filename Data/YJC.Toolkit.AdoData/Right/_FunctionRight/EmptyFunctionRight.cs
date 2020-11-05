using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [Serializable]
    public class EmptyFunctionRight : IFunctionRight
    {
        #region IFunctionRight 成员

        public virtual DataSet GetMenuObject(object userId)
        {
            return null;
        }

        public virtual bool IsFunction(object key)
        {
            return true;
        }

        public virtual bool IsSubFunction(SubFunctionKey subKey, object key)
        {
            return true;
        }

        public virtual bool IsAdmin()
        {
            return true;
        }

        public virtual void Initialize(IUserInfo user)
        {
        }

        #endregion IFunctionRight 成员
    }
}