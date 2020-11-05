using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PostObjectConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2013-11-21",
        Description = "提交数据为Json格式，转换成对应标记了RegTypeAttribute的类型", Author = "YJC")]
    internal class JsonObjectCreatorConfig : IConfigCreator<IPostObjectCreator>
    {
        #region IConfigCreator<IPostObjectCreator> 成员

        public IPostObjectCreator CreateObject(params object[] args)
        {
            JsonObjectCreator result = new JsonObjectCreator(RegClassName)
            {
                ModelName = ModelName
            };
            if (ReadSettings != null)
                result.ReadSettings = ReadSettings;
            return result;
        }

        #endregion

        [SimpleAttribute]
        public string RegClassName { get; private set; }

        [SimpleAttribute]
        public string ModelName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ReadSettings ReadSettings { get; private set; }
    }
}
