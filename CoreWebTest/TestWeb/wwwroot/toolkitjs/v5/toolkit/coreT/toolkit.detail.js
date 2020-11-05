// --------------------------------
// Toolkit.detail
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.detail');

    Toolkit.detail._restoreList = function (div) {
        var listData = $("#listData");
        var data = div.clone();
        data.find("tbody").attr("id", "pageList");
        listData.empty().append(data.children());
        listData.trigger("PageInit");
        listData.bindToolkitUI("PageContentExecute");
    };

    Toolkit.detail._saveList = function (li) {
        if (li.hasClass("active")) {
            var aElm = li.find("a");
            var listData = $("#listData");
            var copyData = listData.clone();
            copyData.find("tbody").removeAttr("id");
            var div = $("#" + aElm.attr("data-dlist"));
            div.empty().append(copyData.children());
            li.removeClass("active");
        }
    };

    Toolkit.detail._firstLoadData = function (response, div) {
        var data = $(response);
        data.find("tbody").removeAttr("id");
        div.append(data);
        div.data("loaded", true);
        Toolkit.detail._restoreList(div);
    }

    Toolkit.detail.detailList = function () {
        var elm = $(this);
        var li = elm.parent();
        li.siblings().each(function () {
            Toolkit.detail._saveList($(this));
        });
        li.addClass("active");

        var childName = elm.attr("data-dlist");
        var div = $("#" + elm.data("dlist"));
        if (!div.data("loaded")) {
            var index = div.data("index");
            var style;
            if (index === 0)
                style = "CDetailList";
            else
                style = "CDetailList" + index;

            var uri = new URI(window.location.href);
            var queryParams = uri.search(true);
            queryParams = $.extend(queryParams, { "ChildName": childName, "Index": index });
            var loadUrl = Toolkit.page.getAbsoluteUrl("c/xml/" + style + "/" + $("body").attr("data-source"));
            $.ajax({
                type: 'get', dataType: 'text', url: loadUrl, data: queryParams,
                success: function (req) {
                    Toolkit.detail._firstLoadData(req, div);
                },
                error: function () {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            });
        }
        else {
            Toolkit.detail._restoreList(div);
        }
    };

    $(document).ready(function () {
        $("body").bind("SelfUrl", Toolkit.data.getSelfUrl);
        var detailList = $("a[data-dlist]");
        detailList.click(Toolkit.detail.detailList).attr("href", "javascript:void(0)");
        detailList.first().click();
        $("#listData").bind("PageInit", Toolkit.list._initPage || null);
    });
})(jQuery);