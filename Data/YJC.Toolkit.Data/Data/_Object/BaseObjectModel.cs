using System.Dynamic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseObjectModel
    {
        protected BaseObjectModel()
        {
            CallerInfo = new ExpandoObject();
        }

        public dynamic CallerInfo { get; private set; }

        public ObjectContainer Object { get; internal set; }
    }
}