// --------------------------------
// Toolkit.page
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.weixin');

    Toolkit.weixin._joinUrl = function (url) {
        if (url === null || url === undefined || url === "")
            return "";

        var tempUri = new URI(url);
        if (tempUri.is("absolute"))
            return url;

        var baseUrl = new URI(document.location.href);
        if (url.startWith("~/")) {
            var newurl = $("body").attr("data-webPath") + url.substring(2);
            tempUri = new URI(newurl);
        }
        return tempUri.absoluteTo(baseUrl).toString();
    };

    Toolkit.weixin._ready = function () {
        wx.ready(function () {
            wx.error(function (res) {
                //alert(res.errMsg);
            });

            var meta = $("#metaData");

            if (meta.length === 0)
            {
                wx.hideOptionMenu();
                return;
            }

            if (meta.attr("data-option") == "false")
                wx.hideOptionMenu();
            else
                wx.showOptionMenu();

            var title = meta.attr("data-title");
            var description = meta.attr("data-desc");
            var link = Toolkit.weixin._joinUrl(meta.attr("data-link"));
            var imgUrl = Toolkit.weixin._joinUrl(meta.attr("data-img"));

            wx.onMenuShareTimeline({
                title: title,
                link: link,
                imgUrl: imgUrl,
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareAppMessage({
                title: title, 
                desc: description, 
                link: link, 
                imgUrl: imgUrl, 
                type: 'link',
                dataUrl: '', 
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareQQ({
                title: title,
                desc: description, 
                link: link, 
                imgUrl: imgUrl,
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareWeibo({
                title: title,
                desc: description,
                link: link, 
                imgUrl: imgUrl,
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        });
    };
    Toolkit.weixin.register = function () {
        var localUrl = location.href.split('#')[0];
        var wxUrl = Toolkit.page.getFullUrl("Library/WebListXmlPage.tkx?Source=WeixinJsConfig&useSource=true");
        wxUrl = Toolkit.page.addQueryString(wxUrl, { "Url": localUrl });

        var jsApiList = [
                'onMenuShareTimeline',
                'onMenuShareAppMessage',
                'onMenuShareQQ',
                'onMenuShareWeibo',
                'startRecord',
                'stopRecord',
                'onVoiceRecordEnd',
                'playVoice',
                'pauseVoice',
                'stopVoice',
                'onVoicePlayEnd',
                'uploadVoice',
                'downloadVoice',
                'chooseImage',
                'previewImage',
                'uploadImage',
                'downloadImage',
                'translateVoice',
                'getNetworkType',
                'openLocation',
                'getLocation',
                'hideOptionMenu',
                'showOptionMenu',
                'hideMenuItems',
                'showMenuItems',
                'hideAllNonBaseMenuItem',
                'showAllNonBaseMenuItem',
                'closeWindow',
                'scanQRCode',
                'chooseWXPay',
                'openProductSpecificView',
                'addCard',
                'chooseCard',
                'openCard'
        ];
        $.ajax({
            url: wxUrl,
            type: 'get', dataType: 'json',
            success: function (data) {
                var timestamp = data.timestamp,
                    nonceStr = data.nonceStr,
                    signature = data.signature,
                    appId = data.appId;

                wx.config({
                    debug: false,
                    appId: appId,
                    timestamp: timestamp,
                    nonceStr: nonceStr,
                    signature: signature,
                    jsApiList: jsApiList
                });

                Toolkit.weixin._ready();
            }
        });
    };
    $(document).ready(function () {
        Toolkit.weixin.register();
    });
})(jQuery);

