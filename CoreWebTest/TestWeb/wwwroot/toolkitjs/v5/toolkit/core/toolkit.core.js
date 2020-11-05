// --------------------------------
// Toolkit.namespace/debug/load
// --------------------------------
(function($) {
	// ----------------------------
	if (!$ || window.Toolkit) return;
	// ----------------------------
	window.Toolkit = {};
	jQuery.ajaxSetup({ cache: false });
	// ----------------------------
	// Toolkit.namespace 命名空间
	Toolkit.namespace = function(name, sep) {
		var s = name.split(sep||'.'), d = {}, o = function(a, b, c) {
			if (c < b.length) {
				if (!a[b[c]]) {
					a[b[c]] = {};
				}
				d = a[b[c]];
				o(a[b[c]], b, c+1);
			}
		};
		o(window, s, 0);
		return d;
	};
	// ----------------------------
	// Toolkit.load/Toolkit.loader 加载管理
	Toolkit.load = function(service, action, params) {
		if ($.isArray(service)) {
			var url = service.join(',');
			var urlsize = service.length;
			var status = Toolkit.loader.checkFileLoader(url);
			if (status == urlsize+1) {
				if (typeof(action)=='function') action();
			} else if (status > 0) {
				Toolkit.loader.addExecute(url, action);
			} else if (status == 0) {
				Toolkit.loader.addExecute(url, action);
				Toolkit.loader.fileLoader[url] = 1;
				Toolkit.debug('开始加载JS', url);
				for (var i=0; i<urlsize; i++) {
					Toolkit.load(service[i], function() {
						Toolkit.loader.fileLoader[url] ++;
						if (Toolkit.loader.fileLoader[url] == urlsize+1) {
							Toolkit.debug('完成加载JS', url);
							Toolkit.loader.execute(url);
						}
					});
				}
			}
		} else if (Toolkit.loader.serviceLibs[service] && Toolkit.loader.serviceLibs[service].requires) {
			Toolkit.load(Toolkit.loader.serviceLibs[service].requires, function() {
				Toolkit.load.run(service, action, params);
			});
		} else {
			Toolkit.load.run(service, action, params);
		}
	};
	Toolkit.load.add = function(key, data) {
		if (Toolkit.loader.serviceLibs[key]) return;
		Toolkit.loader.serviceLibs[key] = data;
	};
	Toolkit.load.extend = function(key, data) {
		if (!Toolkit.loader.serviceLibs[key]) return;
		$.extend(Toolkit.loader.serviceLibs[key], data);
	}; 
	Toolkit.load.setPath = function(path) {
		Toolkit.loader.serviceBase = path;
	};
	Toolkit.load.run = function(service, act, params) {
		var action = (typeof(act)=='string') ? (function() {
			try {
				var o = eval('Toolkit.'+ service);
				if (o && o[act]) o[act](params);
			} catch(e) {}
		}) : (act||function(){});
		if (Toolkit.loader.checkService(service)) {
			action();
			return;
		}
		// status:-1异常, 0未加载, 1开始加载, 2完成加载
		var url = Toolkit.loader.getServiceUrl(service);
		var status = Toolkit.loader.checkFileLoader(url);
		if (status == 2) {
			action();
		} else if (status == 1) {
			Toolkit.loader.addExecute(url, action);
		} else if (status == 0) {
			if ($('script[src="'+url+'"]').size()>0) {
				Toolkit.loader.fileLoader[url] = 2;
				action();
			} else {					
				Toolkit.loader.addExecute(url, action);
				Toolkit.loader.addScript(service);
			}
		} else {
			Toolkit.debug('加载异常', service);					
		}
	};
	// ----------------------------
	Toolkit.loader = {};
	Toolkit.loader.fileLoader = {};
	Toolkit.loader.executeLoader = {};
	Toolkit.loader.serviceBase = (function() {
		return $('script:last')[0].src.sliceBefore('toolkit/');
	})();
	Toolkit.loader.serviceLibs = {};
	Toolkit.loader.checkService = function(service) {
		if (service.isURL()) return false;
		try {
			if (service.indexOf('.')>0) {
				var o = eval('Toolkit.'+service);
				return (typeof(o)!='undefined');
			}
			return false;
		} catch(e) {
			return false;
		}
	};
	Toolkit.loader.checkFileLoader = function(url) {
		return (url!='') ? (this.fileLoader[url] || 0) : -1;
	};
	Toolkit.loader.getServiceUrl = function(service) {
		var url = '';
		if (service.isURL()) {
			url = service;
		} else if (this.serviceLibs[service]) {
			url = (this.serviceLibs[service].js.isURL()) ? this.serviceLibs[service].js : (this.serviceBase + this.serviceLibs[service].js);
		}
		return url;
	};
	Toolkit.loader.execute = function(url) {
		if (this.executeLoader[url]) {
			for (var i=0; i<this.executeLoader[url].length; i++) {
				this.executeLoader[url][i]();
			}
			this.executeLoader[url] = null;
		}
	};
	Toolkit.loader.addExecute = function(url, action) {
		if (typeof(action)!='function') return;
		if (!this.executeLoader[url]) this.executeLoader[url] = [];
		this.executeLoader[url].push(action);
	};
	Toolkit.loader.addScript = function(service) {
		var this_ = this;
		if (service.isURL()) {
			var url = service;
			this.getScript(url, function() {
				this_.fileLoader[url] = 2;
				Toolkit.debug('完成加载JS', url);
				Toolkit.loader.execute(url);
			});
		} else if (this.serviceLibs[service]) {
			if (this.serviceLibs[service].css) {
				var url = (this.serviceLibs[service].css.isURL()) ? this.serviceLibs[service].css : (this.serviceBase + this.serviceLibs[service].css);
				if (!this.fileLoader[url]) {
					$('head').append('<link rel="stylesheet" type="text\/css"  href="'+  url +'" \/>');
					this.fileLoader[url] = 1;
					Toolkit.debug('开始加载CSS', url);
				}
			}
			if (this.serviceLibs[service].js) {
				var url = (this.serviceLibs[service].js.isURL()) ? this.serviceLibs[service].js : (this.serviceBase + this.serviceLibs[service].js);
				this.getScript(url, function() {
					this_.fileLoader[url] = 2;
					Toolkit.debug('完成加载JS', url);
					Toolkit.loader.execute(url);
				});
			}
		}
	};
	Toolkit.loader.getScript = function(url, onSuccess, onError) {
	//	$.ajax({type:"GET", url:url, cache:true, success:onSuccess, error:onError, dataType:'script'});
		this.getRemoteScript(url, onSuccess, onError);
		this.fileLoader[url] = 1;
		Toolkit.debug('开始加载JS', url);
	};
	Toolkit.loader.getRemoteScript = function(url, onSuccess, onError) {
		var head = document.getElementsByTagName("head")[0];
		var script = document.createElement("script");
		script.type = 'text/javascript';
		script.src = url;
		script.onload = script.onreadystatechange = function(){
			if ( !this.readyState || this.readyState == "loaded" || this.readyState == "complete") {
				if (onSuccess) onSuccess();
				script.onload = script.onreadystatechange = null;
				head.removeChild(script);
			}
		};
		script.onerror = function() {
			if (onError) onError();
		};
		head.appendChild(script);
	};
	// ----------------------------
	// Toolkit.debug 过程调试
	Toolkit.debugMode = false;
	Toolkit.debugIndex = 0;
	Toolkit.debug = function(a, b) {
		if (!this.debugMode) return;
		if (typeof(console) == 'undefined') {
			Toolkit.load('firebug', function() {
				if (b == undefined) {
					console.log(((Date.prototype.format)?(new Date()).format('hh:nn:ss.S'):(++Toolkit.debugIndex)) +', '+ a);
				} else {
					console.log(((Date.prototype.format)?(new Date()).format('hh:nn:ss.S'):(++Toolkit.debugIndex)) +', '+ a, '=', b);
				}
			});
		} else {
			if (console && console.log) {
				if (b == undefined) {
					console.log(((Date.prototype.format)?(new Date()).format('hh:nn:ss.S'):(++Toolkit.debugIndex)) +', '+ a);
				} else {
					console.log(((Date.prototype.format)?(new Date()).format('hh:nn:ss.S'):(++Toolkit.debugIndex)) +', '+ a, '=', b);
				}
			}
		}
	};
	// ----------------------------
	// JS相关信息文本（用于多语言支持）
	Toolkit.lang = {};
	Toolkit.lang.language = 'zh-cn'; //用于部分插件的初始语言版本的js的选用
	Toolkit.lang.text = {};
	// ----------------------------
	Toolkit.lang.get = function(dataset, name) {
		if (name) {
			if (this.text[dataset]) {
				return this.text[dataset][name] || '';
			} else {
				return '';
			}
		} else {
			return this.text[dataset] || null; 
		}
	};
	Toolkit.lang.set = function(dataset, name, value) {
		if (!this.text[dataset]) {
			this.text[dataset] = {};
		}
		if (value) {
			this.text[dataset][name] = value;
		} else {
			this.text[dataset] = name;
		}
	};
	Toolkit.lang.extend = function(dataset, data) {
		if (!this.text[dataset]) {
			this.text[dataset] = {};
		}
		$.extend(this.text[dataset], data);
	};
	// ----------------------------
})(jQuery);