using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-16",
        Description = "为Edit模板提供包装ObjectContainer的数据模型")]
    internal class ObjectContainerEditModelCreator : IModelCreator
    {
        public static readonly IModelCreator Instance = new ObjectContainerEditModelCreator();

        private ObjectContainerEditModelCreator()
        {
        }

        #region IModelCreator 成员

        public IModel CreateModel(object model)
        {
            return new ObjectContainerEditModel(model.Convert<EditObjectModel>());
        }

        #endregion IModelCreator 成员
    }
}