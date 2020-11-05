function newEmpty() {
    var element = $(this);
}

function loadData(event) {
    var target = event.target;
    var obj = $(target);
    var loaded = obj.data("loaded");
    if (loaded !== "true") {
        var url = "/c/xml/List/Workflow/WfMyWorkList?GetData=Page&DefName=" + obj.attr("data-def-name") + "&StepId=" + obj.attr("data-step-name") + "&Name=" + $("#Name").val();
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
function DownloadData() {
    var id = $(event.target).attr("data-id");
    $("#" + id).find("a[data-parent='#" + id + "List']").click();//.collapse('show');
}
$(document).ready(function () {
    $('.mywork').on('show.bs.collapse', loadData);
    $("#All").find("a[data-parent='#AllList']").click();

    $(".btn-search").click(function () {
        var name = $("#Name").val();
        if (name != "") {
            location.href = $("body").attr("data-webpath") + "?Name=" + name;
        } else {
            location.href = $("body").attr("data-webpath");
        }
    });
});