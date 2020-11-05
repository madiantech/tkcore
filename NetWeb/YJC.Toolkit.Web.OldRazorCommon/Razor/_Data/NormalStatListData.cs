using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Razor;
using System.Runtime.Serialization;
using YJC.Toolkit.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalStatListData : NormalListData
    {
        private readonly RegNameList<StatFieldDisplayConfig> fDisplayList;

        public NormalStatListData()
        {
            SubTotalCaption = RazorDataConst.SUB_TOTAL_CAPTION;
            SubTotalPosition = DataDirection.Foot;
            TotalCaption = RazorDataConst.TOTAL_CAPTION;
            TotalPosition = DataDirection.Foot;
            OperatorPosition = OperatorPosition.None;
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

        public string SubTotalCaption { get; set; }

        public DataDirection SubTotalPosition { get; set; }

        public string TotalCaption { get; set; }

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