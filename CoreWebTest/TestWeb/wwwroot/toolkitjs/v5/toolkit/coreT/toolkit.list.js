// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.list');

    Toolkit.list.getQueryJson = function (frm) {
        var postObj = frm.attr("data-post");
        if (!postObj)
            return undefined;

        var postTable = postObj.parseJSON();
        var tableData = Toolkit.data.getTableData(postTable, false);

        return tableData;
    };

    Toolkit.list.query = function (frmId) {
        var currentObj = $(this);
        var frm = Toolkit.data._getForm(frmId, currentObj, "QueryForm");
        if (!frm)
            return;

        var queryData = Toolkit.list.getQueryJson(frm);
        if ($.isEmptyObject(queryData)) {
            Toolkit.list.loadPage({ "Page": 0, "Condition": "", "TotalCount": 0 });
            return false;
        }
        var postData = { "Resolver": frm.attr("data-resolver"), "Query": { "Condition": queryData, "IsEqual": false } };

        var url = Toolkit.page.getAbsoluteUrl("source/C/QueryCondition.c")
        $.ajax({
            type: 'post', dataType: 'text', url: url,
            data: Toolkit.json.stringify(postData),
            success: function (req) {
                Toolkit.list.loadPage({ "Page": 0, "Condition": req, "TotalCount": 0 });
            },
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
        return false;
    };

    Toolkit.data.nextPage = function () {
        var listView = $("#pageList");
        var button = $(this);
        if (listView.length === 0)
            return;

        var page = parseInt(listView.attr("data-page"));
        var totalPage = parseInt(listView.attr("data-totalpage"));
        if (totalPage > page) {
            ++page;
            var url = listView.attr("data-url");
            var path = url.getPathName();
            var param = url.getQueryJson();
            var condition = listView.attr("data-condition");
            var data = {
                "Page": page, "GetData": "Page", "Condition": condition,
                "TotalCount": listView.attr("data-totalcount"), "Tab": listView.attr("data-tab"),
                "TotalPage": listView.attr("data-totalpage")
            };
            $.extend(param, data);
            $.ajax({
                type: 'get', dataType: 'text', url: path, data: param,
                success: function (req) {
                    var direction = listView.attr("data-direction");
                    if (direction == "head")
                        listView.prepend(req);
                    else
                        listView.append(req);
                    listView.attr("data-page", page);
                    if (page == totalPage)
                        button.hide();
                    //listView.listview("refresh");
                },
                error: function () {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            });
        }
    };

    Toolkit.list.loadPageByPage = function (page) {
        Toolkit.list.loadPage({ "Page": page });
    };

    Toolkit.list.loadPage = function (options) {
        var listView = $("#pageList");
        if (listView.length === 0)
            return;

        var url = listView.attr("data-url");
        var path = url.getPathName();
        var param = url.getQueryJson();
        var condition = listView.attr("data-condition");
        var data = {
            "Page": listView.attr("data-page"), "Condition": condition,
            "Tab": listView.attr("data-tab"), "TotalCount": listView.attr("data-totalcount"),
            "TotalPage": listView.attr("data-totalpage")
        };
        $.extend(param, data);
        $.extend(param, options);
        url = path + "?" + $.param(param);
        //document.location.replace(url);
        document.location.href = url;
    };

    Toolkit.list.initSearch = function (elm) {
        $("a[data-page]").click(function () {
            Toolkit.list.loadPageByPage(parseInt($(this).attr("data-page")) - 1);
        }).attr("href", "javascript:void(0);");

        $(".btn-search").click(Toolkit.list.query);
        $("select.auto-search").change(Toolkit.list.query);
        $("#QueryForm").bind("submit", function () { return false; });
        $("#btn-getmore").bind("click", Toolkit.data.nextPage);
    };

    Toolkit.list.initPage = function (elm) {
    };

    $(document).ready(function () {
        $("#WebListXmlPage").bind("SearchInit", Toolkit.list.initSearch).bind("PageInit", Toolkit.list.initPage);
        $("#WebListXmlPage").trigger("SearchInit").trigger("PageInit");
    });
})(jQuery);