using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class MetaDataPageMaker : IPageMaker, ISupportMetaData
    {
        private IMetaData fMetaData;

        public MetaDataPageMaker()
        {
            DataType = ContentDataType.Xml;
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            if (fMetaData == null)
                return CreateEmpty();
            else
            {
                object data = fMetaData.ToToolkitObject();
                if (data == null)
                    return CreateEmpty();

                if (DataType == ContentDataType.Xml)
                    return new SimpleContent(data.WriteXml());
                else
                    return new SimpleContent(ContentTypeConst.JSON, data.WriteJson());
            }
        }

        #endregion

        public ContentDataType DataType { get; set; }

        private static IContent CreateEmpty()
        {
            return new SimpleContent(ContentTypeConst.HTML, "No MetaData");
        }
    }
}
