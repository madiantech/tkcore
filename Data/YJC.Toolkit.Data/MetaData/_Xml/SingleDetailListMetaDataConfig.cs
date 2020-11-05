using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-10-25", NamespaceType = NamespaceType.Toolkit,
        Description = "在编辑和列表时，使用主表的MetaData，在详细页使用主从配置的MetaData")]
    [ObjectContext]
    internal class SingleDetailListMetaDataConfig : IConfigCreator<IMetaData>
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
                    masterMeta = Main.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5SingleNormalMetaData(scheme, input, masterMeta);

                case PageStyle.List:
                    masterMeta = Main.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, masterMeta);

                case PageStyle.Detail:
                    masterMeta = Main.CreateSingleMetaData();
                    return new Tk5MultipleMetaData(input, EnumUtil.Convert(masterMeta),
                        Details);

                case PageStyle.Custom:
                    if (Details == null)
                        return null;
                    if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
                    {
                        int index = input.QueryString["Index"].Value<int>();
                        if (index < Details.Count)
                        {
                            var detail = Details[index];
                            InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                            masterMeta = detail.CreateSingleMetaData();
                            scheme = masterMeta.CreateSourceScheme(input);
                            return new Tk5ListMetaData(scheme, proxy, masterMeta,
                                detail.TableOutput?.CreateObject());
                        }
                        return null;
                    }
                    break;
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MasterSingleMetaDataConfig Main { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Detail", Required = true)]
        public List<DetailSingleMetaDataConfig> Details { get; private set; }
    }
}