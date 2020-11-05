$(function () {
    var $Target = $('#Target');

    function hideShow() {
        var selectedId = $Target.children('option:selected').val() + 'Id',
         i = 1,
         len = $Target.children('option').length;

        for (; i < len; i++) {
            var closePart = $Target.children('option:eq(' + i + ')').val() + 'Id';
            $('#' + closePart).closest('div').hide();
        }

        $('#' + selectedId).closest('div').show();
    }

    hideShow();

    $Target.change(function () {
        hideShow();
    });

});