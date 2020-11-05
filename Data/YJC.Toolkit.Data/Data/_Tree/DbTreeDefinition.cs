using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class DbTreeDefinition
    {
        public const string ID_FIELD = "Id";
        public const string PARENT_ID_FIELD = "ParentId";
        public const string NAME_FIELD = "Name";
        public const string LEAF_FIELD = "IsLeaf";
        public const string LAYER_FIELD = "Layer";
        public const string DEFAULT_PARENT_VALUE = "-1";
        private const string DEFAULT_EXEC_PARENT_VALUE = "'-1'";
        private string fExecuteRootId;

        public DbTreeDefinition()
        {
            IdField = ID_FIELD;
            ParentIdField = PARENT_ID_FIELD;
            NameField = NAME_FIELD;
            LeafField = LEAF_FIELD;
            LayerField = LAYER_FIELD;
            RootId = DEFAULT_EXEC_PARENT_VALUE;
            SearchType = RootSearchType.ParentId;
        }

        public DbTreeDefinition(string idField, string nameField, string parentIdField,
            string leafField, string layerField) :
            this(idField, nameField, parentIdField, leafField, layerField, null, RootSearchType.ParentId)
        {
        }

        public DbTreeDefinition(string idField, string nameField, string parentIdField,
            string leafField, string layerField, string rootIdValue, RootSearchType searchType)
        {
            IdField = string.IsNullOrEmpty(idField) ? ID_FIELD : idField;
            ParentIdField = string.IsNullOrEmpty(parentIdField) ? PARENT_ID_FIELD : parentIdField;
            NameField = string.IsNullOrEmpty(nameField) ? NAME_FIELD : nameField;
            LeafField = string.IsNullOrEmpty(leafField) ? LEAF_FIELD : leafField;
            LayerField = string.IsNullOrEmpty(layerField) ? LAYER_FIELD : layerField;
            RootId = string.IsNullOrEmpty(rootIdValue) ? DEFAULT_EXEC_PARENT_VALUE : rootIdValue;
            SearchType = searchType;
        }

        [SimpleAttribute(DefaultValue = ID_FIELD)]
        public string IdField { get; private set; }

        [SimpleAttribute(DefaultValue = PARENT_ID_FIELD)]
        public string ParentIdField { get; private set; }

        [SimpleAttribute(DefaultValue = NAME_FIELD)]
        public string NameField { get; private set; }

        [SimpleAttribute(DefaultValue = LEAF_FIELD)]
        public string LeafField { get; private set; }

        [SimpleAttribute(DefaultValue = LAYER_FIELD)]
        public string LayerField { get; private set; }

        [SimpleAttribute(DefaultValue = DEFAULT_EXEC_PARENT_VALUE)]
        internal string RootId { get; private set; }

        [SimpleAttribute(DefaultValue = RootSearchType.ParentId)]
        public RootSearchType SearchType { get; private set; }

        public string ExecuteRootId
        {
            get
            {
                if (fExecuteRootId == null)
                {
                    if (!string.IsNullOrEmpty(RootId))
                        fExecuteRootId = EvaluatorUtil.Execute(RootId).ConvertToString();
                }
                return fExecuteRootId;
            }
        }
    }
}