using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalDetailData : BootcssDetailData, ISupportDialog
    {
        public NormalDetailData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalDetailData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                IgnoreEmptyField = true;
                ShowTitle = true;
                NormalCancelCaption = RazorDataConst.NORMAL_CANCEL_CAPTION;
                DialogCancelCaption = RazorDataConst.DIALOG_CANCEL_CAPTION;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
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

        [SimpleAttribute]
        public bool DialogMode { get; set; }

        [SimpleAttribute]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool IgnoreEmptyField { get; set; }

        [SimpleAttribute]
        public string NormalCancelCaption { get; set; }

        [SimpleAttribute]
        public string DialogCancelCaption { get; set; }

        [SimpleAttribute]
        public int DialogHeight { get; set; }

        public string CancelCaption
        {
            get
            {
                return DialogMode ? DialogCancelCaption : NormalCancelCaption;
            }
        }

        [ObjectElement(IsMultiple = true)]
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