// --------------------------------
// jQuery extend
// --------------------------------
(function($) {
	if (!$) return;
	// ----------------------------
	// $.browser方法扩展
	$.extend($.browser, {
		'client': function() {
			return {
				width: document.documentElement.clientWidth,
				height: document.documentElement.clientHeight,
				bodyWidth: document.body.clientWidth,
				bodyHeight: document.body.clientHeight
			};
		},
		'scroll': function() {
			return {
				width: document.documentElement.scrollWidth,
				height: document.documentElement.scrollHeight,
				bodyWidth: document.body.scrollWidth,
				bodyHeight: document.body.scrollHeight,
				left: Math.max(document.documentElement.scrollLeft, document.body.scrollLeft),
				top: Math.max(document.documentElement.scrollTop, document.body.scrollTop)
			};
		},
		'screen': function() {
			return {
				width: window.screen.width,
				height: window.screen.height
			};
		},
		//'isMinW': function(val) {
		//	return Math.min($.browser.client().bodyWidth, $.browser.client().width) <= val;
		//},
		//'isMinH': function(val) {
		//	return Math.min($.browser.client().bodyHeight, $.browser.client().height) <= val;
		//},
		//'isIE6': $.browser.msie && $.browser.version == 6,
		//'IEMode': (function() {
		//	if ($.browser.msie)	{
		//		if (document.documentMode) { return document.documentMode; }  // IE8
		//		if (document.compatMode && document.compatMode=='CSS1Compat') { return 7; }
		//		return 5; // quirks mode
		//	}
		//	return 0;
		//})(),
		'isIPad': (/iPad/i).test(navigator.userAgent),
		'language': function() {
			return (navigator.language || navigator.userLanguage || '').toLowerCase();
		}
	});
	// ----------------------------
	// 获取tagName
	$.fn.tagName = function() { 
		if (this.length==0) return '';
		return this[0].tagName.toLowerCase();
	};
	// 获取select的文本
	$.fn.optionText = function() {
		if (this.length==0) return '';
		var sel = this[0];		
		if (sel.selectedIndex==-1) return '';
		return sel.options[sel.selectedIndex].text;
	};
	// 获取element属性的JSON值
	$.fn.attrJSON = function(attr) {
		return (this.attr(attr||'rel')||'').parseAttrJSON();
	};
	// 绑定jQueryUI事件处理
	$.fn.bindJqueryUI = function(action, params) {
		if (this.length==0) return;
		var elm = this;
		Toolkit.load('jqueryui', function() {
			elm[action](params);
		});
		return this;
	};
	// 绑定Toolkit.ui事件处理
	$.fn.bindToolkitUI = function(type, params) {
		if (this.length==0 || !Toolkit) return this;
		if (Toolkit.ui && Toolkit.ui[type]) {
			Toolkit.ui[type](this, params);
		}
		return this;
	};
	// 绑定Toolkit.ui扩展事件处理
	$.fn.bindToolkitUIExtend = function(file, type, params) {
		if (this.length==0 || !Toolkit) return this;
		var elm = this;
		Toolkit.load(file, function() {
			if (!Toolkit.ui[type]) return;
			Toolkit.ui[type](elm, params);
		});
		return this;
	};
	// 绑定Toolkit.data数据处理
	$.fn.bindToolkitData = function(type, params) {
		if (this.length==0 || !Toolkit) return this;
		if (Toolkit.data && Toolkit.data[type]) {
			Toolkit.data[type](this, params);
		}
		return this;
	};
	// ----------------------------
})(jQuery);