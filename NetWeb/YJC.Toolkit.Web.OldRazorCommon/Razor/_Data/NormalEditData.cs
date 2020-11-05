using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalEditData : BootcssEditData, ISupportDialog
    {
        public NormalEditData()
        {
            ShowTitle = true;
            NormalCancelCaption = RazorDataConst.NORMAL_CANCEL_CAPTION;
            DialogCancelCaption = RazorDataConst.DIALOG_CANCEL_CAPTION;
        }

        internal NormalEditData(NormalEditDataConfig config)
            : base(config)
        {
            ShowTitle = config.ShowTitle;
            NormalCancelCaption = config.NormalCancelCaption;
            DialogCancelCaption = config.DialogCancelCaption;
            ContainerType = config.ContainerType;
        }

        #region ISupportDialog 成员

        public void SetDialogMode(bool dialogMode)
        {
            ShowTitle = !dialogMode;
            DialogMode = dialogMode;
        }

        #endregion ISupportDialog 成员

        public bool ShowTitle { get; set; }

        public string NormalCancelCaption { get; set; }

        public string DialogCancelCaption { get; set; }

        public PageContainer ContainerType { get; set; }

        public string CancelCaption
        {
            get
            {
                return DialogMode ? DialogCancelCaption : NormalCancelCaption;
            }
        }
    }
}