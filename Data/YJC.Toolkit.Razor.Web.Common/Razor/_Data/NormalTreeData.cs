using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalTreeData
    {
        public NormalTreeData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                 nameof(NormalTreeData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                ShowUpDownButton = true;
                ShowTitle = true;
                IsNewRootDialog = true;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
        }

        internal NormalTreeData(NormalTreeDataConfig config)
        {
            ShowUpDownButton = config.ShowUpDownButton;
            ShowNewRootButton = config.ShowNewRootButton;
            CanMoveNode = config.CanMoveNode;
            UseWholeRow = config.UseWholeRow;
            ShowTitle = config.ShowTitle;
            IsNewRootDialog = config.IsNewRootDialog;
        }

        [SimpleAttribute]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool ShowNewRootButton { get; set; }

        [SimpleAttribute]
        public bool ShowUpDownButton { get; set; }

        [SimpleAttribute]
        public bool CanMoveNode { get; set; }

        [SimpleAttribute]
        public bool UseWholeRow { get; set; }

        [SimpleAttribute]
        public int DialogHeight { get; set; }

        [SimpleAttribute]
        public bool IsNewRootDialog { get; set; }
    }
}