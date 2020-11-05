using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DetailObjectSource : BaseObjectSource<IDetailObjectSource>
    {
        public DetailObjectSource(IDetailObjectSource source)
            : base(source)
        {
        }
        public IObjectOperatorsConfig Operators { get; set; }

        private void MakeOperateRight(DetailObjectModel model, IInputData input)
        {
            if (Operators == null)
                return;

            IEnumerable<Operator> listOpertors = null;
            var operateRight = Operators.Right.CreateObject();
            if (operateRight == null)
            {
                var allOpertors = Operators.Operators;
                if (allOpertors != null)
                    listOpertors = from item in allOpertors
                                   select new Operator(item, this, input);
            }
            else
            {
                var rights = operateRight.GetOperator(
                    new ObjectOperateRightEventArgs(input.Style, model.Object));
                var allOpertors = Operators.Operators;
                if (rights != null && allOpertors != null)
                    listOpertors = from item in allOpertors
                                   join right in rights on item.Id equals right
                                   select new Operator(item, this, input);
            }
            if (listOpertors != null)
                model.DetailOperators = listOpertors;
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.Detail)
                return DoDetailAction(input);
            else
                return ErrorPageStyle(PageStyle.Detail, input);
        }

        protected OutputData DoDetailAction(IInputData input)
        {
            DetailObjectModel model = new DetailObjectModel();
            object newObject = Source.Query(input, input.QueryString["Id"]);
            ObjectContainer container = new ObjectContainer(newObject);
            model.Object = container;
            container.Decode(GetFields());
            MakeOperateRight(model, input);
            input.CallerInfo.AddInfo(model.CallerInfo);

            return OutputData.CreateObject(model);
        }
    }
}
