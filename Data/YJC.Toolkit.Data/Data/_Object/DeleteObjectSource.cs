using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DeleteObjectSource : BaseObjectSource<IDeleteObjectSource>
    {
        public DeleteObjectSource(IDeleteObjectSource source)
            : base(source)
        {
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.Delete)
                try
                {
                    return Source.Delete(input, input.QueryString["Id"]);
                }
                catch (WebPostException ex)
                {
                    return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                }
            else
                return ErrorPageStyle(PageStyle.Delete, input);
        }
    }
}
