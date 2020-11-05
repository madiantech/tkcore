using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    internal abstract class BaseMobileConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        public BaseConstraint CreateObject(params object[] args)
        {
            throw new NotImplementedException();
        }

        #endregion

        [SimpleAttribute]
        public string Pattern { get; private set; }
    }
}
