function joinUrl(url) {
    if (url === null || url === undefined || url === "")
        return "";

    var tempUri = new URI(url);
    if (tempUri.is("absolute"))
        return url;

    var baseUrl = new URI(document.location.href);
    if (url.startWith("~/"))
    {
        var newurl = $("body").attr("data-webPath") + url.substring(2);
        tempUri = new URI(newurl);
    }
    return tempUri.absoluteTo(baseUrl).toString();
}

function onBridgeReady() {
    var meta = $("#metaData");

    if (meta.length == 0) {
        WeixinJSBridge.call('hideOptionMenu');
        WeixinJSBridge.call('hideToolbar');
        return;
    }

    if (meta.attr("data-option") == "false")
        WeixinJSBridge.call('hideOptionMenu');
    else
        WeixinJSBridge.call('showOptionMenu');
    var toolbar = meta.attr("data-toolbar");
    if (toolbar == "true")
        WeixinJSBridge.call('showToolbar');
    else
        WeixinJSBridge.call('hideToolbar');

    var title = meta.attr("data-title");
    var description = meta.attr("data-desc");
    var link = joinUrl(meta.attr("data-link"));
    var imgUrl = joinUrl(meta.attr("data-img"));

    // 发送给好友; 
    WeixinJSBridge.on('menu:share:appmessage', function (argv) {
        WeixinJSBridge.invoke('sendAppMessage', {
            //"appid"      : appId,
            "img_url": imgUrl,
            "img_width": "640",
            "img_height": "640",
            "link": link,
            "desc": description,
            "title": title
        }, function (res) {
            _report('send_msg', res.err_msg);
        });
    });

    // 分享到朋友圈;
    WeixinJSBridge.on('menu:share:timeline', function (argv) {
        WeixinJSBridge.invoke('shareTimeline', {
            "img_url": imgUrl,
            "img_width": "640",
            "img_height": "640",
            "link": link,
            "desc": description,
            "title": title
        }, function (res) {
            _report('timeline', res.err_msg)
        });

    });

    // 分享到微博;
    WeixinJSBridge.on('menu:share:weibo', function (argv) {
        WeixinJSBridge.invoke('shareWeibo', {
            "content": description,
            "url": link
        }, function (res) {
            _report('weibo', res.err_msg);
        });
    });

    // 分享到Facebook
    WeixinJSBridge.on('menu:share:facebook', function (argv) {
        WeixinJSBridge.invoke('shareFB', {
            "img_url": imgUrl,
            "img_width": "640",
            "img_height": "640",
            "link": link,
            "desc": description,
            "title": title
        }, function (res) { });
    });
}

document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
