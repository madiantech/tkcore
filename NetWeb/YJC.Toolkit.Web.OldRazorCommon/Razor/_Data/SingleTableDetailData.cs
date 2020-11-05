using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class SingleTableDetailData : IRegName
    {
        [SimpleAttribute]
        public string TableName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true,
            LocalName = "RazorField", UseConstructor = true)]
        internal List<RazorField> RazorFields { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ControlGroup")]
        public List<GroupSection> ControlGroupList { get; protected set; }

        public string RegName
        {
            get
            {
                return TableName;
            }
        }

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