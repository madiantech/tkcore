using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class BootcssEditData : BaseBootcssData
    {
        private int fDataColumn;

        public BootcssEditData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(BootcssEditData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                CaptionColumn = RazorDataConst.CAPTION_COLUMN;
                fDataColumn = RazorDataConst.CONTROL_COLUMN;
                SaveButtonCaption = RazorDataConst.SAVE_BUTTON_CAPTION;
                FormAction = RazorDataConst.FORM_ACTION;
                InsertFormat = RazorDataConst.INSERT_FORMAT;
                EditFormat = RazorDataConst.EDIT_FORMAT;
                ShowCaption = true;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
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

        [SimpleAttribute(DefaultValue = RazorDataConst.CAPTION_COLUMN)]
        public int CaptionColumn { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.CONTROL_COLUMN)]
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

        [SimpleAttribute]
        public bool DialogMode { get; set; }

        [SimpleAttribute]
        public string SaveButtonCaption { get; set; }

        [SimpleAttribute]
        public string FormAction { get; set; }

        [SimpleAttribute]
        public bool ShowCaption { get; set; }

        [SimpleAttribute]
        public string EditFormat { get; set; }

        [SimpleAttribute]
        public string InsertFormat { get; set; }

        [ObjectElement(IsMultiple = true)]
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