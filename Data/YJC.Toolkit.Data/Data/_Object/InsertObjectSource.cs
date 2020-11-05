using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class InsertObjectSource : BaseObjectSource<IInsertObjectSource>
    {
        public InsertObjectSource(IInsertObjectSource source)
            : base(source)
        {
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.Insert)
            {
                if (input.IsPost)
                    try
                    {
                        return Source.Insert(input, input.PostObject);
                    }
                    catch (WebPostException ex)
                    {
                        return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                    }
                else
                {
                    EditObjectModel model = new EditObjectModel();
                    object newObject = Source.CreateNew(input);
                    ObjectContainer container = new ObjectContainer(newObject);
                    model.Object = container;
                    IEnumerable<IFieldInfoEx> fields = GetFields();
                    container.Decode(fields);
                    model.FillCodeTable(fields);

                    input.CallerInfo.AddInfo(model.CallerInfo);

                    return OutputData.CreateObject(model);
                }
            }
            else
                return ErrorPageStyle(PageStyle.Insert, input);
        }
    }
}
