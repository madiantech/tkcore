using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class SingleTableEditData : IRegName
    {
        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string OnRowAdded { get; private set; }

        [SimpleAttribute]
        public string OnRowDeleted { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public RazorOutputData RowDisplay { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true,
            LocalName = "RazorField", UseConstructor = true)]
        internal List<RazorField> RazorFields { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ControlGroup")]
        public List<GroupSection> ControlGroupList { get; protected set; }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return TableName;
            }
        }

        #endregion IRegName 成员

        public void Initialize()
        {
            if (ControlGroupList != null)
            {
                ControlGroupList.Sort();
                GroupSection.SetEndOrder(ControlGroupList);
            }
        }
    }
}