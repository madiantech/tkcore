using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IDingSpaceService
    {
        //发送钉盘文件给指定用户
        [ApiMethod("/cspace/add_to_single_chat", Method = HttpMethod.Post)]
        BaseResult AddToSingleChat(
            [ApiParameter(Location = ParamLocation.Content)]SpaceFileInfo pram);

        //新增文件到用户钉盘
        [ApiMethod("/cspace/add", ResultKey = "dentry")]
        string AddFileToUserSpace(
            [ApiParameter(Location = ParamLocation.Content)]FileInfo pram);

        //获取企业下的自定义空间
        [ApiMethod("/cspace/get_custom_space", ResultKey = "spaceid")]
        string GetCustomSpace([ApiParameter]string domain,
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string agentId);

        //授权用户访问企业自定义空间
        [ApiMethod("/cspace/grant_custom_space")]
        BaseResult GrantCustomSpace([ApiParameter]string domain,
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string agentId,
            [ApiParameter]string type, [ApiParameter(NamingRule = NamingRule.Lower)]string userId,
            [ApiParameter]string path, [ApiParameter(NamingRule = NamingRule.Lower)]string fileIds,
            [ApiParameter]int duration);

        //单步文件上传
        [ApiMethod("/file/upload/single", Method = HttpMethod.Post, ResultKey = "media_id",
            PostType = typeof(FileDetail))]
        string UploadSingle([ApiParameter(Location = ParamLocation.Partial)]string agentId,
                            [ApiParameter(Location = ParamLocation.Partial)]int fileSize);

        //分块上传文件 开启分块上传事务
        [ApiMethod("/file/upload/transaction", ResultKey = "upload_id")]
        string UploadTransaction(
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string agentId,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int fileSize,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int chunkNumbers);

        //上传文件块
        [ApiMethod("/file/upload/chunk", Method = HttpMethod.Post, PostType = typeof(ChunkInfo))]
        BaseResult UploadChunk([ApiParameter(Location = ParamLocation.Partial)]string agentId,
            [ApiParameter(Location = ParamLocation.Partial)]string uploadId,
            [ApiParameter(Location = ParamLocation.Partial)]int chunkSequence);

        //提交文件上传事务
        [ApiMethod("/file/upload/transaction", ResultKey = "media_id")]
        string UploadTransaction(
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string agentId,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int fileSize,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]int chunkNumbers,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string uploadId
           );
    }
}