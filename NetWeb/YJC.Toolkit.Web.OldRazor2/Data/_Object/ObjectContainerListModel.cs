using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal class ObjectContainerListModel : BaseObjectContainerModel, IListModel
    {
        private readonly IFieldValueProvider fEmptyProvider;
        private readonly ObjectListModel fSrcModel;

        public ObjectContainerListModel(ObjectListModel model)
            : base(model)
        {
            fSrcModel = model;
            fEmptyProvider = new ObjectContainerFieldValueProvider(null, null);

            YJC.Toolkit.Web.PageInfo info = model.CallerInfo.Info;
            PageStyle = info.Style.ToString();
            Source = info.Source;
            UrlInfo url = model.CallerInfo.URL;
            DecoderSelfUrl = url.DSelfUrl;
        }

        #region IListModel 成员

        public string PageStyle { get; private set; }

        public string Source { get; private set; }

        public bool HasListButtons
        {
            get
            {
                return fSrcModel.ListOperatorCount > 0;
            }
        }

        public IEnumerable<Operator> ListOperators
        {
            get
            {
                return fSrcModel.ListOperators;
            }
        }

        public CountInfo PageInfo
        {
            get
            {
                return fSrcModel.Count;
            }
        }

        public ListSortInfo SortInfo
        {
            get
            {
                return fSrcModel.Sort;
            }
        }

        public string DecoderSelfUrl { get; private set; }

        public IEnumerable<Operator> RowOperators
        {
            get
            {
                return fSrcModel.RowOperators;
            }
        }

        public IEnumerable<IOperatorFieldProvider> CreateDataRowsProvider(IListMetaData metaData)
        {
            if (fSrcModel.List == null)
                return Enumerable.Empty<IOperatorFieldProvider>();

            var result = from item in fSrcModel.List
                         select new ObjectContainerFieldValueProvider(item, null);
            return result;
        }

        public IFieldValueProvider CreateEmptyProvider()
        {
            return fEmptyProvider;
        }

        public IFieldValueProvider CreateQueryProvider()
        {
            return fEmptyProvider;
        }

        public IFieldValueProvider CreateProvider(string tableName)
        {
            return fEmptyProvider;
        }

        public IEnumerable<ListTabSheet> TabSheets
        {
            get
            {
                return null;
            }
        }

        #endregion IListModel 成员
    }
}