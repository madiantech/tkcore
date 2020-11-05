using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessTableDataConfig : TableMetaDataConfig
    {
        [SimpleAttribute(DefaultValue = UpdateMode.OneRow)]
        public UpdateMode UpdateMode { get; private set; }

        [SimpleAttribute]
        public UpdateKind? Status { get; private set; }

        //[SimpleAttribute]
        //public bool IsFix { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseMeta { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> TableResolver { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "DefaultValue")]
        public List<DefaultValueConfig> DefaultValueList { get; private set; }

        public ResolverConfig CreateResolverConfig(WorkflowContent content, IDbDataSource source)
        {
            TableResolver resolver = TableResolver.CreateObject(source);
            TableResolver exits = content.GetTableResolver(resolver.TableName);
            if (exits != null)
                resolver = exits;
            UpdateKind kind;
            if (Status == null)
                kind = Action.ToString().Value<UpdateKind>();
            else
                kind = Status.Value;
            return new ProcessResolverConfig(this, resolver, Action, kind, UpdateMode)
            {
                UseMeta = UseMeta
            };
        }
    }
}