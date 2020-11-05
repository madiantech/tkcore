using System;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalStatListData : NormalListData
    {
        private readonly RegNameList<StatFieldDisplayConfig> fDisplayList;

        public NormalStatListData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalStatListData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                SubTotalCaption = RazorDataConst.SUB_TOTAL_CAPTION;
                SubTotalPosition = DataDirection.Foot;
                TotalCaption = RazorDataConst.TOTAL_CAPTION;
                TotalPosition = DataDirection.Foot;
                OperatorPosition = OperatorPosition.None;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
            fDisplayList = new RegNameList<StatFieldDisplayConfig>();
        }

        internal NormalStatListData(NormalStatListDataConfig config)
            : base(config)
        {
            SubTotalCaption = config.SubTotalCaption;
            SubTotalPosition = config.SubTotalPosition;
            TotalCaption = config.TotalCaption;
            TotalPosition = config.TotalPosition;
            fDisplayList = config.FieldDisplayList;
            if (fDisplayList == null)
                fDisplayList = new RegNameList<StatFieldDisplayConfig>();
        }

        [SimpleAttribute]
        public string SubTotalCaption { get; set; }

        [SimpleAttribute]
        public DataDirection SubTotalPosition { get; set; }

        [SimpleAttribute]
        public string TotalCaption { get; set; }

        [SimpleAttribute]
        public DataDirection TotalPosition { get; set; }

        public IConfigCreator<IDisplay> GetDisplay(string nickName)
        {
            var item = fDisplayList[nickName];
            if (item != null)
                return item.Display;
            return null;
        }
    }
}