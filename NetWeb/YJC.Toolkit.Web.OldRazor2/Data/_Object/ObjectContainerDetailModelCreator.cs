using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-16",
        Description = "为Detail模板提供包装ObjectContainer的数据模型")]
    internal class ObjectContainerDetailModelCreator : IModelCreator
    {
        public static readonly IModelCreator Instance = new ObjectContainerDetailModelCreator();

        private ObjectContainerDetailModelCreator()
        {
        }

        #region IModelCreator 成员

        public IModel CreateModel(object model)
        {
            return new ObjectContainerDetailModel(model.Convert<DetailObjectModel>());
        }

        #endregion IModelCreator 成员
    }
}