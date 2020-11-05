using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TreeSource : BaseDbSource
    {
        protected TreeSource()
        {
        }

        internal TreeSource(ITreeCreator creator)
        {
            if (!string.IsNullOrEmpty(creator.Context))
                Context = DbContextUtil.CreateDbContext(creator.Context);

            Tree = creator.CreateTree(this);
        }

        public ITree Tree { get; protected set; }

        public override OutputData DoAction(IInputData input)
        {
            IEnumerable<ITreeNode> nodes;
            string initValue = input.QueryString["InitValue"];
            if (string.IsNullOrEmpty(initValue))
            {
                string id = input.QueryString["id"];
                if (id == "#" || string.IsNullOrEmpty(id)) // Root
                    nodes = Tree.RootNodes;
                else
                    nodes = Tree.GetChildNodes(id);
            }
            else
                nodes = Tree.GetDisplayTreeNodes(initValue);

            return OutputData.CreateToolkitObject(nodes.ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Tree.DisposeObject();

            base.Dispose(disposing);
        }
    }
}
