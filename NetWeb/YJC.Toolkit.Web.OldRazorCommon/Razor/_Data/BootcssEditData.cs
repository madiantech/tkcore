using System;
using System.Collections.Generic;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class BootcssEditData : BaseBootcssData
    {
        private int fDataColumn;

        public BootcssEditData()
        {
            CaptionColumn = RazorDataConst.CAPTION_COLUMN;
            fDataColumn = RazorDataConst.CONTROL_COLUMN;
            SaveButtonCaption = RazorDataConst.SAVE_BUTTON_CAPTION;
            FormAction = RazorDataConst.FORM_ACTION;
            InsertFormat = RazorDataConst.INSERT_FORMAT;
            EditFormat = RazorDataConst.EDIT_FORMAT;
            ShowCaption = true;
        }

        internal BootcssEditData(BootcssEditDataConfig config)
        {
            CaptionColumn = config.CaptionColumn;
            ShowCaption = config.ShowCaption;
            DataColumn = config.DataColumn;
            SaveButtonCaption = config.SaveButtonCaption;
            FormAction = config.FormAction;
            EditFormat = config.EditFormat;
            InsertFormat = config.InsertFormat;
            DialogMode = config.DialogMode;
            ControlGroupList = config.ControlGroupList;
        }

        public int CaptionColumn { get; set; }

        public int DataColumn
        {
            get
            {
                if (ShowCaption)
                    return fDataColumn;
                else
                    return RazorDataConst.TOTAL_COLUMN;
            }
            set
            {
                fDataColumn = value;
            }
        }

        public bool DialogMode { get; set; }

        public string SaveButtonCaption { get; set; }

        public string FormAction { get; set; }

        public bool ShowCaption { get; set; }

        public string EditFormat { get; set; }

        public string InsertFormat { get; set; }

        public List<GroupSection> ControlGroupList { get; set; }

        public void Initialize()
        {
            if (ControlGroupList != null)
            {
                ControlGroupList.Sort();
                GroupSection.SetEndOrder(ControlGroupList);
            }
        }
    }
}