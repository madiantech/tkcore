using System;
using System.Collections.Generic;
using System.Dynamic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TreeOperationSource : BaseDbSource
    {
        private const string NEW_CHILD = "NewChild";
        public const string MOVE_NODE = "MoveNode";
        public const string MOVE_UP_DOWN = "MoveUpDown";
        private ITree fTree;

        protected TreeOperationSource()
        {
        }

        internal TreeOperationSource(ITreeCreator creator)
        {
            if (!string.IsNullOrEmpty(creator.Context))
                Context = DbContextUtil.CreateDbContext(creator.Context);

            Tree = creator.CreateTree(this);
        }

        public ITree Tree
        {
            get
            {
                return fTree;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                fTree = value;
                TreeOperation = fTree as ITreeOperation;
                if (TreeOperation == null)
                {
                    ITreeOperationCreator creator = fTree as ITreeOperationCreator;
                    TkDebug.AssertNotNull(creator,
                        "ITree需要支持ITreeOperation或者ITreeOperationCreator接口，才能支持Tree操作",
                        fTree);

                    TreeOperation = creator.CreateOperation();
                }
            }
        }

        public ITreeOperation TreeOperation { get; private set; }

        private static string GetQueryStringText(IInputData input, string startChar)
        {
            var queryString = input.QueryStringText;
            if (string.IsNullOrEmpty(queryString))
                return string.Empty;
            Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            dict.ReadQueryString(queryString);
            dict.Remove("InitValue");
            dict.Remove("RetUrl");
            if (dict.Count == 0)
                return string.Empty;
            return startChar + dict.WriteQueryString();
        }

        public override OutputData DoAction(IInputData input)
        {
            try
            {
                object result;
                string source = input.SourceInfo.Source;
                string queryStringText;
                switch (input.Style.Operation)
                {
                    case NEW_CHILD:
                        CheckOperation(Data.TreeOperation.NewChild, "新建子节点");
                        queryStringText = GetQueryStringText(input, "&");
                        string parentId = input.QueryString[TreeOperation.IdFieldName];
                        if (string.IsNullOrEmpty(parentId))
                            parentId = TreeOperation.RootId;
                        string retUrl = input.QueryString["RetUrl"];
                        if (string.IsNullOrEmpty(retUrl))
                            retUrl = string.Empty;
                        else
                            retUrl = "&RetUrl=" + Uri.EscapeDataString(retUrl);
                        string childUrl = string.Format(ObjectUtil.SysCulture,
                            "~/c/xml/insert/{0}?{1}={2}{4}{3}",
                            source, TreeOperation.ParentFieldName, parentId, retUrl, queryStringText);
                        return OutputData.Create(childUrl);

                    case "":
                    case null:
                        queryStringText = GetQueryStringText(input, "?");
                        dynamic data = new ExpandoObject();
                        data.DetailUrl = string.Format(ObjectUtil.SysCulture, "c/xml/detail/{0}{1}", source, queryStringText);
                        data.ListUrl = string.Format(ObjectUtil.SysCulture, "c/xml/list/{0}{1}", source, queryStringText);
                        data.Source = source;
                        data.IdField = TreeOperation.IdFieldName;
                        data.ParentIdField = TreeOperation.ParentFieldName;
                        data.RootId = TreeOperation.RootId;
                        input.CallerInfo.AddInfo(data);
                        return OutputData.CreateObject(data);

                    case MOVE_NODE:
                        CheckOperation(Data.TreeOperation.MoveNode, "移动节点");
                        result = TreeOperation.MoveTreeNode(input.QueryString["SourceId"], input.QueryString["DestId"]);
                        return OutputData.CreateToolkitObject(result);

                    case MOVE_UP_DOWN:
                        CheckOperation(Data.TreeOperation.MoveUpDown, "上移和下移");
                        TreeNodeMoveDirection direction = input.QueryString["direction"]
                            .Value<TreeNodeMoveDirection>();
                        result = TreeOperation.MoveUpDown(input.QueryString["Id"], direction);
                        return OutputData.CreateToolkitObject(result);

                    default:
                        TkDebug.ThrowToolkitException("当前的Source不支持该操作", this);
                        return null;
                }
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }

        private void CheckOperation(TreeOperation current, string operation)
        {
            if ((current & TreeOperation.Support) != current)
                throw new WebPostException(string.Format(ObjectUtil.SysCulture,
                    "当前配置的TreeOperation不支持{0}操作", operation));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                fTree.DisposeObject();

            base.Dispose(disposing);
        }
    }
}