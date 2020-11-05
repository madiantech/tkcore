// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.tree');

    Toolkit.tree.bindTree = function (elm) {
        var url = elm.attr("data-url");
        var initValue = elm.attr("data-initValue");
        var urlFunc = function (node) {
            var realUrl = Toolkit.page.getAbsoluteUrl(url);
            if (!initValue)
                return realUrl;
            if (node.id === "#")
                return Toolkit.page.addQueryString(realUrl, { "InitValue": initValue });
            else
                return realUrl;
        };
        var types = elm.attr("data-types") || { "Root": { "icon": "icon-list" }, "Branch": { "icon": "icon-list" }, "Leaf": { "icon": "icon-file-alt" } };
        var drag = elm.attr("data-drag");
        var plugIns = ["types", "unique"];
        if (drag) {
            plugIns.push("dnd");
        }
        var selectFunc = Toolkit.data.getFunction(elm.attr("data-selectFunc"));
        if (selectFunc)
            elm.on("select_node.jstree", selectFunc);
        var firstFunc = Toolkit.data.getFunction(elm.attr("data-firstFunc"));
        var realFirstFunc = null;
        if (firstFunc) {
            realFirstFunc = function (event, tree) {
                firstFunc(event, tree);
                Toolkit.tree._scrollSelectedItem(event, tree);
            }
        }
        else
            realFirstFunc = Toolkit.tree._scrollSelectedItem;
        elm.on("loaded.jstree", realFirstFunc);
        elm.jstree({
            "plugins": plugIns,
            "types": types,
            "core": {
                "data": {
                    "url": urlFunc,
                    "data": function (node) {
                        return { 'id': node.id };
                    }
                },
                "check_callback": function (operation, node, node_parent, node_position, more) {
                    // operation can be 'create_node', 'rename_node', 'delete_node', 'move_node' or 'copy_node'
                    // in case of 'rename_node' node_position is filled with the new node name
                    return operation === 'move_node' ? true : false;
                },
                "strings": {
                    "Loading ...": "加载中..."
                },
                "multiple": elm.attr("data-multiple") ? true : false,
                "error": function (r) {
                    elm.jstree(true).destroy()
                }
            }
        });
    };

    Toolkit.tree._scrollSelectedItem = function (event, tree) {
        setTimeout(function () {
            var nowTopDistance = $(".jstree-clicked").offset().top;
            var windowHeight = window.innerHeight;
            if (nowTopDistance > windowHeight) {
                //$('html, body').animate({
                //    scrollTop: nowTopDistance - 50
                //});
                $('html, body').scrollTop(nowTopDistance - 50)
            }
        }, 300);
    };

    Toolkit.tree._firstClick = function (event, tree) {
        var treeObj = tree.instance;
        try {
            var node = treeObj.get_top_selected(true);
            if (node.length === 0) {
                var containter = treeObj.get_container();
                var id = containter.find("ul>li:first");
                var nodeId = id.attr("id");
                if (nodeId != null)
                    treeObj.select_node(nodeId);
            }
            else {
                treeObj.deselect_node(node);
                treeObj.select_node(node);
            }
        }
        catch (e) {
        }
    };

    Toolkit.tree.detailClick = function (node, selected) {
        if (selected.node) {
            var obj = selected.instance.element;
            var url = obj.attr("data-detailUrl");
            var params = {};
            params[obj.attr("data-idName")] = selected.node.id;
            url = Toolkit.page.addQueryString(url, params);
            $("#nodeDetail").load(url, function () {
                $('#nodeDetail').bindToolkitUI('dataLinkExecute');

                var detailList = $("a[data-dlist]");
                detailList.click(Toolkit.tree.detailList).attr("href", "javascript:void(0)");
                detailList.first().click();
            });
        }
    };

    Toolkit.tree._loadData = function (div) {
        var data = div.clone();
        data.find("tbody").attr("id", "pageList");
        $("#listData").empty().append(data.children());
        if (Toolkit.list != undefined)
            $("#listData").bind("PageInit", Toolkit.list._initPage).trigger("PageInit");
        $("#listData").bindToolkitUI("PageContentExecute");
    };

    Toolkit.tree.detailList = function () {
        var treeObj = $('#treeContent');
        var tree = $.jstree.reference(treeObj);
        if (!tree)
            return;

        var node = tree.get_top_selected(true);
        var id;
        if (node.length === 1) {
            id = node[0].id;
        }
        else
            return;

        var elm = $(this);
        var li = elm.parent();
        li.siblings().removeClass("active");
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
            var params = { "ChildName": childName, "Index": index };
            params[treeObj.attr("data-idname")] = id;
            queryParams = $.extend(queryParams, params);
            var loadUrl = Toolkit.page.getAbsoluteUrl("c/xml/" + style + "/" + $("body").attr("data-source"));
            $.ajax({
                type: 'get', dataType: 'text', url: loadUrl, data: queryParams,
                success: function (req) {
                    div.html(req);
                    div.data("loaded", true);
                    Toolkit.tree._loadData(div);
                },
                error: function () {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            });
        }
        else {
            Toolkit.tree._loadData(div);
        }
    };

    Toolkit.tree.refreshCurrent = function (elm) {
        var tree = $.jstree.reference($('#treeContent'));
        if (!tree)
            return;

        var node = tree.get_top_selected(true);
        if (node.length === 1) {
            var params = { "InitValue": node[0].id };
            var url = Toolkit.page.adjustUrl(params);
            document.location.href = url;
        }
        else {
            document.location.reload();
        }
        return true;
    };

    Toolkit.tree.getSelfUrl = function (elm) {
        var tree = $.jstree.reference($('#treeContent'));
        if (!tree)
            return;

        var node = tree.get_top_selected(true);
        if (node.length === 1) {
            var params = { "InitValue": node[0].id };
            return Toolkit.page.adjustUrl(params, "RetURL");
        }
        else
            return Toolkit.page.adjustUrl(null, "RetURL");
    };

    Toolkit.tree.moveTreeNode = function () {
        var treeObj = $("#treeContent");
        var tree = $.jstree.reference(treeObj);
        if (!tree)
            return;
        var nodes = tree.get_selected(true);
        if (nodes.length === 1) {
            var obj = $(this);
            var action = obj.attr("data-action");
            var node = nodes[0];
            var nextNode;
            if (action === "before")
                nextNode = tree.get_prev_dom(node, true);
            else
                nextNode = tree.get_next_dom(node, true);
            if (nextNode.length === 1)
                tree.move_node(node, nextNode, action, function (node, parent, position) {
                    var direction = action === "before" ? "Up" : "Down";
                    var url = Toolkit.page.getAbsoluteUrl("c/xml/CMoveUpDown/" + treeObj.attr("data-source"));
                    var params = { "direction": direction, "Id": node.id };
                    $.ajax({
                        type: 'get', dataType: 'text', url: url, data: params,
                        success: function (req) {
                        },
                        error: function () {
                            Toolkit.page.showError('数据提交失败！请稍候重试。');
                        }
                    });
                });
        }
    };

    $(document).ready(function () {
        var treeDiv = $("div.tktree");
        if (treeDiv.length > 0)
            Toolkit.tree.bindTree(treeDiv);
        $("body").bind("SelfUrl", Toolkit.tree.getSelfUrl).bind("ListRefresh", Toolkit.tree.refreshCurrent);
        $("#moveUp").click(Toolkit.tree.moveTreeNode);
        $("#moveDown").click(Toolkit.tree.moveTreeNode);
    });
})(jQuery);