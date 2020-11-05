using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-06-01",
        NamespaceType = NamespaceType.Toolkit, Description = "多主表多从表的MetaData")]
    [ObjectContext]
    internal class MultipleMDMetaDataConfig : IConfigCreator<IMetaData>, IReadObjectCallBack
    {
        private MasterSingleMetaDataConfig fMaster;

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
                    var masters = from item in MasterItems
                                  select item.CreateSingleMetaData();
                    return new Tk5MultipleMetaData(input, masters, DetailItems);
                case PageStyle.List:
                    masterMeta = fMaster.CreateSingleMetaData();
                    scheme = masterMeta.CreateSourceScheme(input);
                    return new Tk5ListMetaData(scheme, input, masterMeta);
                case PageStyle.Custom:
                    bool shouldReturn;
                    IMetaData result = MultipleDetailMetaDataConfig.CreateDetailListMeta(input, 
                        DetailItems, out shouldReturn);
                    if (shouldReturn)
                        return result;

                    break;
            }
            return null;
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (MasterItems.Count == 1)
                fMaster = MasterItems[0];
            else
            {
                var masters = (from item in MasterItems
                               where item.Main
                               select item).ToArray();
                TkDebug.Assert(masters.Length == 1, "", this);
                fMaster = masters[0];
            }
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Master", Required = true)]
        public List<MasterSingleMetaDataConfig> MasterItems { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Detail", Required = true)]
        public List<DetailSingleMetaDataConfig> DetailItems { get; private set; }
    }
}
