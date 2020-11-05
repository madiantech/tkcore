using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(Description = "根据具体条件来选择相应配置的数据源",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-12-25")]
    internal class CompositeSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            CompositeSource source = CreateSource();
            if (Items != null)
                foreach (var item in Items)
                    source.Add(item.Condition.UseCondition, item.Source);

            return source;
        }

        #endregion IConfigCreator<ISource> 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<CompositeSourceItemConfig> Items { get; protected set; }

        protected virtual CompositeSource CreateSource()
        {
            return new CompositeSource();
        }
    }
}