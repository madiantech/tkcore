using System;
using System.Globalization;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class PageInfo
    {
        internal PageInfo(IPageData pageData, SessionGlobal sessionGbl)
        {
            IUserInfo info = WebGlobalVariable.Info;
            UserId = info.UserId.ConvertToString();
            RoleId = info.MainOrgId.ConvertToString();
            Source = pageData.SourceInfo.Source;
            Module = true;
            IsHttpPost = pageData.IsPost;
            Guid = sessionGbl.TempIndentity;
            SessionId = sessionGbl.SessionId;
            Culture = ObjectUtil.SysCulture;
            Style = PageStyleClass.FromStyle(pageData.Style);
            ModuleCreator = pageData.SourceInfo.ModuleCreator;
        }

        [SimpleElement]
        public string UserId { get; private set; }

        [SimpleElement]
        public string RoleId { get; private set; }

        [SimpleElement]
        public string Source { get; private set; }

        [SimpleElement]
        public bool Module { get; private set; }

        [SimpleElement]
        public bool IsHttpPost { get; private set; }

        [SimpleElement]
        public Guid Guid { get; private set; }

        [SimpleElement]
        public string SessionId { get; private set; }

        [SimpleElement]
        public CultureInfo Culture { get; private set; }

        [SimpleElement]
        public PageStyleClass Style { get; private set; }

        [SimpleElement]
        public string ModuleCreator { get; private set; }
    }
}