using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public abstract class TemplatePage : BaseTemplatePage
    {
        //private readonly HashSet<string> fRenderedSections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //private bool fRenderedBody;

        //private bool fIgnoreBody;
        private HashSet<string> fIgnoredSections;

        protected TemplatePage()
            : base()
        {
        }

        public async Task IncludeAsync(string key, object model = null)
        {
            TkDebug.AssertArgumentNull(key, nameof(key), this);
            TkDebug.AssertNotNull(IncludeFunc, nameof(IncludeFunc) + "is not set", this);

            await IncludeFunc(key, model);
        }

        protected virtual IHtmlContent RenderBody()
        {
            TkDebug.AssertNotNull(BodyContent, "Method can not be called", this);

            //fRenderedBody = true;
            return BodyContent;
        }

        public void IgnoreBody()
        {
            //fIgnoreBody = true;
        }

        public override void DefineSection(string name, Func<Task> section)
        {
            //TkDebug.AssertArgumentNull(name, nameof(name), this);
            //TkDebug.AssertArgumentNull(section, nameof(section), this);

            //if (SectionWriters.ContainsKey(name))
            //{
            //    throw new InvalidOperationException($"Section {name} is already defined");
            //}
            //SectionWriters[name] = section;
            PageContext.DefineSection(name, section);
        }

        public bool IsSectionDefined(string name)
        {
            //TkDebug.AssertArgumentNull(name, nameof(name), this);

            //EnsureMethodCanBeInvoked(nameof(IsSectionDefined));
            //return SectionWriters.ContainsKey(name);
            return PageContext.IsSectionDefined(name);
        }

        public HtmlString RenderSectionIfDefined(string name)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return HtmlString.Empty;
        }

        public HtmlString RenderSection(string name)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            return RenderSection(name, true);
        }

        public HtmlString RenderSection(string name, bool required)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            //EnsureMethodCanBeInvoked(nameof(RenderSection));

            var task = RenderSectionAsyncCore(name, required);
            return task.GetAwaiter().GetResult();
        }

        public Task<HtmlString> RenderSectionAsync(string name)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            return RenderSectionAsync(name, true);
        }

        public Task<HtmlString> RenderSectionAsync(string name, bool required)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            //EnsureMethodCanBeInvoked(nameof(RenderSectionAsync));
            return RenderSectionAsyncCore(name, required);
        }

        public Task<HtmlString> RenderSectionIfDefinedAsync(string name)
        {
            TkDebug.AssertArgumentNull(name, nameof(name), this);

            if (IsSectionDefined(name))
                return RenderSectionAsync(name);
            else
                return Task.FromResult(HtmlString.Empty);
        }

        private async Task<HtmlString> RenderSectionAsyncCore(string sectionName, bool required)
        {
            //TkDebug.Assert(!fRenderedSections.Contains(sectionName),
            //    $"Section {sectionName} is already rendered", this);
            var renderDelegate = PageContext.GetSectionDelegate(sectionName);
            if (renderDelegate != null)
            {
                //fRenderedSections.Add(sectionName);

                await renderDelegate();

                // Return a token value that allows the Write call that wraps the RenderSection \
                // RenderSectionAsync to succeed.
                return HtmlString.Empty;
            }
            else if (required)
            {
                throw new InvalidOperationException($"Section {sectionName} is not defined");
            }
            else
            {
                // If the section is optional and not found, then don't do anything.
                return null;
            }
        }

        public void IgnoreSection(string sectionName)
        {
            TkDebug.AssertArgumentNull(sectionName, nameof(sectionName), this);
            TkDebug.Assert(PageContext.IsSectionDefined(sectionName),
                $"Section {sectionName} is not defined", this);

            if (fIgnoredSections == null)
            {
                fIgnoredSections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            fIgnoredSections.Add(sectionName);
        }

        public override void EnsureRenderedBodyOrSections()
        {
            // a) all sections defined for this page are rendered.
            // b) if no sections are defined, then the body is rendered if it's available.
            //if (PreviousSectionWriters != null && PreviousSectionWriters.Count > 0)
            //{
            //    var sectionsNotRendered = PreviousSectionWriters.Keys.Except(
            //        fRenderedSections,
            //        StringComparer.OrdinalIgnoreCase);

            // string[] sectionsNotIgnored; if (fIgnoredSections != null) { sectionsNotIgnored =
            // sectionsNotRendered.Except(fIgnoredSections,
            // StringComparer.OrdinalIgnoreCase).ToArray(); } else { sectionsNotIgnored =
            // sectionsNotRendered.ToArray(); }

            //    if (sectionsNotIgnored.Length > 0)
            //    {
            //        var sectionNames = string.Join(", ", sectionsNotIgnored);
            //        throw new InvalidOperationException($"One or more section(s) have been ignored. Ignored section(s): '{sectionNames}'");
            //    }
            //}
            //else if (BodyContent != null && !fRenderedBody && !fIgnoreBody)
            //{
            //    // There are no sections defined, but RenderBody was NOT called.
            //    // If a body was defined and the body not ignored, then RenderBody should have been called.
            //    throw new InvalidOperationException("RenderBody was not called");
            //}
        }

        public override void BeginContext(int position, int length, bool isLiteral)
        {
        }

        public override void EndContext()
        {
        }

        public override string ToString()
        {
            return $"{Key}的代码类";
        }
    }
}