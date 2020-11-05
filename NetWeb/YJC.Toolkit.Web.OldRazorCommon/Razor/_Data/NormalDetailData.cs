using System;
using System.Collections.Generic;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalDetailData : BootcssDetailData, ISupportDialog
    {
        public NormalDetailData()
        {
            IgnoreEmptyField = true;
            ShowTitle = true;
            NormalCancelCaption = RazorDataConst.NORMAL_CANCEL_CAPTION;
            DialogCancelCaption = RazorDataConst.DIALOG_CANCEL_CAPTION;
        }

        internal NormalDetailData(NormalDetailDataConfig config)
            : base(config)
        {
            DialogMode = config.DialogMode;
            IgnoreEmptyField = config.IgnoreEmptyField;
            ShowTitle = config.ShowTitle;
            NormalCancelCaption = config.NormalCancelCaption;
            DialogCancelCaption = config.DialogCancelCaption;
            DialogHeight = config.DialogHeight;
            ControlGroupList = config.ControlGroupList;
        }

        #region ISupportDialog 成员

        public void SetDialogMode(bool dialogMode)
        {
            ShowTitle = !dialogMode;
            DialogMode = dialogMode;
        }

        #endregion ISupportDialog 成员

        public bool DialogMode { get; set; }

        public bool ShowTitle { get; set; }

        public bool IgnoreEmptyField { get; set; }

        public string NormalCancelCaption { get; set; }

        public string DialogCancelCaption { get; set; }

        public int DialogHeight { get; set; }

        public string CancelCaption
        {
            get
            {
                return DialogMode ? DialogCancelCaption : NormalCancelCaption;
            }
        }

        public List<GroupSection> ControlGroupList { get; protected set; }

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