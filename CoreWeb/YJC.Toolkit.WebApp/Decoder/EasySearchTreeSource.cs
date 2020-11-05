using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [JsonObjectPageMaker]
    [Source(Author = "YJC", CreateDate = "2014-08-21", 
        Description = "提供树形EasySearch各级节点的数据")]
    [WebPage(SupportLogOn = false)]
    class EasySearchTreeSource : TreeSource
    {
        public override OutputData DoAction(IInputData input)
        {
            string regName = input.QueryString["RegName"];
            EasySearch easySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                EasySearchPlugInFactory.REG_NAME, regName);
            BaseDbEasySearch dbSearch = easySearch as BaseDbEasySearch;
            if (dbSearch != null)
            {
                if (!string.IsNullOrEmpty(dbSearch.ContextName))
                    Context = DbContextUtil.CreateDbContext(dbSearch.ContextName);
            }
            IConfigCreator<ITree> creator = easySearch as IConfigCreator<ITree>;
            TkDebug.AssertNotNull(creator, string.Format(ObjectUtil.SysCulture,
                "类型为{0}，注册名为{1}的EasySearch需要实现创建ITree的接口",
                easySearch.GetType(), regName), easySearch);
            Tree = creator.CreateObject(this);

            return base.DoAction(input);
        }
    }
}
