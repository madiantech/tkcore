using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PostObjectConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2013-01-14",
        Description = "根据具体条件来选择相应配置的PostObjectCreator", Author = "YJC")]
    internal class CompositeCreatorConfig : IConfigCreator<IPostObjectCreator>
    {
        #region IConfigCreator<IPostObjectCreator> 成员

        public IPostObjectCreator CreateObject(params object[] args)
        {
            //IInputData input = ObjectUtil.QueryObject<IInputData>(args);
            return new CompositeCreator(this);
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<CompositeCreatorItemConfig> Items { get; private set; }
    }
}
