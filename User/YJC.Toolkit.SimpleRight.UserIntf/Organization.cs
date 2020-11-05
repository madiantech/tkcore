using System.Collections;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class Organization : IOrganization
    {
        private readonly int fOrgIsLeaf;
        private readonly OrganizationProvider fProvider;

        private IEnumerable<IOrganization> fChildren;
        private bool fIsGetChildren;
        private IEnumerable<IUser> fUsers;
        private bool fIsGetUsers;
        private ITreeNode fParent;
        private bool fIsGetParent;

        public Organization(DataRow row, OrganizationProvider provider)
        {
            fProvider = provider;

            Id = row["Id"].ToString();
            Name = row["Name"].ToString();
            OrgCode = row["Code"].ToString();
            ParentId = row["ParentId"].ToString();
            fOrgIsLeaf = row["IsLeaf"].Value<int>();
            Layer = row["Layer"].ToString();
            HasChildren = fOrgIsLeaf != 1;
            HasParent = ParentId != provider.RootParentId;
        }

        [SimpleAttribute]
        public string Layer { get; private set; }

        #region IOrganization 成员

        public bool HasDepartment
        {
            get
            {
                return false;
            }
        }

        public bool IsDepartment
        {
            get
            {
                return false;
            }
        }

        [SimpleAttribute]
        public string OrgCode { get; private set; }

        public IEnumerable<IUser> Users
        {
            get
            {
                if (!fIsGetUsers)
                {
                    fUsers = fProvider.GetUsersInOrganization(OrgCode);
                    fIsGetUsers = true;
                }
                return fUsers;
            }
        }

        #endregion IOrganization 成员

        #region ITreeNode 成员

        public IEnumerable<ITreeNode> Children
        {
            get
            {
                if (!fIsGetChildren)
                {
                    fChildren = fProvider.GetChildren(Id);
                    fIsGetChildren = true;
                }
                return fChildren;
            }
        }

        [SimpleAttribute]
        public bool HasChildren { get; private set; }

        public ITreeNode Parent
        {
            get
            {
                if (!fIsGetParent)
                {
                    fParent = fProvider.GetOrgById(ParentId);
                    fIsGetParent = true;
                }
                return fParent;
            }
        }

        [SimpleAttribute]
        public string ParentId { get; private set; }

        [SimpleAttribute]
        public bool HasParent { get; private set; }

        #endregion ITreeNode 成员

        #region IEntity 成员

        [SimpleAttribute]
        public string Id { get; private set; }

        [SimpleAttribute]
        public string Name { get; private set; }

        #endregion IEntity 成员
    }
}