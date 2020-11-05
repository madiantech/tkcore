using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal class ObjectContainerDetailModel : ObjectContainerEditModel, IDetailModel
    {
        private readonly DetailObjectModel fSrcModel;

        public ObjectContainerDetailModel(DetailObjectModel model)
            : base(model)
        {
            fSrcModel = model;
            PageInfo info = model.CallerInfo.Info;
            Source = info.Source;
        }

        #region IDetailModel 成员

        public IEnumerable<Operator> DetailOperators
        {
            get
            {
                return fSrcModel.DetailOperators;
            }
        }

        public string Source { get; private set; }

        #endregion IDetailModel 成员
    }
}