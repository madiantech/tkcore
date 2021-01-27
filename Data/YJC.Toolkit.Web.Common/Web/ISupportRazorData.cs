using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface ISupportRazorData
    {
        object GetRazorData(ISource source, IPageData pageData, OutputData outputData);
    }
}