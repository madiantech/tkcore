using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class JsonResultPageMaker : BaseObjectPageMaker
    {
        private readonly JsonDataSetResultPageMaker fChild;

        private JsonResultPageMaker()
        {
            fChild = new JsonDataSetResultPageMaker();
            Add(PageMakerUtil.IsType(SourceOutputType.XmlReader, SourceOutputType.DataSet), fChild);
        }

        public JsonResultPageMaker(ActionResultData result)
            : this()
        {
            TkDebug.AssertArgumentNull(result, "result", null);

            Result = result;
        }

        internal JsonResultPageMaker(JsonResultPageMakerAttribute attr)
            : this()
        {
            SetFormat(attr);
            switch (attr.Result)
            {
                case ActionResult.Success:
                    Result = ActionResultData.CreateSuccessResult(attr.Message);
                    break;
                case ActionResult.ReLogOn:
                    Result = ActionResultData.CreateReLogOnResult(attr.Message);
                    break;
                case ActionResult.Error:
                    Result = ActionResultData.CreateErrorResult(attr.Message);
                    break;
                case ActionResult.Fail:
                    Result = ActionResultData.CreateFailResult(attr.Message);
                    break;
            }
        }

        internal JsonResultPageMaker(JsonResultPageMakerConfig config)
            : this()
        {
            SetFormat(config);
            Result = config.Result;
            if (config.TableMappings != null)
            {
                foreach (var mapping in config.TableMappings)
                {
                    if (mapping.RemoveFields != null)
                    {
                        foreach (string field in mapping.RemoveFields)
                            fChild.AddRemoveField(mapping.TableName, field);
                    }
                }
            }

            if (config.RemoveTables != null)
            {
                foreach (string table in config.RemoveTables)
                    fChild.AddRemoveTable(table);
            }
        }

        public ActionResultData Result
        {
            get
            {
                return fChild.Result;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                fChild.Result = value;
            }
        }

        public void AddRemoveTableChild(string tableName)
        {
            fChild.AddRemoveTable(tableName);
        }

        public bool RemoveRemoveTableChild(string tableName)
        {
            return fChild.RemoveRemoveTable(tableName);
        }

        public void AddRemoveFieldChild(string tableName, string fieldName)
        {
            fChild.AddRemoveField(tableName, fieldName);
        }

        public bool RemoveRemoveFieldChild(string tableName, string fieldName)
        {
            return fChild.RemoveRemoveField(tableName, fieldName);
        }
    }
}
