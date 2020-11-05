using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class UpdateObjectSource : BaseObjectSource<IUpdateObjectSource>
    {
        private readonly IUpdateObjectSource fSource;

        public UpdateObjectSource(IUpdateObjectSource source)
            : base(source)
        {
            fSource = source;
        }

        protected OutputData DoDetailAction(IInputData input)
        {
            EditObjectModel model = new EditObjectModel();
            object newObject = Source.Query(input, input.QueryString["Id"]);
            ObjectContainer container = new ObjectContainer(newObject);
            model.Object = container;
            IEnumerable<IFieldInfoEx> fields = GetFields();
            container.Decode(fields);
            model.FillCodeTable(fields);

            input.CallerInfo.AddInfo(model.CallerInfo);

            return OutputData.CreateObject(model);
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.Update)
            {
                if (input.IsPost)
                    try
                    {
                        return fSource.Update(input, input.PostObject);
                    }
                    catch (WebPostException ex)
                    {
                        return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                    }
                else
                    return DoDetailAction(input);
            }
            else
                return ErrorPageStyle(PageStyle.Update, input); ;
        }
    }
}
