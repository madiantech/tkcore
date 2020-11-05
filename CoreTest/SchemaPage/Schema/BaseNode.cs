using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite.Schema
{
    public abstract class BaseNode : ITreeNode
    {
        internal static int IdSeed = 0;
        private readonly BaseNode fParentNode;
        private int fId;

        protected BaseNode(BaseNode parentNode)
        {
            fParentNode = parentNode;
        }

        #region ITreeNode 成员

        public virtual IEnumerable AllDescendants
        {
            get
            {
                return null;
            }
        }

        public virtual IEnumerable<ITreeNode> Children
        {
            get
            {
                return null;
            }
        }

        public virtual bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public ITreeNode Parent
        {
            get
            {
                return fParentNode;
            }
        }

        public string ParentId
        {
            get
            {
                if (fParentNode == null)
                    return "-1";
                return fParentNode.Id;
            }
        }

        public virtual bool HasParent { get => fParentNode != null; }

        #endregion ITreeNode 成员

        #region IEntity 成员

        public string Id
        {
            get
            {
                if (fId == 0)
                    fId = ++IdSeed;
                return fId.ToString();
            }
        }

        public abstract string Name { get; }

        #endregion IEntity 成员

        public virtual bool IsRequired
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsMultiple
        {
            get
            {
                return false;
            }
        }
    }
}