using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Web
{
    public interface IEsModel
    {
        string Name { get; }

        string FileDirectory { get; }

        string NodeDirectory { get; }

        ISinglePageGenerator GetPageGenerator(string name);
    }
}
