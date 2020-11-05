using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace ZyWorkOrder
{
    [EnumCodeTable(Author = "YK", CreateDate = "2020-02-17", Description = "工单状态")]
    public enum WorkOrderStatus
    {
        [DisplayName("已关闭")]
        YGB =0,
        [DisplayName("待提交")]
        DTJ = 1,
        [DisplayName("筛选工单")]
        SXGD = 2,
        [DisplayName("待排期")]
        DPQ = 3,
        [DisplayName("工单执行")]
        GDZX = 4,
        [DisplayName("工单确认")]
        GDQR = 5,
        [DisplayName("待验收")]
        DYS = 6,
        [DisplayName("已完成")]
        YWC = 7
    }
}