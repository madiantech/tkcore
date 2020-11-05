using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class IdParam
    {
        public IdParam(int id)
        {
            Id = id.ToString();
        }

        public IdParam(string id)
        {
            Id = id;
        }

        [SimpleElement]
        [NameModel(NameModelConst.ROLE, LocalName = "roleId")]
        [NameModel(NameModelConst.ROLE_ID, LocalName = "role_id")]
        [NameModel(NameModelConst.USER, LocalName = "userid")]
        [NameModel(NameModelConst.ROLE_GROUP, LocalName = "group_id")]
        [NameModel(NameModelConst.PROCESS_INSTANCE_ID, LocalName = "process_instance_id")]
        public string Id { get; private set; }
    }
}