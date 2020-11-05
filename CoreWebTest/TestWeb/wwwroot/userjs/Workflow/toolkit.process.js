// --------------------------------
// Toolkit.workflow
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.workflow');

    Toolkit.workflow.initPage = function (elm) {
        $(".btn-submit").click(Toolkit.data.post).attr("href", "javascript:void(0);");
        $("#PostForm").bind("submit", function () { return false; });
        Toolkit.data.setTableProp();
    };

    Toolkit.workflow.loadContent = function () {
        var div = $("#_workflow_content");
        var url = div.attr("data-url");
        $.ajax({
            type: "get", dataType: 'text', url: url,
            success: function (req) {
                div.html(req);
            },
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    };

    Toolkit.workflow.uiPost = function (e) {
        var params = {};
        var saveTag = $(this).attr("data-save");
        if (typeof (saveTag) != "undefined") {
            params.save = saveTag;
        }
        var url = Toolkit.page.adjustUrl(params, null);
        $("#PostForm").attr("action", url);
        Toolkit.data.post(e);
    };

    Toolkit.workflow._nonUiAjax = function (url) {
        $.ajax({
            type: 'get',
            dataType: 'text',
            url: url,
            success: function (req) {
                _successCommit(req);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    };

    Toolkit.workflow.nonUIPost = function () {
        var btn = $(this);
        var url = Toolkit.page.getAbsoluteUrl("c/source/C/WfOperationProcess");
        var params = {
            "RegName": btn.attr("data-content"),
            "WfId": btn.attr("data-wf-id")
        };
        url = Toolkit.page.addQueryString(url, params, null);
        var confirm = btn.attr("data-confirm");
        if (confirm === undefined || confirm === "")
            Toolkit.workflow._nonUiAjax(url);
        else
            Toolkit.page.confirm(confirm, function () { Toolkit.workflow._nonUiAjax(url); });
    };

    $(document).ready(function () {
        $("body").bind("PageInit", Toolkit.workflow.initPage).trigger("PageInit");
        Toolkit.workflow.loadContent();
        $("button[data-oper-type=UI]").attr("href", "javascript:void(0);").click(Toolkit.workflow.uiPost);
        $("button[data-oper-type=NonUI]").attr("href", "javascript:void(0);").click(Toolkit.workflow.nonUIPost);
    });
})(jQuery);