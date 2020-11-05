using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class PageContext : IPageContext, IDisposable
    {
        private readonly dynamic fViewBag;

        public PageContext()
        {
            fViewBag = new ExpandoObject();
            Writer = new StringWriter();
        }

        public PageContext(ExpandoObject viewBag, object initData)
        {
            fViewBag = viewBag ?? new ExpandoObject();
            InitData = initData;
        }

        public TextWriter Writer { get; set; }

        public dynamic ViewBag => fViewBag;

        public string ExecutingPageKey { get; set; }

        public ModelTypeInfo ModelTypeInfo { get; set; }

        public object Model { get; set; }

        public IDictionary<string, Func<Task>> SectionWriters { get; } = new Dictionary<string, Func<Task>>(StringComparer.OrdinalIgnoreCase);

        public object InitData { get; }

        public void DefineSection(string name, Func<Task> section)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, nameof(name), this);
            TkDebug.AssertArgumentNull(section, nameof(section), this);

            if (SectionWriters.ContainsKey(name))
            {
                throw new InvalidOperationException();
            }
            SectionWriters[name] = section;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Func<Task> GetSectionDelegate(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, nameof(name), this);

            SectionWriters.TryGetValue(name, out var result);
            return result;
        }

        public bool IsSectionDefined(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, nameof(name), this);

            return SectionWriters.ContainsKey(name);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                Writer.DisposeObject();
        }
    }
}