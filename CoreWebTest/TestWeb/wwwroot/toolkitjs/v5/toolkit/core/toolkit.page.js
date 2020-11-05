// --------------------------------
// Toolkit.page
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.page');
    // ----------------------------
    // 页面初始化
    Toolkit.page.isTopWin = ($('.tk-wrapper').size() > 0) ? true : false; //判断是否为框架top页
    Toolkit.page.isMulTab = ($('ul#tkTabs').size() > 0) ? true : false; //判断是否支持多Tab
    if (window != top) {
        if (Toolkit.page.isTopWin) {
            top.location.href = document.location.href;
            return false;
        }
        Toolkit.page.isIframe = true; //判断是不是iFrame内嵌页面
        Toolkit.page.isDlgWin = (parent.Toolkit.util.dialog && parent.Toolkit.util.dialog.mode == 'iframe') ? true : false; //判断是不是弹窗iframe页面
        Toolkit.page.isSubWin = (!Toolkit.page.isDlgWin) ? true : false; //判断是否为框架子页面
    }
    // ----------------------------
    // 页面数据集合
    Toolkit.page.data = {};
    Toolkit.page.data.userinfo = null; //用户信息定义
    Toolkit.page.data.themes = null; //主题样式定义
    Toolkit.page.data.mainPage = null; //初始载入页面定义
    // ----------------------------
    // 页面自定义事件集合
    Toolkit.page.event = {};
    Toolkit.page.event.bind = function (event, callback) {
        if (!this['on' + event]) this['on' + event] = [];
        this['on' + event].push(callback);
    };
    Toolkit.page.event.unbind = function (event) {
        this['on' + event] = [];
    };
    Toolkit.page.event.trigger = function (event, data) {
        if (!this['on' + event]) return;
        var callbacks = this['on' + event];
        for (var i = 0; i < callbacks.length; i++) {
            callbacks[i](data);
        }
        Toolkit.debug('on' + event, data);
    };
    // 事件定义
    // Toolkit.page.event.onInit：初始附加方法定义
    // Toolkit.page.event.onHtmlLoadComplete：HTML加载完成事件定义
    // ----------------------------
    // 页面模块和页面数据加载管理
    // Toolkit.page.dataLoader 
    Toolkit.page.dataLoader = {};
    Toolkit.page.dataLoader.loadCount = 0;
    Toolkit.page.dataLoader.completeCount = 0;
    Toolkit.page.dataLoader.add = function () {
        if (this.loadCount == 0) {
            //	Toolkit.page.showLoading('正在加载内容…');
        }
        this.loadCount++;
    };
    Toolkit.page.dataLoader.complete = function () {
        this.completeCount++;
        if (this.completeCount == this.loadCount) {
            //	Toolkit.page.hideLoading();			
            this.loadCount = 0;
            this.completeCount = 0;
            Toolkit.page.event.trigger('HtmlLoadComplete');
        }
    };
    // ----------------------------
    // 弹窗相关方法的二次封装
    // Toolkit.util.dialog 
    Toolkit.page.alert = function (message, callback) {
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'alert', { message: message, callback: callback });
    };
    Toolkit.page.showSuccess = function (message, callback) {
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'alert', { message: message, type: 'success', callback: callback });
    };
    Toolkit.page.showError = function (message, callback) {
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'alert', { message: message, type: 'error', callback: callback });
    };
    Toolkit.page.confirm = function (message, onCallback, onCancel) {
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'confirm', { message: message, callback: onCallback, cancel: onCancel });
    };
    Toolkit.page.showLoading = function (params) {
        if (typeof (params) == 'undefined') params = {};
        if (typeof (params) == 'string') params = { loadingText: params };
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'showLoading', params);
    };
    Toolkit.page.hideLoading = function () {
        var TK = (this.isSubWin) ? top.Toolkit : Toolkit;
        TK.load('util.dialog', 'close');
    };
    Toolkit.page.closeDialog = function () {
        var TK = (this.isDlgWin) ? parent.Toolkit : Toolkit;
        TK.load('util.dialog', 'close');
    };
    Toolkit.page.dialog = function (params) {
        Toolkit.load('util.dialog', 'open', params);
    };
    Toolkit.page.dialog.show = function (params) {
        Toolkit.load('util.dialog', 'show', params);
    };
    Toolkit.page.dialog.pop = function (params) {
        Toolkit.load('util.dialog', 'pop', params);
    };
    Toolkit.page.dialog.ajax = function (params) {
        Toolkit.load('util.dialog', 'ajax', params);
    };
    Toolkit.page.dialog.setConfig = function (attr, value) {
        Toolkit.load('util.dialog', function () {
            Toolkit.util.dialog[attr] = value;
        });
    };
    if (Toolkit.page.isDlgWin) {
        Toolkit.page.dialog.setConfig('dialogType', 'mini');
    }
    Toolkit.page.showMessage = function (message) {
        $('body').append('<div class="msg-info fixed bottom10 left10 right10"><b class="i16 i16-close fr pointer" onclick="$(this).parent().fadeOut();" title="关闭提醒"></b><b class="i16 i16-alert fl mr5"></b><span>' + message + '</span></div>');
    };
    // ----------------------------
    Toolkit.page.setLoadingTimer = function (a) {
        this.loadingTimer = setTimeout(function () {
            Toolkit.page.hideLoading();
        }, 3000);
    };
    Toolkit.page.clearLoadingTimer = function () {
        clearTimeout(this.loadingTimer);
        this.loadingTimer = null;
    };
    // ----------------------------
    // 用于弹窗的insert或Update页中的回调刷新
    Toolkit.page.dataList = {};
    Toolkit.page.dataList.current = null;
    Toolkit.page.dataList.insertSuccess = function (message, retUrl) {
        if (message) {
            Toolkit.page.showSuccess(message, function () {
                if (retUrl) {
                    Toolkit.page.dataList.current.trigger('refreshDataPage', retUrl);
                } else {
                    Toolkit.page.dataList.refresh(1);
                }
            });
        }
        else
            document.location = retUrl;
    };
    Toolkit.page.dataList.updateSuccess = function (message, retUrl) {
        Toolkit.page.showSuccess(message, function () {
            if (retUrl) {
                Toolkit.page.dataList.current.trigger('refreshDataPage', retUrl);
            } else {
                Toolkit.page.dataList.refresh();
            }
        });
    };
    Toolkit.page.dataList.refresh = function (page) {
        if (Toolkit.page.dataList.current) {
            if (Toolkit.page.dataList.current.hasClass('tk-page')) {
                Toolkit.page.dataList.current.trigger('refreshDataPage');
                return;
            }
            if (page) {
                Toolkit.page.dataList.current.bindToolkitData('LoadDataListByPage', page);
            } else {
                Toolkit.page.dataList.current.bindToolkitData('DataListRefresh');
            }
        }
    };
    // ----------------------------
    // 页面多Tab切换处理
    // Toolkit.page.tab
    Toolkit.page.showTab = function (type, tab, title, url) {
        if (!Toolkit.page.isMulTab) {
            Toolkit.page.showFrame(type, title, url);
            return;
        }
        var tabId = ('tk-tab-' + tab).toCamelCase();
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        if ($('#' + tabId).size() == 0) {
            Toolkit.page.createTab(type, tab, title, url);
        } else {
            if (Toolkit.page.checkTab(type, tab, title, url)) {
                $('#' + tabId).click();
                return;
            }
            Toolkit.page.updateTab(type, tab, title, url);
        }
        if ($('#' + pageId).size() == 0) {
            Toolkit.page.createPage(type, tab);
        }
        Toolkit.page.updatePage(type, tab, url);
        $('#' + tabId).click();
    };
    Toolkit.page.showFrame = function (type, title, url) {
        if (type == 'page') {
            Toolkit.page.alert('此框架不支持Ajax加载页面内容。');
            return;
        }
        var tab = 'main';
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        $('h2#tkTabs').html(title);
        if ($('#' + pageId).size() == 0) {
            Toolkit.page.createPage(type, tab);
            $('#' + pageId).css('display', 'block');
        }
        Toolkit.page.updatePage(type, tab, url);
    };
    Toolkit.page.createTab = function (type, tab, title, url) {
        var tabId = ('tk-tab-' + tab).toCamelCase();
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        var tab = $('<li id="{tabId}" dataPage="{pageId}" dataUrl="{url}" onclick="Toolkit.page.switchTab(this);"><span>{title}</span><del onclick="Toolkit.page.removeTab(this);"></del></li>'.substitute({ title: title, tabId: tabId, pageId: pageId, url: url }));
        $('#tkTabs').append(tab);
    };
    Toolkit.page.createPage = function (type, tab) {
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        var htmlstr = (type == 'frame') ? '<iframe id="{pageId}" name="{pageId}" src="about:blank" frameborder="0" allowTransparency="true" class="tk-frame" style="display:none;"></iframe>' : '<div id="{pageId}" class="tk-page clearfix dn"></div>';
        $('#tkPages').append(htmlstr.substitute({ pageId: pageId }));
    };
    Toolkit.page.checkTab = function (type, tab, title, url) {
        var tabId = ('tk-tab-' + tab).toCamelCase();
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        return type == 'page' && $('#' + tabId).attr('dataPage') == pageId && $('#' + tabId).attr('dataUrl') == url;
    };
    Toolkit.page.updateTab = function (type, tab, title, url) {
        var tabId = ('tk-tab-' + tab).toCamelCase();
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        $('#' + tabId).attr('dataPage', pageId);
        $('#' + tabId).attr('dataUrl', url);
        $('#' + tabId + ' span').html(title);
    };
    Toolkit.page.updatePage = function (type, tab, url) {
        var pageId = ('tk-' + type + '-' + tab).toCamelCase();
        if (type == 'frame') {
            $('#tkPages').css('overflow', 'hidden');
            Toolkit.page.showLoading('正在加载页面…');
            Toolkit.page.setLoadingTimer();
            $('#' + pageId).attr('src', url);
        } else {
            $('#tkPages').css('overflow', 'auto');
            $('#' + pageId).attr('dataSource', url);
            Toolkit.page.main.initPage($('#' + pageId));
        }
    };
    Toolkit.page.switchTab = function (obj) {
        var elm = $(obj);
        if (elm.hasClass('on') || elm.hasClass('disableSwitch')) return;
        elm.siblings('.on').removeClass('on');
        elm.addClass('on');
        $('#tkPages').children().hide();
        $('#' + elm.attr('dataPage')).show();
    };
    Toolkit.page.removeTab = function (obj) {
        var elm = $(obj).parent();
        elm.addClass('disableSwitch');
        Toolkit.page.confirm('确认删除“<strong>' + elm.text() + '</strong>”？', function () {
            elm.unbind('click');
            $('#' + elm.attr('dataPage')).remove();
            elm.remove();
        }, function () {
            elm.removeClass('disableSwitch');
        });
    };
    Toolkit.page.setTabTitle = function (title) {
        if (Toolkit.page.isMulTab) {
            $('ul#tkTabs li.on span').html(title);
        } else {
            $('h2#tkTabs').html(title);
        }
    };
    // ----------------------------
    // 框架页面内容生成和处理方法加载
    // Toolkit.page.main
    Toolkit.page.main = {};
    Toolkit.page.main.loadHTML = function (elm, callback) {
        if (elm.size() == 0) return;
        Toolkit.page.dataLoader.add();
        var url = elm.attr('dataSource');
        var path = url.getPathName();
        var param = url.getQueryJson();
        if (!elm.attr('enableCache')) {
            param._ = (new Date()).getTime();
        }
        elm.load(path + '?' + $.param(param), function () {
            if (callback) callback();
            Toolkit.page.dataLoader.complete();
        });
    };
    Toolkit.page.main.initPage = function (elm) {
        if (elm.size() == 0) return;
        Toolkit.page.main.loadHTML(elm, function () {
            elm.bindToolkitUI('PageContentExecute');
        });
    };
    Toolkit.page.main.initDropmenu = function () {
        var element = $('#tkDropmenu');
        Toolkit.page.main.loadHTML(element, function () {
            Toolkit.load('dropmenu', function () {
                element.find('ul.tk-dropmenu').dropmenu();
            });
            element.bindToolkitUI('PageLinksExecute');
        });
    };
    Toolkit.page.main.initAccordion = function () {
        var element = $('#tkAccordion');
        Toolkit.page.main.loadHTML(element, function () {
            Toolkit.load('jqueryui', function () {
                element.accordion({ header: ".acc-header", fillSpace: true });
            });
            if (element.find('.ui-treeview').size() > 0) {
                Toolkit.load('treeview', function () {
                    element.find('.ui-treeview').treeview()
                });
            }
            element.bindToolkitUI('PageLinksExecute');
        });
    };
    // ----------------------------
    // 页面主题样式切换处理
    // Toolkit.page.themes
    Toolkit.page.themes = {};
    Toolkit.page.themes.add = function (title, href) {
        if ($('link[title=' + title + ']').size() == 0) {
            $('head').append('<link rel="stylesheet" type="text/css" href="' + href + '" title="' + title + '" />');
        } else if (!$('link[title=' + title + ']').attr('disabled')) {
            return false;
        }
        $('link[title]').attr('disabled', true);
        $('link[title=' + title + ']').attr('disabled', false);
    };
    Toolkit.page.themes.save = function (title, href) {
        if (Toolkit.page.data && Toolkit.page.data.userinfo) {
            Toolkit.page.data.userinfo.userTheme = title;
        }
        // 保存样式的处理还没写，暂时保存在coolkie里。
        Toolkit.cookie.set('toolkit_user_theme', title);
    };
    Toolkit.page.themes.get = function (data) {
        if (data && data.themes && data.userinfo && data.userinfo.userTheme && data.themes[data.userinfo.userTheme]) {
            return {
                theme: data.userinfo.userTheme,
                data: data.themes[data.userinfo.userTheme]
            };
        }
        return null;
    };
    Toolkit.page.themes.init = function () {
        var htmlstr = '<li title="{title}" name="{key}" style="background-color:{color}"></li>';
        for (var item in Toolkit.page.data.themes) {
            var theme = Toolkit.page.data.themes[item];
            theme.key = item;
            $('#tkThemes').append(htmlstr.substitute(theme));
        }
        $('#tkThemes li').click(function () {
            if ($(this).hasClass('on')) return false;
            $(this).siblings('.on').removeClass('on');
            $(this).addClass('on');
            var theme = $(this).attr('name');
            var css = Toolkit.page.data.themes[theme].css;
            Toolkit.page.themes.add(theme, css);
            Toolkit.page.themes.save(theme, css);
            if (window.subWin) {
                subWin.Toolkit.page.themes.load(window);
            }
            Toolkit.debug('theme switch', theme + ',' + css);
        });
        //
        var userThemeInCookie = Toolkit.cookie.get('toolkit_user_theme') || 'default';
        if (userThemeInCookie != '') {
            Toolkit.namespace('Toolkit.page.data.userinfo');
            Toolkit.page.data.userinfo.userTheme = userThemeInCookie;
        }
        //
        Toolkit.page.themes.load();
    };
    Toolkit.page.themes.load = function (topWin) {
        var data = (topWin) ? topWin.Toolkit.page.data : Toolkit.page.data;
        var currentTheme = Toolkit.page.themes.get(data);
        if (currentTheme) {
            Toolkit.page.themes.add(currentTheme.theme, currentTheme.data.css);
            if (!topWin) {
                $('#tkThemes li[name=' + currentTheme.theme + ']').addClass('on');
            }
        }
    };
    // ----------------------------
    // 页面初始化
    Toolkit.page.init = function () {
        Toolkit.stat.addFuncStat('Toolit.page.init');
        if (Toolkit.page.isTopWin) {
            Toolkit.page.main.initDropmenu();
            Toolkit.page.main.initAccordion();
            Toolkit.page.themes.init();
            $('.tk-headbar,.tk-sidebar').bindToolkitUI('PageLinksExecute');
            // 加载用户首页
            var page = { type: 'frame', target: 'home', title: '首页', url: '' };
            if (Toolkit.page.data.mainPage) $.extend(page, Toolkit.page.data.mainPage);
            Toolkit.page.showTab(page.type, page.target, page.title, page.url);
        } else {
            Toolkit.page.showLoading('加载页面处理中……');
            // 绑定页面事件处理
            $('body').bindToolkitUI('PageContentExecute');
            Toolkit.page.hideLoading();
        }
        Toolkit.page.event.trigger('Init');
        Toolkit.stat.addFuncStat('Toolit.page.init');
    };
    Toolkit.page.initBase = function () {
        if ($.browser.msie && $.browser.IEMode < 8 && !Toolkit.page.isSubWin && !Toolkit.page.isDlgWin) {
            if (document.documentMode) {
                var str = '您的IE浏览器目前被设置为' + ($.browser.IEMode == 7 ? 'IE7标准的浏览器模式' : 'Quirks模式的内容渲染方式') + '。请及时调整浏览器的设置，以便于正常使用本系统。<br>';
                str += '<div class="pt5 btdc mt5 f12 pl20 c3">';
                str += '<strong>调整设置的方法：</strong><br>';
                str += '按“F12”键，在打开的开发人员工具条里，将浏览器模式和文本模式设置为最高的版本，直到此提醒条消失。<br>';
                str += '如下例，将浏览器模式设置为“Internet Explorer 8”<br><span class="img-iedevtoolbar"></span>';
                str += '</div>';
                Toolkit.page.showMessage(str);
            } else {
                var str = '您的IE浏览器版本过低，影响了系统功能的正常使用，请升级您的IE浏览器到IE8或以上版本，或者换用火狐浏览器、谷歌浏览器等。';
                str += '<div class="pt5 btdc mt5 f12 pl20 c3">';
                str += '<a class="ico-l-down" href="http://windows.microsoft.com/zh-CN/internet-explorer/downloads/ie-8" target="_blank">下载 Internet Explorer 8</a>';
                str += '<a class="ico-l-down ml20" href="http://firefox.com.cn/download/" target="_blank">下载 Firefox 火狐浏览器</a>';
                str += '<a class="ico-l-down ml20" href="http://www.google.cn/chrome/intl/zh-CN/landing_chrome.html?hl=zh-cn" target="_blank">下载 Chrome 谷歌浏览器</a>';
                str += '</div>';
                Toolkit.page.showMessage(str);
            }
        }
        if (!Toolkit.page.isTopWin) {
            if (Toolkit.page.isSubWin) {
                top.Toolkit.page.setTabTitle(document.title);
                $("#tkFrameMain").attr("data-Url", document.location.href);
                top.Toolkit.page.clearLoadingTimer();
                top.subWin = window;
                Toolkit.page.themes.load(top);
            } else if (Toolkit.page.isDlgWin) {
                //	parent.Toolkit.util.dialog.setTitle(document.title);
                Toolkit.page.themes.load(top);
            }
        }
    };
    Toolkit.page.refreshMainframe = function () {
        var iframe = $("#tkFrameMain");
        iframe.attr("src", iframe.attr("data-Url"));
    };
    //---------
    // ----------------------------
    $(document).ready(function () {
        Toolkit.page.initBase();
        $(".TitleSet").each(function () { if ($(this).text().trim() != "") $(this).attr("title", $(this).text()); });
    });
})(jQuery);

