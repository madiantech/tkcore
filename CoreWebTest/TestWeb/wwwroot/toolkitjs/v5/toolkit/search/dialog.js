(function ($) {
    // 绑定数据返回处理
    $.fn.bindSearchListClick = function () {
        if (this.length === 0) return;
        this.click(function () {
            var ctrl = $(this);
            var id = ctrl.attr("data-id");
            var text = ctrl.attr("data-text");
            parent.Toolkit.ui.linkChange($(this).parent(), $('#searchText').data('addition'), $(this).parent().data('addition'));
            //parent.Toolkit.ui.linkChange($(this).parent(), parent.Toolkit.page.currentEasySearchElement.data('addition'), $(this).parent().data('addition'));
            parent.Toolkit.ui.setEasySearchData({ id: id, text: text });
            Toolkit.page.closeDialog();
        });
    };
    var showResult = function (data) {
        var list = $("#data");
        var htmlTemplate = '<li class="list-group-item"><a href="#" data-id="{Value}" data-text="{Name}">{DisplayName}</a></li>';
        var array = data; //data.EasySearch;
        if (array) {
            list.empty();
            var $input = parent.Toolkit.page.currentEasySearchElement;
            //var addition = parent.Toolkit.page.currentEasySearchElement.data('addition');
            var addition = $('#searchText').data('addition');
            for (var i = 0; i < array.length; i++) {
               
                var $str = $(htmlTemplate.substitute(array[i]));
                if ($input.parents('tr').length > 0) {
                    $str.attr({ 'data-trindex': $input.closest('.table-row')[0].rowIndex, 'data-panelid': $input.closest('.tk-datatable').attr('id') });
                }
                if (addition) {
                    $str.attr('data-addition', JSON.stringify(array[i].Data));
                }
                list.append($str[0].outerHTML);
            }
            list.find("li a").bindSearchListClick();
        }
        else
            showError(list, "无数据");
    };

    var showError = function (list, msg) {
        list.empty();
        var htmlTemplate = '<li class="list-group-item">{Message}</li>'
        var html = htmlTemplate.substitute({ "Message": msg });
        list.append(html);
    };

    var queryData = function () {
        var input = $("#searchText");
        var postdata = { "RegName": input.attr("data-regName"), "Text": input.val() };
        var refValue = input.attr("data-refFields");
        if (refValue && refValue != "")
            postdata.RefField = refValue.parseJSON();
        var url = Toolkit.page.getAbsoluteUrl("c/source/C/EasySearch");
        $.ajax({
            type: "post",
            url: url,
            data: Toolkit.json.stringify(postdata),
            dataType: "json",
            success: function (d) {
                showResult(d);
            },
            error: function () {
                showError($("#data"), "数据读取失败");
            }
        });
    };

    $(document).ready(function () {
        $("#searchText").keydown(function (e) {
            try {
                var code = e.which || e.keyCode || 0;
                if (code === 13) {
                    $("searchBtn").click();
                }
            } catch (err) { }
        });
        $("#searchBtn").click(queryData);
        queryData();
    });
})(jQuery);