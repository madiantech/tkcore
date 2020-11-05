using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class VueOperator
    {
        public VueOperator(OperatorConfig config)
        {
            Id = config.Id;
            Caption = config.Caption.ToString(ObjectUtil.SysCulture);
            Info = config.Info;
            ConfirmData = config.ConfirmData?.ToString(ObjectUtil.SysCulture);
            IconClass = config.IconClass;
            if (config.Content != null)
                Content = Expression.Execute(config.Content);
            UseKey = config.UseKey;
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
        public bool UseKey { get; private set; }
    }
}