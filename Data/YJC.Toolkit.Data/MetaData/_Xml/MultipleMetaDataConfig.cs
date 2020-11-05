using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-07-30",
        NamespaceType = NamespaceType.Toolkit, Description = "多表的MetaData")]
    [ObjectContext]
    internal class MultipleMetaDataConfig : IConfigCreator<IMetaData>
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
                    IEnumerable<ISingleMetaData> oneToOneMeta;
                    if (OneToOneTables == null)
                        oneToOneMeta = null;
                    else
                        oneToOneMeta = (from item in OneToOneTables
                                        select item.CreateSingleMetaData());
                    var data = new Tk5MultipleMetaData(input,
                        EnumUtil.Convert(masterMeta, oneToOneMeta), OneToManyTables);
                    return data;

                case PageStyle.List:
                    masterMeta = Master.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, masterMeta);

                case PageStyle.Custom:
                    if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
                    {
                        int index = input.QueryString["Index"].Value<int>();
                        TkDebug.Assert(OneToManyTables != null && OneToManyTables.Count > index,
                            string.Format(ObjectUtil.SysCulture, "第{0}项metaData配置不存在", index + 1), this);
                        var config = OneToManyTables[index];
                        var detailMetaData = config.CreateSingleMetaData();
                        scheme = detailMetaData.CreateSourceScheme(input);
                        InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                        Tk5ListMetaData meta = new Tk5ListMetaData(scheme, proxy, detailMetaData,
                            config.TableOutput?.CreateObject());
                        return meta;
                    }
                    break;
            }
            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        [ObjectElement(NamespaceType = NamespaceType.Toolkit)]
        public MasterSingleMetaDataConfig Master { get; private set; }

        [ObjectElement(NamespaceType = NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoOTable")]
        public List<DetailSingleMetaDataConfig> OneToOneTables { get; private set; }

        [ObjectElement(NamespaceType = NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoMTable")]
        public List<DetailSingleMetaDataConfig> OneToManyTables { get; private set; }
    }
}