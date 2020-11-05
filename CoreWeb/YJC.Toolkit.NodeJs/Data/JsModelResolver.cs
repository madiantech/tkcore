using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class JsModelResolver : TableResolver
    {
        private static readonly string[] SearchFields = new string[] { "Model", "Template" };

        public JsModelResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("JsModel.xml"), source)
        {
        }

        public int GetVersion(string modelName, string templateName, List<FileCacheInfo> list)
        {
            DataRow row = TrySelectRowWithParams(SearchFields, modelName, templateName);
            if (row == null)
            {
                SetCommands(AdapterCommand.Insert);
                row = NewRow();
                row.BeginEdit();
                row["Id"] = CreateUniId();
                row["Model"] = modelName;
                row["Template"] = templateName;
                row["CacheInfo"] = list.WriteJson();
                row["Version"] = 1;
                row.EndEdit();
                UpdateDatabase();
                return 1;
            }
            else
            {
                int version = row["Version"].Value<int>();
                List<FileCacheInfo> oldList = new List<FileCacheInfo>();
                oldList.ReadJson(row["CacheInfo"].ToString());
                if (oldList.SequenceEqual(list))
                    return version;

                SetCommands(AdapterCommand.Update);
                row.BeginEdit();
                row["CacheInfo"] = list.WriteJson();
                row["Version"] = ++version;
                row.EndEdit();
                UpdateDatabase();
                return version;
            }
        }
    }
}