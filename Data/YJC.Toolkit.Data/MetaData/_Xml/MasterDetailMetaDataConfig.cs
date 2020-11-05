using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2014-09-05",
        NamespaceType = NamespaceType.Toolkit, Description = "主从表的MetaData")]
    [ObjectContext]
    internal class MasterDetailMetaDataConfig : IConfigCreator<IMetaData>
    {
        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            ITableSchemeEx scheme;
            ISingleMetaData masterMeta;
            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Detail:
                    masterMeta = Master.CreateSingleMetaData();
                    return new Tk5MultipleMetaData(input, EnumUtil.Convert(masterMeta),
                        EnumUtil.Convert(Detail));
                //return new Tk5SingleNormalMetaData(scheme, input, this);
                //break;
                case PageStyle.List:
                    masterMeta = Master.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, masterMeta);

                case PageStyle.Custom:
                    if (input.Style.Operation == "DetailList")
                    {
                        var detailMeta = Detail.CreateSingleMetaData();
                        scheme = detailMeta.CreateSourceScheme(input);
                        InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                        return new Tk5ListMetaData(scheme, proxy, detailMeta, Detail.TableOutput?.CreateObject());
                    }
                    var metaData = SchemeUtil.CreateVueMetaData(input, Master, EnumUtil.Convert(Detail));
                    if (metaData != null)
                        return metaData;
                    break;
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MasterSingleMetaDataConfig Master { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public DetailSingleMetaDataConfig Detail { get; private set; }
    }
}