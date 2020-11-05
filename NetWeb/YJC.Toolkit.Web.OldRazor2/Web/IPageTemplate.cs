using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IPageTemplate
    {
        string GetTemplateFile(ISource source, IInputData input, OutputData outputData);

        object GetDefaultPageData(ISource source, IInputData input, OutputData outputData);

        Type GetRazorTempate(ISource source, IInputData input, OutputData outputData);
    }
}