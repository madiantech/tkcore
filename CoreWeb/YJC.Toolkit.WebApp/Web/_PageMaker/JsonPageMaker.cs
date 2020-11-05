using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class JsonPageMaker : BaseObjectPageMaker
    {
        public JsonPageMaker()
        {
            Add(PageMakerUtil.IsType(SourceOutputType.XmlReader, SourceOutputType.DataSet),
                JsonDataSetXmlReaderPageMaker.PageMaker);
            Add(PageMakerUtil.IsType(SourceOutputType.ToolkitObject),
                new InternalJsonObjectPageMaker(null, null));
            Add(PageMakerUtil.IsType(SourceOutputType.String),
                new SourceOutputPageMaker(ContentTypeConst.JSON));
        }

        internal JsonPageMaker(JsonPageMakerAttribute attribute)
            : this()
        {
            SetFormat(attribute);
        }

        internal JsonPageMaker(JsonPageMakerConfig config)
            : this()
        {
            SetFormat(config);
        }
    }
}