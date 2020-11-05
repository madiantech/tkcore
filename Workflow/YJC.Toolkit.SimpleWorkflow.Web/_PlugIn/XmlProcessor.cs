using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class XmlProcessor : Processor
    {
        public XmlProcessor(ProcessXml config)
        {
            XmlConfig = config;
            FillMode = config.FillMode.Value;
        }

        public ProcessXml XmlConfig { get; private set; }

        public override IEnumerable<ResolverConfig> CreateUpdateResolverConfigs(DataRow workflowRow)
        {
            if ((XmlConfig?.TableList?.Count ?? 0) == 0)
                return Enumerable.Empty<ResolverConfig>();
            var result = from item in XmlConfig.TableList
                         select item.CreateResolverConfig(Content, Source);
            return result;
        }

        public override bool AddContent()
        {
            int contentCount = XmlConfig?.ContentList?.Count ?? 0;
            if (contentCount > 0)
            {
                foreach (ProcessContentConfigItem content in XmlConfig.ContentList)
                {
                    //ContentItem contentItem = new ContentItem(false, regName);
                    foreach (ProcessRecordConfigItem record in content.RecordList)
                    {
                        //RecordItem recordItem = new RecordItem();
                        int count = record.KeyList.Count;
                        string[] keys = new string[count];
                        string[] values = new string[count];
                        int index = 0;
                        foreach (ProcessKeyConfigItem keyItem in record.KeyList)
                        {
                            string value = EvaluatorUtil.Execute(keyItem.Value, ("mainRow", Content.MainRow),
                                ("content", Content), ("source", Source)).ConvertToString();
                            keys[index] = keyItem.NickName;
                            values[index] = value;
                            index++;
                        }
                        TableResolver resolver = content.Resolver.CreateObject(Source);
                        Content.AddContentItem(content.OrderBy, resolver.TableName, content.Resolver,
                            content.FilterSql, keys, values);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}