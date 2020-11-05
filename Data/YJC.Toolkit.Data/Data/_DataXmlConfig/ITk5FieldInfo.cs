using System.Collections.Generic;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal interface ITk5FieldInfo : IFieldInfoEx
    {
        List<IConfigCreator<BaseConstraint>> Constraints { get; }

        Tk5ListDetailConfig ListDetail { get; }

        Tk5EditConfig Edit { get; }

        Tk5ExtensionConfig Extension { get; }
    }
}
