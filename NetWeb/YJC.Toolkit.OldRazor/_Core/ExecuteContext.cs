using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    /// <summary>
    /// Defines a context for tracking template execution.
    /// </summary>
    public class ExecuteContext
    {
        private readonly IDictionary<string, Action> fDefinedSections = new Dictionary<string, Action>();
        private readonly dynamic fViewBag;
        private StringBuilder fBuilder;

        /// <summary>
        /// Creates a new instance of ExecuteContext with an empty ViewBag.
        /// </summary>
        public ExecuteContext()
        {
            fViewBag = new DynamicObjectBag();
        }

        /// <summary>
        /// Creates a new instance of DynamicViewBag, setting initial values in the ViewBag.
        /// </summary>
        /// <param name="viewBag">The initial view bag data or NULL for an empty ViewBag.</param>
        public ExecuteContext(DynamicObjectBag viewBag)
        {
            if (viewBag == null)
                fViewBag = new DynamicObjectBag();
            else
                fViewBag = viewBag;
        }

        /// <summary>
        /// Gets the current writer.
        /// </summary>
        //internal TextWriter CurrentWriter { get { return _writers.Peek(); } }
        internal TextWriter CurrentWriter { get; private set; }

        internal RazorConfiguration Config { get; set; }

        /// <summary>
        /// Gets the viewbag that allows sharing state.
        /// </summary>
        public dynamic ViewBag
        {
            get
            {
                return fViewBag;
            }
        }

        /// <summary>
        /// Defines a section used in layouts.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <param name="action">The delegate action used to write the section at a later stage in the template execution.</param>
        internal void DefineSection(string name, Action action)
        {
            TkDebug.Assert(!fDefinedSections.ContainsKey(name), string.Format(ObjectUtil.SysCulture,
                "已经定义了名称为{0}的Section", name), this);

            fDefinedSections.Add(name, action);
        }

        /// <summary>
        /// Gets the section delegate.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <returns>The section delegate.</returns>
        internal Action GetSectionDelegate(string name)
        {
            Action result;
            if (fDefinedSections.TryGetValue(name, out result))
                return result;

            return null;
        }

        internal bool ConstainsSection(string name)
        {
            return fDefinedSections.ContainsKey(name);
        }

        internal bool CreateWriter()
        {
            if (CurrentWriter == null)
            {
                fBuilder = new StringBuilder();
                CurrentWriter = new StringWriter(fBuilder, ObjectUtil.SysCulture);
                return true;
            }
            else
                return false;
        }

        internal string DisposeWriter()
        {
            if (CurrentWriter != null)
            {
                CurrentWriter.Dispose();
                CurrentWriter = null;
                return fBuilder.ToString();
            }
            return string.Empty;
        }
    }
}