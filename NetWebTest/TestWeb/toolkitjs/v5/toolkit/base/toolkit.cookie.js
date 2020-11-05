// --------------------------------
// Toolkit.cookie
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.cookie');
	// ----------------------------
	Toolkit.cookie.get = function(name) {
		var cname = name +'=';
		var dc = document.cookie;
		if (dc.length > 0) {
			begin = dc.indexOf(cname);
			if (begin!=-1) {
				begin += cname.length;
				end = dc.indexOf(';',begin);
				if (end==-1) end = dc.length;
				return unescape(dc.substring(begin,end));
			}
		}
		return '';
	};
	Toolkit.cookie.del = function(name) {
		document.cookie = name +'=; path=/; expires=Thu,01-Jan-70 00:00:01 GMT';
	};
	Toolkit.cookie.set = function(name, value, expdate) {
		var exp = new Date();
		exp.setTime(exp.getTime() + (1000 * 60 * 60 * 24 * (expdate||365)));
		document.cookie = name +'='+ escape(value) +'; domain='+ document.domain +'; path=/'+ ((exp==null)?' ':('; expires='+exp.toGMTString()));
	};
	// ----------------------------
})(jQuery);