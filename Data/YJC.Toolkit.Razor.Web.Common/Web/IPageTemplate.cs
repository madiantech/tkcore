using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IPageTemplate
    {
        string GetTemplateFile(ISource source, IInputData input, OutputData outputData);

        object GetDefaultPageData(ISource source, IInputData input, OutputData outputData);

        string GetEngineName(ISource source, IInputData input, OutputData outputData);
    }
}