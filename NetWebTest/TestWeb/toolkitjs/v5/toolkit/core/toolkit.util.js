// --------------------------------
// Toolkit.util
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.util');
	// ----------------------------
	// 加载Flash
	Toolkit.util.flash = function(pSwf, pBoxID, pW, pH, pVer, pSetup, pFlashvars, pParams, pAttr) {
		Toolkit.load('swfobject', function() {
			swfobject.embedSWF(pSwf, pBoxID, pW, pH, pVer, pSetup||false, pFlashvars||false, pParams||false, pAttr||false);
		});
	};
	// ----------------------------
	// 遮盖层插件 Toolkit.util.overlayer
	Toolkit.util.overlayer = {};
	Toolkit.util.overlayer.id = 'util-overlayer';
	Toolkit.util.overlayer.status = false;
	// 显示遮盖层
	Toolkit.util.overlayer.showLayer = function(op, bg) {
		if (this.status) return false;
		op = (typeof(op)!='undefined')?op:0.3;
		bg = bg || '#000';
		if ($('#'+this.id).length==0) $('body').append('<div id="'+this.id+'" style="width:100%;height:100%;position:absolute;top:0; left:0;right:0;bottom:0;display:none;z-index:9990;"></div>');
		$('#'+this.id).css({background:bg,filter:('alpha(opacity='+op*100+')'),opacity:op});
		this.setSize();
		this.status = true;
	};
	// 关闭遮盖层
	Toolkit.util.overlayer.hideLayer = function() {
		if (this.status) $('#'+this.id).css('display','none');
		this.status = false;
	};
	// 设置遮盖层大小
	Toolkit.util.overlayer.setSize = function() {
		$('#'+this.id).css({ width:'100%', height:$(document).height()+'px', display:'block'});
	};
	// 自适应屏幕调整遮盖层大小
	Toolkit.util.overlayer.resize = function() {
		if (this.status) { $('#'+this.id).css('display','none'); this.setSize(); }
	};
	// ----------------------------
	$(window).resize(function() {
		Toolkit.util.overlayer.resize();
	});
	// ----------------------------
})(jQuery);