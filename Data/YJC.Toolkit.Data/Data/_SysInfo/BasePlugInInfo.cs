using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class BasePlugInInfo
    {
        internal BasePlugInInfo()
        {
        }

        internal BasePlugInInfo(string regName, BasePlugInAttribute attr)
        {
            RegName = regName;
            Author = attr.Author;
            CreateDate = attr.CreateDate;
            Description = attr.Description;
            Suffix = attr.Suffix;
        }

        [SimpleAttribute]
        public string RegName { get; protected set; }

        [SimpleAttribute]
        public string Author { get; protected set; }

        [SimpleAttribute]
        public string CreateDate { get; protected set; }

        [SimpleAttribute]
        public string Description { get; protected set; }

        [SimpleAttribute]
        public string Suffix { get; protected set; }

    }
}
