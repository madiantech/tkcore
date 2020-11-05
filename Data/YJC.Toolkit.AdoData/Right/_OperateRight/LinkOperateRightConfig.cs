using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2017-06-16", Description = "将操作权限链接起来，取各个权限的交集")]
    internal class LinkOperateRightConfig : IConfigCreator<IOperateRight>
    {
        [DynamicElement(OperateRightConfigFactory.REG_NAME, IsMultiple = true)]
        public List<IConfigCreator<IOperateRight>> Rights { get; private set; }

        #region IConfigCreator<IOperateRight> 成员

        public IOperateRight CreateObject(params object[] args)
        {
            if (Rights == null)
                return null;

            var rights = from item in Rights
                         select item.CreateObject(args);
            return new LinkOperateRight(rights);
        }

        #endregion IConfigCreator<IOperateRight> 成员
    }
}