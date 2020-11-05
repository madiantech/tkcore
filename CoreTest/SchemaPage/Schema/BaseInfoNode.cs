using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolkit.SchemaSuite.Schema
{
    public abstract class BaseInfoNode : BaseNode
    {
        private readonly string fCaption;

        protected BaseInfoNode(BaseNode parentNode, string caption)
            : base(parentNode)             
        {
            fCaption = caption;
        }

        public override bool HasChildren
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return fCaption;
            }
        }

        public virtual IEnumerable<BaseNode> GetChildNodes()
        {
            return null;
        }
    }
}
