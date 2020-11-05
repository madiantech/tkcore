function internalSendMass(id, retUrl) {
    var url = "../Library/WebModuleContentPage.tkx?Source=CXCS/SendMass&MassId=" + id;
    $.ajax({
        type: 'get', dataType: 'text', url: url,
        success: function (req) {
            if (req === "0") {
                alert("群发成功");
                if (retUrl === "")
                    retUrl = "../Library/WebListXmlPage.tkx?Source=CXCS/WeixinMass";
                document.location.href = retUrl;
            }
            else if (req === "-1")
                alert("群发失败");
            else if (req === "-2")
                alert("已经群发过，不再发送");
            else
                alert(req);
        },
        error: function () {
            Toolkit.page.showError('数据提交失败！请稍候重试。');
        }
    });
}

function sendMassMessage(id, retUrl) {
    var path = "../Library/WebModuleContentPage.tkx?Source=CXCS/MassCount";
    $.ajax({
        type: 'get', dataType: 'text', url: path, 
        success: function(req) {
            var count = parseInt(req);
            var msg;
            if (count === 0)
                msg = "本月尚未发送，";
            else
                msg = "本月已发送" + count + "条，"
            msg = "每月只能群发4次，" + msg + "请确定是否需要群发？";
            Toolkit.page.confirm(msg, function () { internalSendMass(id, retUrl); });
        },
        error: function () {
            Toolkit.page.showError('数据提交失败！请稍候重试。');
        }
    });
}
