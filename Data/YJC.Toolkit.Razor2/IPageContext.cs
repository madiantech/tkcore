using System;
using System.IO;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public interface IPageContext
    {
        TextWriter Writer { get; set; }

        dynamic ViewBag { get; }

        object InitData { get; }

        void DefineSection(string name, Func<Task> section);

        bool IsSectionDefined(string name);

        Func<Task> GetSectionDelegate(string name);
    }
}