// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.list');

    Toolkit.list._queryParams = function (listView) {
        var data = {
            "Page": listView.attr("data-page"), "Condition": listView.attr("data-condition"),
            "Tab": listView.attr("data-tab"), "TotalCount": listView.attr("data-totalcount"),
            "TotalPage": listView.attr("data-totalpage"), "JsonOrder": listView.attr("data-jsonorder")
        };
        return data;
    };

    Toolkit.list._queryResult = function (req) {
        var div = $("#listData");
        div.html(req);
        div.trigger("PageInit");
        div.bindToolkitUI("PageContentExecute");
        Toolkit.ui.enableButton(true);
    };

    Toolkit.list._initPage = function (elm) {
        $("a[data-tab]").click(function () {
            Toolkit.list._loadPageByTab($(this).attr("data-tab"));
        }).attr("href", "javascript:void(0);");
        $("a[data-page]").click(function () {
            Toolkit.list._loadPageByPage(parseInt($(this).attr("data-page")) - 1);
        }).attr("href", "javascript:void(0);");
        //$('thead th[data-sort]').click(function () {
        //    var sort = $(this).attr('data-sort') || '';
        //    var order = $(this).attr('data-order') || '';
        //    Toolkit.list._loadPageByOrder({ sort: sort, order: order });
        //});
        $('thead th[data-name]').click(function () {
            var name = $(this).attr('data-name');
            var order = $(this).attr('data-order') || '';
            Toolkit.list._loadPageByOrder2({ "NickName": name, "Order": order });
        });
        Toolkit.list.tabCount(elm);
    };

    Toolkit.list._loadPageByTab = function (tab) {
        Toolkit.list._loadPage({ "Tab": tab, "Page": 0, "TotalCount": 0 });
    };

    //Toolkit.list._loadPageByOrder = function (data) {
    //    Toolkit.list._loadPage({ 'Sort': data.sort, 'Order': data.order });
    //};

    Toolkit.list._loadPageByOrder2 = function (data) {
        var jsonOrder = { "FieldList": [] };
        jsonOrder.FieldList.push(data);
        Toolkit.list._loadPage({ 'JsonOrder': Toolkit.json.stringify(jsonOrder) });
    };

    Toolkit.list._loadPageByPage = function (page) {
        Toolkit.list._loadPage({ "Page": page });
    };

    Toolkit.list._loadPage = function (options) {
        var listView = $("#pageList");
        if (listView.length === 0)
            return;

        var url = listView.attr("data-url");
        var path = url.getPathName();
        var param = url.getQueryJson();
        var data = Toolkit.list._queryParams(listView);
        data.GetData = "Page";

        $.extend(param, data);
        $.extend(param, options);
        delete param[''];
        param._ = (new Date()).getTime();
        $.ajax({
            type: 'get', dataType: 'text', url: path, data: param,
            //beforeSend: function (request) {
            //    var token = listView.attr("data-condition");
            //    request.setRequestHeader("X-Condition", token);
            //},
            success: Toolkit.list._queryResult,
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    };

    Toolkit.list.getQueryJson = function (frm) {
        var postObj = frm.attr("data-post");
        if (!postObj)
            return undefined;

        var fields = postObj.parseJSON().Fields;
        var postData = { "Condition": {}, "IsEqual": false };
        for (var i = 0; i < fields.length; i++) {
            var field = fields[i];
            var elm = Toolkit.data.getElement(frm, field);
            var fieldValue = Toolkit.data.getElementValue(elm, field.Type);
            if (fieldValue !== "")
                postData.Condition[field.Name] = fieldValue;
        }

        return postData;
    };

    Toolkit.list.query = function (frmId) {
        var frm = Toolkit.data._getForm(frmId, $(this), "QueryForm");
        if (!frm)
            return;
        Toolkit.ui.enableButton(false);
        var table = frm.attr("data-post").parseJSON();
        var postData = { "Condition": {}, "IsEqual": false };
        postData.Condition = Toolkit.data.getTableData(table, false);
        postData.IsEqual = $("#_searchMethod").prop("checked");
        var listView = $("#pageList");
        if (listView.length === 0)
            return;
        $("#WebListXmlPage").attr("data-tabcountData", "");
        var listParams = Toolkit.list._queryParams(listView);
        listParams.TotalCount = 0;
        listParams.Page = 0;
        var url = Toolkit.page.adjustUrl(listParams, "RetURL");
        $.ajax({
            type: 'post', dataType: 'text', url: url,
            data: Toolkit.json.stringify(postData),
            success: Toolkit.list._queryResult,
            error: function () {
                Toolkit.ui.enableButton(true);
                //Toolkit.page.showError('数据提交失败！请稍候重试。');
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
            var data = Toolkit.list._queryParams(listView);
            data.GetData = "Page";
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

    Toolkit.list.getSelfUrl = function (elm) {
        var listView = $("#pageList");
        if (listView.length === 0)
            return;

        var listParams = Toolkit.list._queryParams(listView);
        return Toolkit.page.adjustUrl(listParams, "RetURL");
    };

    Toolkit.list.refreshCurrent = function (elm) {
        var listView = $("#pageList");
        if (listView.length === 0)
            return;

        var listParams = Toolkit.list._queryParams(listView);
        listParams.TotalCount = 0;
        var url = Toolkit.page.adjustUrl(listParams, "RetURL");
        document.location.href = url;

        return true;
    };

    Toolkit.list.exportExcel = function (elm) {
        var listView = $("#pageList");
        if (listView.length === 0)
            return;

        var listParams = Toolkit.list._queryParams(listView);
        listParams.PageSize = 65535;
        listParams.Page = 0;
        var url = Toolkit.page.getAbsoluteUrl("excel/list/" + listView.attr("data-source") + ".c");
        url = Toolkit.page.addQueryString(url, listParams);
        document.location.href = url;
    };

    Toolkit.list._setTabCount = function (res, tabSheet) {
        if (res.TabList && res.TabList.length > 0) {
            for (var i = 0; i < res.TabList.length; i++) {
                var item = res.TabList[i];
                var text = tabSheet.find("[data-tab='" + item.Id + "']").text();
                $("#listTabSheet").find("[data-tab='" + item.Id + "']").append("(" + item.Count + ")");
            }
        }
    };

    Toolkit.list.tabCount = function (elm) {
        var tabSheet = $("#listTabSheet");
        if (!tabSheet || tabSheet.length === 0)
            return;
        var pageList = $("#pageList");
        if (pageList.attr("data-tabcount") !== "1")
            return;

        var tabCountData = $("#WebListXmlPage").attr("data-tabcountData");
        if (tabCountData && tabCountData != "") {
            var res = tabCountData.parseJSON();
            Toolkit.list._setTabCount(res, tabSheet);
        }
        else {
            var source = pageList.attr("data-source");
            var url = Toolkit.page.getAbsoluteUrl("CTabCount/" + source + ".c");
            var listParams = Toolkit.list._queryParams(pageList);
            $.ajax({
                url: url,
                data: listParams,
                dataType: "json",
                success: function (res) {
                    $("#WebListXmlPage").attr("data-tabcountData", Toolkit.json.stringify(res));
                    Toolkit.list._setTabCount(res, tabSheet);
                }
            });
        }
    };

    Toolkit.list.initSearch = function (elm) {
        Toolkit.data._clearError($("#QueryForm"));
        $(".btn-search").click(Toolkit.list.query);
        $("select.auto-search").change(Toolkit.list.query);
        $("#QueryForm").bind("submit", function () { return false; });
        $("#btn-getmore").bind("click", Toolkit.data.nextPage);
        $("button.export-excel").click(Toolkit.list.exportExcel);
        $("select[data-auto-query=true]").change(Toolkit.list.query);
    };

    $(document).ready(function () {
        var listObj = $("#WebListXmlPage");
        if (listObj.length > 0) {
            listObj.bind("SearchInit", Toolkit.list.initSearch);
            listObj.trigger("SearchInit");
            $("body").bind("SelfUrl", Toolkit.list.getSelfUrl).bind("ListRefresh", Toolkit.list.refreshCurrent);
            $("#listData").bind("PageInit", Toolkit.list._initPage).trigger("PageInit");
        }
    });
})(jQuery);