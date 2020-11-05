using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-06-01",
        NamespaceType = NamespaceType.Toolkit, Description = "单主表多从表的MetaData")]
    [ObjectContext]
    internal class MultipleDetailMetaDataConfig : IConfigCreator<IMetaData>
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
                        DetailItems);
                //return new Tk5SingleNormalMetaData(scheme, input, this);
                //break;
                case PageStyle.List:
                    masterMeta = Master.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, masterMeta);

                case PageStyle.Custom:
                    bool shouldReturn;
                    IMetaData result = CreateDetailListMeta(input, DetailItems, out shouldReturn);
                    if (shouldReturn)
                        return result;
                    break;
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MasterSingleMetaDataConfig Master { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Detail", Required = true)]
        public List<DetailSingleMetaDataConfig> DetailItems { get; private set; }

        internal static IMetaData CreateDetailListMeta(IInputData input, List<DetailSingleMetaDataConfig> detailItems,
            out bool shouldReturn)
        {
            shouldReturn = false;

            if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
            {
                int index = input.QueryString["Index"].Value<int>(0);
                if (index < detailItems.Count)
                {
                    var detailMeta = detailItems[index].CreateSingleMetaData();
                    var scheme = detailMeta.CreateSourceScheme(input);
                    InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                    shouldReturn = true;
                    return new Tk5ListMetaData(scheme, proxy, detailMeta,
                        detailItems[index].TableOutput?.CreateObject());
                }
            }
            return null;
        }
    }
}