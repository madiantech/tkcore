using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-16",
        Description = "为List模板提供包装ObjectContainer的数据模型")]
    internal class ObjectContainerListModelCreator : IModelCreator
    {
        public static readonly IModelCreator Instance = new ObjectContainerListModelCreator();

        private ObjectContainerListModelCreator()
        {
        }

        #region IModelCreator 成员

        public IModel CreateModel(object model)
        {
            return new ObjectContainerListModel(model.Convert<ObjectListModel>());
        }

        #endregion IModelCreator 成员
    }
}