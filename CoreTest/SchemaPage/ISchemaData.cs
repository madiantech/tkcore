using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    internal interface ISchemaData
    {
        void AddMetaData(SchemaTable schemaTable, PageData pageData);

        void AddData(Dictionary<string, object> data);
    }
}