using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PostObjectConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-18",
        Description = "读取Json数据包中内嵌类型的数据，转换成对应标记了RegTypeAttribute的类型", Author = "YJC")]
    internal sealed class CustomJsonObjectCreatorConfig : IConfigCreator<IPostObjectCreator>
    {
        #region IConfigCreator<IPostObjectCreator> 成员

        public IPostObjectCreator CreateObject(params object[] args)
        {
            return new CustomJsonObjectCreator(RegTypeName, LocalName)
            {
                UseConstructor = UseConstructor,
                ModelName = ModelName
            };
        }

        #endregion

        [SimpleAttribute]
        public string LocalName { get; private set; }

        [SimpleAttribute]
        public string RegTypeName { get; private set; }

        [SimpleAttribute]
        public bool UseConstructor { get; private set; }

        [SimpleAttribute]
        public string ModelName { get; private set; }
    }
}
