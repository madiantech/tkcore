using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(CreateDate = "2014-06-06", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "标记了TypeSchemeAttribute类型的单表MetaData")]
    [SingleMetaDataConfig(CreateDate = "2013-11-06", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "标记了TypeSchemeAttribute类型的单表MetaData")]
    [ObjectContext]
    internal class SingleClassMetaDataConfig : BaseClassMetaDataConfig, IConfigCreator<IMetaData>,
        IConfigCreator<ISingleMetaData>
    {
        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            ITableSchemeEx scheme = CreateSourceScheme(input);
            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Detail:
                    return new Tk5SingleNormalMetaData(scheme, input, this);

                case PageStyle.List:
                    return new Tk5ListMetaData(scheme, input, this);

                case PageStyle.Custom:
                    if (input.Style.Operation == "DetailList")
                    {
                        InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                        return new Tk5ListMetaData(scheme, proxy, this);
                    }
                    else
                    {
                        var metaData = SchemeUtil.CreateVueMetaData(input, scheme, this);
                        if (metaData != null)
                            return metaData;
                    }
                    break;
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        #region IConfigCreator<ISingleMetaData> 成员

        ISingleMetaData IConfigCreator<ISingleMetaData>.CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<ISingleMetaData> 成员

        public override string ToString()
        {
            return string.IsNullOrEmpty(ClassRegName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "类注册名为{0}的元数据", ClassRegName);
        }
    }
}