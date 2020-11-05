function save() {
    var data = [];
    var subData = [];
    var partId = $("#PartId").val();
    var functions = $("li.function");
    functions.each(function (index) {
        var self = $(this);
        var selected = self.find("label>input[type=checkbox]").prop("checked");
        if (selected) {
            var item = { "PartId": partId, "FnId": self.attr("data-name") };
            data.push(item);
        }
    });
    var subFuncs = $("li.subFunc");
    subFuncs.each(function (index) {
        var self = $(this);
        var selected = self.find("label>input[type=checkbox]").prop("checked");
        if (selected) {
            var item = { "PartId": partId, "SfId": self.attr("data-name") };
            subData.push(item);
        }
    });

    var postData = { "SYS_PART_FUNC": data, "SYS_PART_SUB_FUNC": subData };
    // alert(Toolkit.json.stringify(postData));
    var url = document.location.href;
    $.ajax({
        type: 'post', dataType: 'text', url: url, data: Toolkit.json.stringify(postData),
        success: _successCommit,
        error: function () {
            Toolkit.page.showError('数据提交失败！请稍候重试。');
        }
    });
}

$(document).ready(function () {
    $("button.btn-save").click(save);
});
