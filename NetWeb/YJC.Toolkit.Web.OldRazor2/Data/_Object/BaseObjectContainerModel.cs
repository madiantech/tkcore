namespace YJC.Toolkit.Data
{
    public abstract class BaseObjectContainerModel : IModel
    {
        protected BaseObjectContainerModel(object model)
        {
            Model = model;
        }

        #region IModel 成员

        public object SourceObject
        {
            get
            {
                return Model;
            }
        }

        #endregion IModel 成员

        public object Model { get; private set; }
    }
}