using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal abstract class BaseTreeRazorPageMakerConfig : BaseCompositeRazorPageMakerConfig
    {
        protected BaseTreeRazorPageMakerConfig()
        {
            DetailTemplateName = "NormalTreeDetail";
            TreeTemplateName = "NormalTree";
        }

        [SimpleAttribute]
        public bool ShowNewRootButton { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowUpDownButton { get; protected set; }

        [SimpleAttribute]
        public bool CanMoveNode { get; protected set; }

        [SimpleAttribute]
        public bool UseWholeRow { get; protected set; }

        [SimpleAttribute]
        public string TreeModelName { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsNewRootDialog { get; protected set; }

        public string TreeTemplateName { get; protected set; }

        private void SetTreeData(NormalTreeDataConfig treeData)
        {
            treeData.ShowNewRootButton = ShowNewRootButton;
            treeData.ShowUpDownButton = ShowUpDownButton;
            treeData.UseWholeRow = UseWholeRow;
            treeData.CanMoveNode = CanMoveNode;
            treeData.IsNewRootDialog = IsNewRootDialog;
        }

        private RazorPageMaker CreateTreePageMaker(OverrideItemConfig config, IPageData pageData)
        {
            RazorPageMakerConfig makerConfig = new RazorPageMakerConfig(TreeTemplateName, config);
            if (makerConfig.RazorData == null)
            {
                NormalTreeDataConfig treeData = new NormalTreeDataConfig();
                SetTreeData(treeData);
                makerConfig.RazorData = treeData;
            }
            else
            {
                NormalTreeDataConfig treeData = makerConfig.RazorData as NormalTreeDataConfig;
                if (treeData != null)
                    SetTreeData(treeData);
            }
            return new RazorPageMaker(makerConfig, pageData);
        }

        protected override IPageMaker CreateListPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            return new JsonObjectPageMaker(TreeModelName, null);
        }

        protected override IPageMaker CreateCustomPageMaker(OverrideItemConfig config,
            IPageData pageData, PageStyleClass style)
        {
            switch (style.Operation)
            {
                case TreeOperationSource.MOVE_NODE:
                case TreeOperationSource.MOVE_UP_DOWN:
                    return new PostPageMaker(ContentDataType.Json, PageStyle.Custom, new CustomUrlConfig());
                case "":
                case null:
                    return CreateTreePageMaker(config, pageData);
            }
            return base.CreateCustomPageMaker(config, pageData, style);
        }
    }
}
