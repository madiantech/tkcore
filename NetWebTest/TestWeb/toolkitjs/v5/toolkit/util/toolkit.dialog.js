// --------------------------------
// Toolkit.util.dialog
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.util');
    // ----------------------------
    if (Toolkit.util.dialog) return;
    // ----------------------------
    var utilId = 'util-dialog';
    var utilText = Toolkit.lang.get(utilId) || {
        'close': '关闭',
        'closeTips': '关闭浮层',
        'loading': '正在加载中……',
        'loadingError': '加载失败',
        'confirm': '确认',
        'prompt': '请输入…',
        'cancel': '取消',
        'info': '提醒'
    };
    var alertParams = {
        "info": { "title": "提醒", "button": "info", "icon": "info", "close": "关闭", "type": "info" },
        "success": { "title": "成功", "button": "success", "icon": "ok", "close": "关闭", "type": "success" },
        "error": { "title": "失败", "button": "danger", "icon": "remove", "close": "关闭", "type": "danger" },
        "warning": { "title": "警告", "button": "warning", "icon": "warning", "close": "关闭", "type": "warning" },
        "primary": { "title": "提示", "button": "primary", "icon": "exclamation", "close": "关闭", "type": "primary" }
    };
    // ----------------------------
    Toolkit.util.dialog = {};
    Toolkit.util.dialog.id = utilId;
    Toolkit.util.dialog.status = false;
    // 弹窗类型
    Toolkit.util.dialog.mode = '';
    Toolkit.util.dialog.dialogType = 'common';
    // 关闭弹出窗时回调方法
    Toolkit.util.dialog.closeCallback = null;
    // 打开弹出窗
    Toolkit.util.dialog.init = function () {
        if ($('#' + this.id).size() == 0) {
            var pdhtml = '<div class="fade modal" role="dialog" tabindex="-1" id="' + this.id + '"><div class="modal-dialog">';
            pdhtml += '<div class="modal-content">';
            pdhtml += '<div class="modal-header"><button type="button" class="close" data-dismiss="modal" title="{close}" aria-hidden="true">&times;</button><h4 class="modal-title"></h4></div>';
            pdhtml += '<div class="modal-body"><div class="pdb-contentframe"></div></div>';
            pdhtml += '<div class="modal-footer"></div>';
            pdhtml += '</div>';
            pdhtml += '</div></div>';
            $('body').append(pdhtml.substitute(utilText));
        }
    };
    Toolkit.util.dialog.open = function (options) {
        if (this.monopolize) return;
        if (this.dialogType == 'mini') {
            options.op = 0.01;
            options.title = '';
        }
        this.init();
        if (options.noCloseBtn) {
            $('#' + this.id + ' .modal-header button').hide();
        }
        if (options.title) {
            $('#' + this.id + ' .modal-header').show();
            this.setTitle(options.title);
        } else {
            $('#' + this.id + ' .modal-header').hide();
        }
        if (options.bottom) {
            $('#' + this.id + ' .modal-footer').show();
            this.setBottom(options.bottom);
        } else {
            $('#' + this.id + ' .modal-footer').hide();
        }
        this.mode = options.mode || 'text';
        var height = parseInt(options.height || 300, 10);
        var _this = this;
        switch (this.mode) {
            case 'ajax':
                $('#' + this.id + ' .pdb-contentframe').ajaxStart(function () {
                    var loading_html = '<p class="txt-loading-32" style="margin-top:{height}px;">&nbsp;</p>'.substitute({ height: (height - 32) / 2 });
                    $(this).html(options.content.loading || loading_html);
                });
                $.ajax({
                    type: options.content.type || 'get',
                    url: options.content.url,
                    data: options.content.param || '',
                    error: function () {
                        _this.setContent(options.content.error || '<div class="p20"><p class="txt-error">{loadingError}</p></div>'.substitute(utilText));
                    },
                    success: function (html) {
                        _this.setContent(html);
                        if (options.ajaxComplete) {
                            var elm = $('#' + _this.id + ' .pdb-contentframe');
                            options.ajaxComplete(elm);
                        }
                    }
                });
                break;
            case 'text':
                this.setContent(options.content);
                break;
            case 'id':
                this.setContent('');
                $('#' + options.content).appendTo('#' + this.id + ' .pdb-contentframe').removeClass("hide").show();
                this.lastContentId = options.content;
                break;
            case 'iframe':
                var iframeId = 'iframe_' + (new Date()).getTime();
                this.setContent('<iframe id="' + iframeId + '" src="about:blank" width="100%" height="' + height + 'px" scrolling="auto" frameborder="0" marginheight="0" marginwidth="0"></iframe>');
                $('#' + iframeId).attr('src', options.url);
                break;
        }
        $('#' + this.id + ' .modal-body').css('overflow', (options.mode == 'iframe') ? 'hidden' : 'auto');
        $('#' + this.id).modal('show');
        $('#' + this.id + ' button[data-dismiss=modal]').click(function () { Toolkit.util.dialog.close("slide"); });
        if (options.callback) options.callback();
        if (options.closeFunction) this.closeCallback = options.closeFunction;
        this.options = options;
        this.status = true;
    };

    // 关闭弹出窗
    Toolkit.util.dialog.close = function () {
        if (this.monopolize) return;
        if (!this.status) return;
        var dialogObj = $('#' + this.id);
        if ($.isFunction(this.closeCallback)) {
            dialogObj.on("hidden.bs.modal", function () {
                dialogObj.off("hidden.bs.modal");
                Toolkit.util.dialog.closeCallback();
                Toolkit.util.dialog.closeCallback = null;
            });
        }
        dialogObj.modal('hide');
        this.status = false;
        this.mode = '';
        if (this.lastContentId) {
            $('body').append($('#' + this.lastContentId));
            $('#' + this.lastContentId).hide();
            this.lastContentId = null;
        }
        //if ($.browser.msie && parseFloat($.browser.version) < 7) $('#' + this.id).bindToolkitUI('ShowOverElements', 'select,object');
        //if ($.isFunction(this.closeCallback)) {
        //    this.closeCallback();
        //    this.closeCallback = null;
        //}
    };
    // 更换弹出窗标题
    Toolkit.util.dialog.setTitle = function (html) {
        $('#' + this.id + ' .modal-header h4').html(html);
    };
    Toolkit.util.dialog.setStyle = function (options) {
        if (options.titlecolor) $('#' + this.id + ' .modal-header').css('color', options.titlecolor);
        if (options.bgcolor) $('#' + this.id + ' .modal-header').css('background-color', options.bgcolor);
        if (options.bordercolor) $('#' + this.id + ' .modal-content').css('border-color', options.bordercolor);
    };
    // 更换弹出窗内容
    Toolkit.util.dialog.setContent = function (html) {
        $('#' + this.id + ' .pdb-contentframe').html(html);
    };
    // 更换弹出窗底部内容 { html, actionBtnText, cancelBtnText, callback }
    Toolkit.util.dialog.setBottom = function (options) {
        if (!options) return;
        if (options.html) {
            $('#' + this.id + ' .modal-footer').html(options.html);
        } else {
            var bottomhtml = '<button type="button" class="btn btn-primary btn-action">{confirm}</button>';
            bottomhtml += '<button type="button" class="btn btn-default" data-dismiss="modal">{cancel}</button>';
            $('#' + this.id + ' .modal-footer').html(bottomhtml.substitute(utilText));
            //if (options.noteHtml) $('#' + this.id + ' .modal-footer .pdb-bottom-text').html(options.noteHtml);
            if (options.actionBtnText) $('#' + this.id + ' .modal-footer .btn-action').html(options.actionBtnText);
            if (options.cancelBtnText) $('#' + this.id + ' .modal-footer .btn-default').html(options.cancelBtnText);
        }
        if (options.callback) {
            $('#' + this.id + ' .modal-footer .btn-action').click(function () { options.callback(); });
        } else {
            $('#' + this.id + ' .modal-footer .btn-action').click(function () { Toolkit.util.dialog.close(); });
        }
        if (options.cancel) {
            $('#' + this.id + ' .modal-footer .btn-default').attr("data-dismiss", "").click(function () { options.cancel(); });
        }
    };
    // 显示加载进度条
    Toolkit.util.dialog.showLoading = function (params) {
        params.mode = 'text';
        params.width = 350;
        params.height = 100;
        params.content = '<p class="txt-loading-32" style="margin-top:' + (params.height - 56) / 2 + 'px;">' + (params.loadingText || utilText.loading) + '</p>';
        if (this.dialogType == 'mini') {
            params.height = 36;
            params.content = '<p class="msg-info tac"><span class="dib ico-l-loading">' + (params.loadingText || utilText.loading) + '</span></p>';
        }
        this.open(params);
        return false;
    };
    // 显示弹出窗内容
    Toolkit.util.dialog.show = function (params) {
        params.mode = 'id';
        this.open(params);
        return false;
    };
    // 打开弹出窗页面
    Toolkit.util.dialog.pop = function (params) {
        //if (this.dialogType == 'mini') {
        //    this.alert('不支持此方法');
        //    return false;
        //}
        params.mode = 'iframe';
        this.open(params);
        return false;
    };
    // 打开Ajax页面
    Toolkit.util.dialog.ajax = function (params) {
        params.mode = 'ajax';
        //params.content = { url: params.url };
        this.open(params);
        return false;
    };
    Toolkit.util.dialog.isOpen = function () {
        return $('#' + this.id).data("bs.modal").isShown;
    };
    // 打开提醒框
    Toolkit.util.dialog.alert = function (params) {
        if (typeof (params) == 'string') params = { message: params };
        params.close = utilText.close;
        if (!params.type) params.type = 'info';
        var infoParams = alertParams[params.type];
        var content = ('<p class="text-{type} f14"><i class="icon-2x icon-{icon}-sign vam mr10"></i>' + params.message + '</p>').substitute(infoParams);
        var bottom = '<button type="button" class="btn btn-{button}" data-dismiss="modal">{close}</button>'.substitute(infoParams);
        var param = { mode: 'text', title: params.title || infoParams.title, content: content, bottom: { html: bottom }, top: params.top, closeFunction: params.callback };
        //if (this.dialogType == 'mini') {
        //    param.content = '<div class="msg-info"><b class="i16 i16-close fr pointer" onclick="Toolkit.util.dialog.close();" title="{close}"></b><b class="i16 i16-{type} fl mr5"></b><span>{message}</span>'.substitute(params);
        //}
        this.open(param);
        return false;
    };
    // 打开确认框
    Toolkit.util.dialog.confirm = function (params) {
        if (typeof (params) == 'string') params = { message: params };
        var content = '<p class="text-primary f14"><i class="icon-2x icon-question-sign vam mr10"></i>{message}</p>'.substitute(params);
        var callback = function () {
            if (params.callback && $.isFunction(params.callback)) Toolkit.util.dialog.closeCallback = params.callback;
            Toolkit.util.dialog.close();
        };
        this.open({ mode: 'text', title: params.title || utilText.confirm, content: content, bottom: { callback: callback, cancel: params.cancel } });
        return false;
    };
    // 打开输入对话框
    Toolkit.util.dialog.prompt = function (params) {
        var templates = {
            text:
              '<input type="text" class="form-control" id="promptText" value="{value}">',
            textarea:
              '<textarea class="form-control" id="promptText">{value}</textarea>'
        };
        if (params == null) params = function (value) { };
        if ($.isFunction(params)) params = { callback : params };
        var ctrl = templates[params.type || 'text'].substitute(params);
        var content = '<div class="form-group">' + ctrl + '</div>';
        var callback = function () {
            var value = $("#promptText").val();
            var retValue = params.callback(value);
            if (retValue == null || !retValue)
                Toolkit.util.dialog.close();
        };
        var cancel = function () {
            var retValue = params.callback(null);
            if (retValue == null || !retValue)
                Toolkit.util.dialog.close();
        }
        this.open({ mode: 'text', title: params.title || utilText.prompt, content: content, bottom: { callback: callback, cancel: cancel } });
        return false;
    };


    // ----------------------------
    Toolkit.debug('dialog.js', '初始化成功');
})(jQuery);