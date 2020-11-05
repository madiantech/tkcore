using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.ExternalContacts;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IExternalContactsService
    {
        //离职成员的外部联系人再分配
        [ApiMethod("/crm/transfer_external_contact", Method = HttpMethod.Post, PostType = typeof(ExtContactParam))]
        BaseResult TransferExtContact([ApiParameter(Location = ParamLocation.Partial)]string externalUserId,
            [ApiParameter(Location = ParamLocation.Partial)]string handoverUserId,
            [ApiParameter(Location = ParamLocation.Partial)]string takeoverUserId);

        //成员对外信息

        //获取外部联系人详情
        [ApiMethod("/crm/get_external_contact")]
        ExtContactResult GetExtContactDetail(
            [ApiParameter(Location = ParamLocation.Content, ParamName = "external_userid")]string externalUserId);

        //获取外部联系人列表
        [ApiMethod("/crm/get_external_contact_list", ResultKey = "external_userid")]
        List<string> GetExtContactList([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        //获取配置了客户联系功能的成员列表
        [ApiMethod("/crm/get_customer_contacts", ResultKey = "customer_contacts")]
        List<string> GetCustomerContacts();
    }
}