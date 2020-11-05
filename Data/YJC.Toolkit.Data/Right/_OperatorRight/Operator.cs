using System.Collections.Generic;
using System.Data;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class Operator
    {
        private const string ID_STR = "Id=*Id*";

        internal Operator()
        {
        }

        private Operator(OperatorConfig config)
        {
            Id = config.Id;
            Caption = config.Caption.ToString(ObjectUtil.SysCulture);
            Info = config.Info;
            ConfirmData = config.ConfirmData == null ? null
                : config.ConfirmData.ToString(ObjectUtil.SysCulture);
            IconClass = config.IconClass;
        }

        internal Operator(OperatorConfig config, ISource source, IInputData input)
            : this(config)
        {
            CreateContent(config, source, input, ID_STR);
        }

        internal Operator(OperatorConfig config, IDbDataSource source,
            IInputData input, IFieldInfo[] keys)
            : this(config)
        {
            string queryString = GetQueryString(keys);
            CreateContent(config, source, input, queryString);
        }

        [SimpleAttribute]
        public string Id { get; private set; }

        [SimpleAttribute]
        public string Caption { get; private set; }

        [SimpleAttribute]
        public string Info { get; private set; }

        [SimpleAttribute]
        public string Content { get; private set; }

        [SimpleAttribute]
        public string ConfirmData { get; private set; }

        [SimpleAttribute]
        public string IconClass { get; private set; }

        [SimpleAttribute]
        public string DialogTitle { get; private set; }

        private static string GetQueryString(IFieldInfo[] keyFields)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var key in keyFields)
            {
                if (builder.Length > 0)
                    builder.Append("&");
                builder.AppendFormat(ObjectUtil.SysCulture, "{0}=*{0}*", key.NickName);
            }
            return builder.ToString();
        }

        private void CreateContent(OperatorConfig config, object source, IInputData input, string queryString)
        {
            if (config.Content != null)
            {
                Content = Expression.Execute(config.Content, source);
                if (config.UseKey)
                    Content = UriUtil.AppendQueryString(Content, queryString);
            }
            else
            {
                //string sourceStr = input.SourceInfo.CcSource;
                //if (Info != null)
                //{
                //    if (Info.Contains(RightConst.INSERT))
                //        Content = string.Format(ObjectUtil.SysCulture,
                //            "insert/{0}.c", sourceStr).AppVirutalPath();
                //    else if (Info.Contains(RightConst.UPDATE))
                //        Content = string.Format(ObjectUtil.SysCulture,
                //            "update/{0}.c?{1}", sourceStr, queryString).AppVirutalPath();
                //    else if (Info.Contains(RightConst.DELETE))
                //        Content = string.Format(ObjectUtil.SysCulture,
                //            "delete/{0}.c?{1}", sourceStr, queryString).AppVirutalPath();
                //}
                string sourceStr = input.SourceInfo.Source;
                if (Info != null)
                {
                    if (Info.Contains(RightConst.INSERT))
                        Content = string.Format(ObjectUtil.SysCulture,
                            "c/xml/insert/{0}", sourceStr).AppVirutalPath();
                    else if (Info.Contains(RightConst.UPDATE))
                        Content = string.Format(ObjectUtil.SysCulture,
                            "c/xml/update/{0}?{1}", sourceStr, queryString).AppVirutalPath();
                    else if (Info.Contains(RightConst.DELETE))
                        Content = string.Format(ObjectUtil.SysCulture,
                            "c/xml/delete/{0}?{1}", sourceStr, queryString).AppVirutalPath();
                }
            }
            if (config.DialogTitle == null)
                DialogTitle = Caption;
            else
                DialogTitle = Expression.Execute(config.DialogTitle, source);
        }

        public static List<Operator> ReadFromDataTable(DataTable table)
        {
            return table.CreateListFromTable(() => new Operator());
        }
    }
}