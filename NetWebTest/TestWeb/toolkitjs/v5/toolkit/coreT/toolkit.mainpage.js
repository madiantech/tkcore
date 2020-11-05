// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.mainpage');
    Toolkit.mainpage.menuClick = function () {
        var obj = $(this);
        var url = obj.attr("data-menu");
        $("#tkFrameMain").attr("src", url);
    }

    Toolkit.mainpage.frameLoaded = function() {
        var title = $("#MainPage").attr("data-title");
        var subTitle = $("#tkFrameMain").get(0).contentWindow.document.title;
        if (title.trim() != "")
            subTitle += " - " + title;
        document.title = subTitle;
    }

    Toolkit.mainpage.reloadFrame = function () {
        $("#tkFrameMain").get(0).contentWindow.location.reload();
    }

    Toolkit.mainpage.toggleMenu = function () {
        var $navbarToggle = $('#navbarToggle');
        var $toggleNavbarBtn = $('#toggleNavbarBtn');
        var $tkMain = $('div.tk-main');

        if (!$navbarToggle.is(':hidden')) {
            $navbarToggle.slideUp('slow');
            $toggleNavbarBtn.find('span').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
            $tkMain.animate({ 'top': '0px' }, 'slow');
        } else {
            $navbarToggle.slideDown('down');
            $toggleNavbarBtn.find('span').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
            $tkMain.animate({ 'top': '52px' }, 'slow');
        }
    }

    $(document).ready(function () {
        $("li>a[data-menu]").click(Toolkit.mainpage.menuClick);
        $('.dropdown-submenu').hover(function (event) {
            $(this).find('ul.dropdown-menu').toggle()
        });
        $("#tkFrameMain").on("load", Toolkit.mainpage.frameLoaded);
        $('#toggleNavbarBtn').click(Toolkit.mainpage.toggleMenu);
        $('#refreshFrame').click(Toolkit.mainpage.reloadFrame);
    });
})(jQuery);