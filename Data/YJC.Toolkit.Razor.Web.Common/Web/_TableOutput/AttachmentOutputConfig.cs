using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [TableOutputConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2020-07-10",
        Description = "针对多附件上传（专门存储附件的子表）")]
    internal class AttachmentOutputConfig : IConfigCreator<ITableOutput>
    {
        public ITableOutput CreateObject(params object[] args)
        {
            return new AttachmentOutput(DirectShowDetail)
            {
                IsMultiple = IsMultiple,
            };
        }

        [SimpleAttribute]
        public bool IsMultiple { get; set; }

        [SimpleAttribute]
        public bool DirectShowDetail { get; private set; }
    }
}