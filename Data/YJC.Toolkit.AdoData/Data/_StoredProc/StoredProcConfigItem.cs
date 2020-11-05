using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class StoredProcConfigItem
    {
        private readonly List<ParamConfigItem> fParams;

        public StoredProcConfigItem()
        {
            fParams = new List<ParamConfigItem>();
        }

        [SimpleAttribute]
        public string Name { get; private set; }

        [ObjectElement(LocalName = "Param", IsMultiple = true, ObjectType = typeof(ParamConfigItem))]
        public IEnumerable<ParamConfigItem> Params
        {
            get
            {
                return fParams;
            }
        }
    }
}