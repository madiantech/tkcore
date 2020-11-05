// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.import');

    Toolkit.import.upload = function (elm) {
        Toolkit.ui.enableButton(false);
        $.ajax({
            type: 'post',
            dataType: 'text',
            url: window.location.href,
            data: "",
            success: _successCommit,
            error: function () {
                Toolkit.ui.enableButton(true);
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    };

    $(document).ready(function () {
        $("#uploadBtn").click(Toolkit.import.upload).attr("href", "javascript:void(0);");
    });
})(jQuery);