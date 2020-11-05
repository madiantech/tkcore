using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [EsModel]
    internal class VueDemoEsModel : BaseEsModel
    {
        private const string Path = "vuedemo";

        public VueDemoEsModel() : base(Path, Path)
        {
            Add("insert", new RazorSinglePageGenerator(this, "WebpackTest",  "Edit/template.cshtml", 
                "src/page/components/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("update", new RazorSinglePageGenerator(this, "WebpackTest", "Edit/template.cshtml",
                "src/page/components/edit.vue", (PageStyleClass)PageStyle.Update));
            Add("detail", new RazorSinglePageGenerator(this, "WebpackTest", "Detail/template.cshtml",
                "src/page/components/detail.vue", (PageStyleClass)PageStyle.Detail));
            Add("list", new RazorSinglePageGenerator(this, "WebpackTest", "List/template.cshtml",
                "src/page/index.vue", (PageStyleClass)PageStyle.List));
        }
    }
}
