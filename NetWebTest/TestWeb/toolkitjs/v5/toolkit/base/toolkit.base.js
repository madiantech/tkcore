// --------------------------------
// Toolkit.base
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.base');
	// ----------------------------
	// StringBuilder: 字符串连接
	Toolkit.base.StringBuilder = function() {
		this._strings = [];
	};
	$.extend(Toolkit.base.StringBuilder.prototype, {
		append: function(str) {
			var aLen = arguments.length;
			for (var i=0; i<aLen; i++) {
				this._strings.push(arguments[i]);
			}
		},
		appendFormat: function(fmt) {
			var re = /{[0-9]+}/g;
			var aryMatch = fmt.match(re);
			var aLen = aryMatch.length;
			for (var i=0; i < aLen; i++) {
				fmt = fmt.replace(aryMatch[i], arguments[parseInt(aryMatch[i].replace(/[{}]/g, "")) + 1]);
			}
			this._strings.push(fmt);
		},
		toString: function() {
			return this._strings.join("");
		}
	});
	// ----------------------------
	// ImageLoader: 图像加载器
	Toolkit.base.ImageLoader = function(options) {
		this.options = $.extend({src:'', min:0.5, max:10, timer:0.1}, options || {});
		this.minTimeout = this.options.min * 1000;
		this.maxTimeout = this.options.max * 1000;
		this.intervalTimer = this.options.timer * 1000;
		this.theTimeout = 0;
		this.loadStatus = 0;
		this.loaderId = (new Date()).getTime();
		this.onLoad = this.options.onLoad || null;
		this.onError = this.options.onError || null;
		var m_this = this;
		this.init = function() {
			this.element = new Image();
			this.element.onload = function() {
				m_this.loadStatus = 1;
				if (m_this.onLoad) m_this.onLoad();
				Toolkit.debug('image onload', m_this.loaderId +': '+ this.width +','+ this.height);
			}
			this.element.onerror = function() {
				m_this.loadStatus = -1;
				if (m_this.onError) m_this.onError();
				Toolkit.debug('image onerror', m_this.loaderId);
			}
			this.element.src = this.options.src;
			this.startMonitor();
			Toolkit.debug('image init', m_this.loaderId);
		};
		this.startMonitor = function() {
			var m_this = this;
			setTimeout(function() {
				m_this.checkMonitor();
			}, this.minTimeout);
		};
		this.checkMonitor = function() {
			if (this.loadStatus != 0) return;
			this.theTimeout = this.minTimeout;
			this._monitor = setInterval(function() {
				m_this.theTimeout += 50;
				if (m_this.loadStatus != 0) {
					clearInterval(m_this._monitor);
				} else if (m_this.element.complete) {
					clearInterval(m_this._monitor);
					m_this.loadStatus = 1;
					if (m_this.onLoad) m_this.onLoad();
					Toolkit.debug('image complete', m_this.loaderId +': '+ m_this.element.width +','+ m_this.element.height);
				} else if (m_this.theTimeout >= m_this.maxTimeout) {
					clearInterval(m_this._monitor);
					m_this.loadStatus = -1;
					if (m_this.onError) m_this.onError();
					Toolkit.debug('image timeout', m_this.loaderId);
				}
			}, m_this.intervalTimer);
		};
		this.init();
	};
	// ----------------------------
})(jQuery);