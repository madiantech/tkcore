using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-12-14",
        Author = "YJC", UseConstructor = true, Description = "标准操作定义")]
    public sealed class OperatorConfig : IConfigCreator<OperatorConfig>, IRegName
    {
        public static readonly OperatorConfig InsertOperator =
            new OperatorConfig(RightConst.INSERT, "新建", OperatorPosition.Global,
                RightConst.INSERT, null, "icon-plus", null);

        public static readonly OperatorConfig UpdateOperator =
            new OperatorConfig(RightConst.UPDATE, "修改", OperatorPosition.Row,
                RightConst.UPDATE, null, "icon-edit", null)
            { UseKey = true };

        public static readonly OperatorConfig DeleteOperator =
            new OperatorConfig(RightConst.DELETE, "删除", OperatorPosition.Row,
                RightConst.DELETE + ",AjaxUrl", "确认删除吗？", "icon-remove", null)
            { UseKey = true };

        public static readonly OperatorConfig InsertDialogOperator =
            new OperatorConfig(RightConst.INSERT, "新建", OperatorPosition.Global,
                RightConst.INSERT_DIALOG, null, "icon-plus", null);

        public static readonly OperatorConfig UpdateDialogOperator =
            new OperatorConfig(RightConst.UPDATE, "修改", OperatorPosition.Row,
                RightConst.UPDATE_DIALOG, null, "icon-edit", null)
            { UseKey = true };

        internal OperatorConfig()
        {
        }

        public OperatorConfig(string id, string caption, OperatorPosition position,
            string info, MarcoConfigItem content) :
            this(id, caption, position, info, null, null, content)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OperatorConfig class.
        /// </summary>
        public OperatorConfig(string id, string caption, OperatorPosition position, string info,
            string confirmData, string iconClass, MarcoConfigItem content)
        {
            TkDebug.AssertArgumentNullOrEmpty(id, "id", null);
            TkDebug.AssertArgumentNullOrEmpty(caption, "caption", null);

            Id = id;
            Caption = new MultiLanguageText(caption);
            Position = position;
            Info = info;
            if (!string.IsNullOrEmpty(confirmData))
                ConfirmData = new MultiLanguageText(confirmData);
            IconClass = iconClass;
            Content = content;
        }

        #region IConfigCreator<OperatorConfig> 成员

        public OperatorConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<OperatorConfig> 成员

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return Id;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute]
        public string Id { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Caption { get; private set; }

        [SimpleAttribute]
        public OperatorPosition Position { get; private set; }

        [SimpleAttribute]
        public string Info { get; internal set; }

        [SimpleAttribute]
        public bool UseKey { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText ConfirmData { get; private set; }

        [SimpleAttribute]
        public string IconClass { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem Content { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem DialogTitle { get; set; }

        public override string ToString()
        {
            if (Caption == null)
                return base.ToString();
            return Caption.ToString();
        }
    }
}