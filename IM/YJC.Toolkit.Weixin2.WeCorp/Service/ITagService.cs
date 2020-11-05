using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model;
using YJC.Toolkit.WeCorp.Model.Company;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface ITagService
    {
        //创建标签
        [ApiMethod("/tag/create", Method = HttpMethod.Post, ResultKey = "tagid")]
        int CreateTag([ApiParameter(Location = ParamLocation.Content)]Tag tag);

        //更新标签名字
        [ApiMethod("/tag/update", Method = HttpMethod.Post)]
        BaseResult UpdateTag([ApiParameter(Location = ParamLocation.Content)]Tag tag);

        //删除标签
        [ApiMethod("/tag/delete")]
        BaseResult DeleteTag([ApiParameter(NamingRule = NamingRule.Lower)]int tagId);

        //获取标签成员
        [ApiMethod("/tag/get")]
        TagMember GetTagUser([ApiParameter(NamingRule = NamingRule.Lower)]int tagId);

        //增加标签成员
        [ApiMethod("/tag/addtagusers", Method = HttpMethod.Post, PostType = typeof(TagMemberParam))]
        TagResult AddTagUsers([ApiParameter(Location = ParamLocation.Partial)]int tagId,
            [ApiParameter(Location = ParamLocation.Partial)]List<string> userList = null,
            [ApiParameter(Location = ParamLocation.Partial)]List<int> partyList = null);

        //删除标签成员
        [ApiMethod("/tag/deltagusers", Method = HttpMethod.Post, PostType = typeof(TagMemberParam))]
        TagResult DeleteTagUsers([ApiParameter(Location = ParamLocation.Partial)]int tagId,
            [ApiParameter(Location = ParamLocation.Partial)]List<string> userList = null,
            [ApiParameter(Location = ParamLocation.Partial)]List<int> partyList = null);

        //获取标签列表
        [ApiMethod("/tag/list", ResultKey = "taglist", IsMultiple = true)]
        RegNameList<Tag> GetTagList();
    }
}