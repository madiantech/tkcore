// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.list');

    Toolkit.list._queryResult = function (req) {
        var div = $("#resultTable");
        div.html(req);
        var options = {};
        if (div.data("export") === "True") {
            options.showExport = true;
            options.exportDataType = "all";
            options.exportTypes = ["excel", "doc", "pdf", "powerpoint", "csv", "json"];
        }
        $('#_ResultTable').bootstrapTable(options);
    };

    Toolkit.list.query = function (frmId) {
        var frm = Toolkit.data._getForm(frmId, $(this), "QueryForm");
        if (!frm)
            return;
        Toolkit.ui.enableButton(false);
        var table = frm.attr("data-post").parseJSON();
        var postData = { "Condition": {}, "IsEqual": false };
        postData.Condition = Toolkit.data.getTableData(table, false);
        var url = Toolkit.data.getPostUrl(frm);
        //alert(Toolkit.json.stringify(postData));
        //Toolkit.ui.enableButton(true);
        var response = $("#resultTable").attr("data-response");
        var func = response ? window[response] : Toolkit.list._queryResult;
        $.ajax({
            type: 'post', dataType: 'text', url: url,
            data: Toolkit.json.stringify(postData),
            success: function (req) {
                Toolkit.ui.enableButton(true);
                func(req);
            },
            error: function () {
                Toolkit.ui.enableButton(true);
                //Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
        return false;
    };

    Toolkit.list._initSearch = function (elm) {
        $("#btn-search").bind("click", Toolkit.list.query);
    };

    $(document).ready(function () {
        $("#QueryForm").on('submit', function (event) {
            event.preventDefault();
        });
        var listObj = $("#WebQueryPage");
        if (listObj.length > 0) {
            listObj.bind("SearchInit", Toolkit.list._initSearch);
            listObj.trigger("SearchInit");
        }
    });
})(jQuery);