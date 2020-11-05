using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2014-10-21",
        NamespaceType = NamespaceType.Toolkit, Description = "支持单表多记录编辑的单表MetaData")]
    [SingleMetaDataConfig(Author = "YJC", CreateDate = "2014-10-21",
        NamespaceType = NamespaceType.Toolkit, Description = "支持单表多记录编辑的单表MetaData")]
    [ObjectContext]
    internal class DetailMetaDataConfig : DetailSingleMetaDataConfig, IConfigCreator<IMetaData>,
        IConfigCreator<ISingleMetaData>
    {
        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Detail:
                    return new Tk5MultipleMetaData(input, this);

                case PageStyle.List:
                    var singleMetaData = CreateSingleMetaData();
                    var scheme = singleMetaData.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, singleMetaData);
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        #region IConfigCreator<ISingleMetaData> 成员

        ISingleMetaData IConfigCreator<ISingleMetaData>.CreateObject(params object[] args)
        {
            return CreateSingleMetaData();
        }

        #endregion IConfigCreator<ISingleMetaData> 成员
    }
}