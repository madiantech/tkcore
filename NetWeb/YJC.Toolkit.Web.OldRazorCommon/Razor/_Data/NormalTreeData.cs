using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalTreeData
    {
        public NormalTreeData()
        {
            ShowUpDownButton = true;
            ShowTitle = true;
            IsNewRootDialog = true;
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

        public bool ShowTitle { get; set; }

        public bool ShowNewRootButton { get; set; }

        public bool ShowUpDownButton { get; set; }

        public bool CanMoveNode { get; set; }

        public bool UseWholeRow { get; set; }

        public int DialogHeight { get; set; }

        public bool IsNewRootDialog { get; set; }
    }
}
