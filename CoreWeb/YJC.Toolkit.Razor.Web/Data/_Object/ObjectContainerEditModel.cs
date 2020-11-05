using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal class ObjectContainerEditModel : BaseObjectContainerModel, IEditModel
    {
        private readonly EditObjectModel fSrcModel;

        protected ObjectContainerEditModel(BaseObjectModel model)
            : base(model)
        {
            PageInfo info = model.CallerInfo.Info;
            PageStyle = info.Style.ToString();
            RetUrl = GetDynamicRetUrl(model.CallerInfo);
        }

        public ObjectContainerEditModel(EditObjectModel model)
            : this((BaseObjectModel)model)
        {
            fSrcModel = model;
        }

        #region IEditModel 成员

        public string PageStyle { get; private set; }

        public string RetUrl { get; private set; }

        public IFieldValueProvider CreateMainObjectProvider(INormalTableData metaData)
        {
            return CreateMainObjectProvider(metaData.TableName);
        }

        public IFieldValueProvider CreateMainObjectProvider(string tableName)
        {
            BaseObjectModel model = Model.Convert<BaseObjectModel>();
            return new ObjectContainerFieldValueProvider(model.Object,
                fSrcModel == null ? null : fSrcModel.CodeTables);
        }

        public IEnumerable<IFieldValueProvider> CreateDataRowsProvider(INormalTableData tableData)
        {
            return Enumerable.Empty<IFieldValueProvider>();
        }

        public IEnumerable<IFieldValueProvider> CreateDataRowsProvider(string tableName)
        {
            return Enumerable.Empty<IFieldValueProvider>();
        }

        #endregion IEditModel 成员

        private static string GetDynamicRetUrl(dynamic callerInfo)
        {
            string homePage = WebAppSetting.WebCurrent.HomePath;
            if (callerInfo == null)
                return homePage;

            UrlInfo url = callerInfo.URL;
            string retUrl = url != null ? url.DRetUrl : null;
            if (string.IsNullOrEmpty(retUrl))
                return homePage;
            return WebUtil.ResolveUrl(retUrl);
        }
    }
}