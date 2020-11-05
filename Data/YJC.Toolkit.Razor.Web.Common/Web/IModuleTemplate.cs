using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IModuleTemplate
    {
        IEnumerable<PageTemplateInfo> PageTemplates { get; }

        IEnumerable<PageMakerInfo> CreatePageMakers(IPageData pageData);

        void SetPageData(ISource source, IInputData input, OutputData outputData, object pageData);

        void SetPageMaker(ISource source, IInputData input, OutputData outputData, IPageMaker pageMaker);
    }
}