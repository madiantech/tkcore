using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2017-11-01",
        NamespaceType = NamespaceType.Toolkit, Description = "基于Tk5的DataXml的单表MetaData")]
    [SingleMetaDataConfig(Author = "YJC", CreateDate = "2017-11-01",
        NamespaceType = NamespaceType.Toolkit, Description = "基于Tk5的DataXml的单表MetaData")]
    [ObjectContext]
    internal class SingleSchemaMetaDataConfig : BaseSingleMetaDataConfig, IConfigCreator<IMetaData>,
        IConfigCreator<ISingleMetaData>
    {
        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(TableSchemeExConfigFactory.REG_NAME)]
        public IConfigCreator<ITableSchemeEx> Scheme { get; private set; }

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

        ISingleMetaData IConfigCreator<ISingleMetaData>.CreateObject(params object[] args)
        {
            return this;
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "原始Scheme是{0}的元数据", Scheme);
        }

        public override ITableSchemeEx CreateSourceScheme(IInputData input)
        {
            return Scheme.CreateObject();
        }

        public override Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input)
        {
            return new Tk5TableScheme(scheme, input, this, SchemeUtil.CreateDataXmlField);
        }
    }
}