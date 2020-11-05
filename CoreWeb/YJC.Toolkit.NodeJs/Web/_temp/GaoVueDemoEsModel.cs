using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [EsModel]
    internal class GaoVueDemoEsModel : BaseEsModel
    {
        private const string Path = "vuedemo2";

        public GaoVueDemoEsModel() : base(Path, Path)
        {
            Add("insert", new RazorSinglePageGenerator(this, "GaoVueDemo", "Edit/template.cshtml",
                "src/views/toolkit/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("update", new RazorSinglePageGenerator(this, "GaoVueDemo", "Edit/template.cshtml",
                "src/views/toolkit/edit.vue", (PageStyleClass)PageStyle.Update));
            Add("detail", new RazorSinglePageGenerator(this, "GaoVueDemo", "Detail/template.cshtml",
                "src/views/toolkit/detail.vue", (PageStyleClass)PageStyle.Detail));
            Add("list", new RazorSinglePageGenerator(this, "GaoVueDemo", "List/template.cshtml",
                "src/views/toolkit/list.vue", (PageStyleClass)PageStyle.List));
            Add("multiinsert", new RazorSinglePageGenerator(this, "GaoVueDemo", "MultiEdit/template.cshtml",
                "src/views/toolkit/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("multiupdate", new RazorSinglePageGenerator(this, "GaoVueDemo", "MultiEdit/template.cshtml",
                "src/views/toolkit/edit.vue", (PageStyleClass)PageStyle.Update));
        }
    }
}