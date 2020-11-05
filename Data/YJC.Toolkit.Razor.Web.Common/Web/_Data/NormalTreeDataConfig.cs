using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-09-01",
        Author = "YJC", Description = "匹配NormalTree和NormalObjectTree模板使用的数据")]
    internal class NormalTreeDataConfig : IConfigCreator<object>
    {
        public NormalTreeDataConfig()
        {
            ShowTitle = ShowUpDownButton = IsNewRootDialog = true;
        }

        #region IConfigCreator<object> 成员

        public object CreateObject(params object[] args)
        {
            return new NormalTreeData(this);
        }

        #endregion IConfigCreator<object> 成员

        [SimpleAttribute]
        public bool ShowNewRootButton { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowUpDownButton { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool CanMoveNode { get; set; }

        [SimpleAttribute]
        public bool UseWholeRow { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsNewRootDialog { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_DIALOG_HEIGHT)]
        public int DialogHeight { get; set; }
    }
}