using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [EsModel]
    internal class SunVueDemoEsModel : BaseEsModel
    {
        private const string Path = "vuedemo3";

        public SunVueDemoEsModel() : base(Path, Path)
        {
            Add("insert", new RazorSinglePageGenerator(this, "SunVueDemo", "Edit/template.cshtml",
                "src/components/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("update", new RazorSinglePageGenerator(this, "SunVueDemo", "Edit/template.cshtml",
                "src/components/edit.vue", (PageStyleClass)PageStyle.Update));
            Add("detail", new RazorSinglePageGenerator(this, "SunVueDemo", "Detail/template.cshtml",
                "src/components/detail.vue", (PageStyleClass)PageStyle.Detail));
            Add("list", new RazorSinglePageGenerator(this, "SunVueDemo", "List/template.cshtml",
                "src/page/index.vue", (PageStyleClass)PageStyle.List));
            Add("index", new RazorSinglePageGenerator(this, "SunVueDemo", "Index/template.cshtml",
                "src/config/index.js", (PageStyleClass)PageStyle.Update, PageStyleClass.FromString("IndexVue")));
            Add("multiindex", new RazorSinglePageGenerator(this, "SunVueDemo", "MultiIndex/template.cshtml",
                "src/config/index.js", (PageStyleClass)PageStyle.Update, PageStyleClass.FromString("IndexVue")));
            Add("multiinsert", new RazorSinglePageGenerator(this, "SunVueDemo", "MultiEdit/template.cshtml",
                "src/components/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("multiupdate", new RazorSinglePageGenerator(this, "SunVueDemo", "MultiEdit/template.cshtml",
                "src/components/edit.vue", (PageStyleClass)PageStyle.Update));
            Add("multidetail", new RazorSinglePageGenerator(this, "SunVueDemo", "MultiDetail/template.cshtml",
                "src/components/detail.vue", (PageStyleClass)PageStyle.Detail));
            Add("tree", new RazorSinglePageGenerator(this, "SunVueDemo", "Tree/template.cshtml",
                "src/page/index.vue", (PageStyleClass)PageStyle.List, PageStyleClass.FromString(string.Empty)));
            Add("treeinsert", new RazorSinglePageGenerator(this, "SunVueDemo", "Tree/edittemplate.cshtml",
               "src/components/add.vue", (PageStyleClass)PageStyle.Insert));
            Add("treeupdate", new RazorSinglePageGenerator(this, "SunVueDemo", "Tree/edittemplate.cshtml",
                "src/components/edit.vue", (PageStyleClass)PageStyle.Update));
            Add("treedetail", new RazorSinglePageGenerator(this, "SunVueDemo", "Tree/detailtemplate.cshtml",
                "src/components/detail.vue", (PageStyleClass)PageStyle.Detail));
        }
    }
}