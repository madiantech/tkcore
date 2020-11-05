using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.Company;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IUserService
    {
        //创建成员
        [ApiMethod("/user/create", Method = HttpMethod.Post)]
        BaseResult CreateUser([ApiParameter(Location = ParamLocation.Content)]User user);

        //读取成员
        [ApiMethod("/user/get", UseConstructor = true)]
        User GetUser([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        //更新成员
        [ApiMethod("/user/update", Method = HttpMethod.Post)]
        BaseResult UpdateUser([ApiParameter(Location = ParamLocation.Content)]User user);

        //删除成员
        [ApiMethod("/user/delete")]
        BaseResult DeleteUser([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        //批量删除成员*
        [ApiMethod("/user/batchdelete", Method = HttpMethod.Post)]
        BaseResult BatchDeleteUser(
            [ApiParameter(Location = ParamLocation.ContentDictionary,
                NamingRule = NamingRule.Lower)]List<string> userIdList);

        //获取部门成员
        [ApiMethod("/user/simplelist", ResultKey = "userlist", IsMultiple = true)]
        List<SimpleUser> GetDepartmentUser(
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int departmentId,
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]
            [TkTypeConverter(typeof(BoolIntConverter))]bool fetchChild = false);

        //获取部门成员详情
        [ApiMethod("/user/list", ResultKey = "userlist", IsMultiple = true, UseConstructor = true)]
        List<User> GetDepartmentUserDetail(
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int departmentId,
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]
            [TkTypeConverter(typeof(BoolIntConverter))]bool fetchChild = false);

        //userid与openid互换  userid转openid
        [ApiMethod("/user/convert_to_openid", ResultKey = "openid", Method = HttpMethod.Post)]
        string ConvertToOpenId(
            [ApiParameter(Location = ParamLocation.ContentDictionary, NamingRule = NamingRule.Lower)]string userId);

        //openid转userid
        [ApiMethod("/user/convert_to_userid", ResultKey = "userid", Method = HttpMethod.Post)]
        string ConvertToUserId(
            [ApiParameter(Location = ParamLocation.ContentDictionary, NamingRule = NamingRule.Lower)]string openId);

        //二次验证
        [ApiMethod("/user/authsucc")]
        BaseResult AuthSucc([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        //邀请成员
        [ApiMethod("/batch/invite", Method = HttpMethod.Post, PostType = typeof(UserInviteParam))]
        UserInviteResult BatchInvite(
            [ApiParameter(Location = ParamLocation.Partial)]List<string> user = null,
            [ApiParameter(Location = ParamLocation.Partial)]List<int> party = null,
            [ApiParameter(Location = ParamLocation.Partial)]List<int> tag = null);

        [ApiMethod("/corp/get_join_qrcode", ResultKey = "join_qrcode")]
        string GetJoinQrCode([ApiParameter(NamingRule = NamingRule.UnderLineLower)]
            [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
            ImageSizeType sizeType = ImageSizeType.Normal);
    }
}