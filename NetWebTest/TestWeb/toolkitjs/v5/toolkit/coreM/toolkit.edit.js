// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.edit');

    Toolkit.edit.initPage = function (elm) {
        $(".btn-submit").click(Toolkit.data.post).attr("href", "javascript:void(0);");
        $("#PostForm").bind("submit", function () { return false; });
    };

    $(document).ready(function () {
        $("#WebInsertXmlPage").bind("PageInit", Toolkit.edit.initPage).trigger("PageInit");
        $("#WebUpdateXmlPage").bind("PageInit", Toolkit.edit.initPage).trigger("PageInit");
    });
})(jQuery);