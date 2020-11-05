using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalEditData : BootcssEditData, ISupportDialog
    {
        public NormalEditData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalEditData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                ShowTitle = true;
                NormalCancelCaption = RazorDataConst.NORMAL_CANCEL_CAPTION;
                DialogCancelCaption = RazorDataConst.DIALOG_CANCEL_CAPTION;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
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

        [SimpleAttribute]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public string NormalCancelCaption { get; set; }

        [SimpleAttribute]
        public string DialogCancelCaption { get; set; }

        [SimpleAttribute]
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