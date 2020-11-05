using System.Collections.Generic;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IMultiEditModel : IEditModel
    {
        IFieldValueProvider CreateEmptyProvider();
    }
}