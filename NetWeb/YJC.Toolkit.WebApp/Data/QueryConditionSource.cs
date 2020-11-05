using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [Source(Author = "YJC", CreateDate = "2014-03-13",
        Description = "将用户输入的查询条件转换成系统识别的数据格式并编码")]
    [SourceOutputPageMaker]
    [JsonObjectCreator(typeof(QueryConditionPostObject))]
    [WebPage(SupportLogOn = false)]
    internal class QueryConditionSource : BaseWebDbSource
    {
        protected override OutputData DoGet(IInputData input)
        {
            return OutputData.Create(string.Empty);
        }

        protected override OutputData DoPost(IInputData input)
        {
            QueryConditionPostObject postObject = input.PostObject.Convert<QueryConditionPostObject>();
            TableResolver resolver = PlugInFactoryManager.CreateInstance<TableResolver>(
                ResolverPlugInFactory.REG_NAME, postObject.Resolver, this);
            using (resolver)
            {
                MetaDataTableResolver metaResolver = resolver as MetaDataTableResolver;
                if (metaResolver != null)
                {
                    IParamBuilder builder = metaResolver.GetQueryCondition(postObject.Query);
                    if (builder != null)
                    {
                        QueryCondition condition = new QueryCondition(postObject.Query.Condition, builder);
                        return OutputData.Create(condition.ToEncodeString());
                    }
                }
                return OutputData.Create(string.Empty);
            }
        }
    }
}
