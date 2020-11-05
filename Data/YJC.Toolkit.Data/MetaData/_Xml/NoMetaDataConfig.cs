using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-08-10",
        NamespaceType = NamespaceType.Toolkit, Description = "没有MetaData")]
    internal class NoMetaDataConfig : IConfigCreator<IMetaData>
    {
        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
