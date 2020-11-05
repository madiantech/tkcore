function newEmpty() {
    var element = $(this);
}

function loadData(event) {
    var target = event.target;
    var obj = $(target);
    var loaded = obj.data("loaded");
    if (loaded !== "true") {
        var url = "/xml/List/Workflow_WfMyWorkList.c?GetData=Page&DefName=" + obj.attr("data-def-name") + "&StepId=" + obj.attr("data-step-name");
        url = Toolkit.page.getAbsoluteUrl(url);
        $.ajax({
            type: "get", dataType: 'text', url: url,
            success: function (req) {
                obj.find("div").html(req);
                obj.data("loaded", "true");
            },
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    }
}

$(document).ready(function () {
    $('.mywork').on('show.bs.collapse', loadData);
    //$('.mywork').collapse('show');
});