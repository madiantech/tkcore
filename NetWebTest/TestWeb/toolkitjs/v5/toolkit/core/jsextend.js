// --------------------------------
// String/Number/Array/Date.prototype
// --------------------------------
(function($) {
	if (!$) return;
	// ----------------------------
	// String原型方法扩展
	$.extend(String.prototype, {
		'trim':  function() { 
			return this.replace(/(^\s*)|(\s*$)/g,'');
		},
		'ltrim': function() {
			return this.replace(/(^\s*)/g,'');
		},
		'rtrim': function() { 
			return this.replace(/(\s*$)/g,'');
		},
		'trimAll': function() {
			return this.replace(/\s/g,'');
		},
		'left': function(len) {
			return this.substring(0, len);
		},
		'right': function(len) {
			if (this.length <= len) {
				return this;
			}
			return this.substring(this.length-len, this.length);
		},
		'reverse': function() {
			return this.split('').reverse().join('');
		},
		'startWith': function(start, noCase) {
			var index = noCase ? this.toLowerCase().indexOf(start.toLowerCase()) : this.indexOf(start);
			return !index;
		},
		'endWith': function(end, noCase) {
			return noCase ? (new RegExp(end.toLowerCase()+"$").test(this.toLowerCase().trim())) : (new RegExp(end+"$").test(this.trim()));
		},
		'sliceAfter': function(str) {
			if (this.indexOf(str)<0) return '';
			return this.substring(this.indexOf(str)+str.length, this.length);
		},
		'sliceBefore': function(str) {
			if (this.indexOf(str)<0) return '';
			return this.substring(0, this.indexOf(str));
		},
		'getByteLength':  function() {
			return this.replace(/[^\x00-\xff]/ig, 'xx').length;
		},
		'subByte': function(len, s) {
			if (len<0 || this.getByteLength()<=len) {
				return this;
			}
			var str = this;
			str = str.substr(0,len).replace(/([^\x00-\xff])/g,"\x241 ").substr(0,len).replace(/[^\x00-\xff]$/,"").replace(/([^\x00-\xff]) /g,"\x241");
			return str + (s||'');
		},
		'textToHtml': function() { 
			return this.replace(/</ig,'&lt;').replace(/>/ig,'&gt;').replace(/\r\n/ig,'<br>').replace(/\n/ig,'<br>');
		},
		'htmlToText': function() { 
			return this.replace(/<br>/ig, '\r\n');
		},
		'htmlEncode': function() {
			var text = this; var re = {'<':'&lt;','>':'&gt;','&':'&amp;','"':'&quot;'};
			for (i in re) {
				text = text.replace(new RegExp(i,'g'), re[i]);
			}
			return text;
		},
		'htmlDecode': function() {
			var text = this;
			var re = {'&lt;':'<','&gt;':'>','&amp;':'&','&quot;':'"'};
			for (i in re) {
				text = text.replace(new RegExp(i,'g'), re[i]);
			}
			return text;
		},
		'stripHtml': function() {
			return this.replace(/(<\/?[^>\/]*)\/?>/ig, ''); 
		},
		'stripScript': function() {
			return this.replace(/<script(.|\n)*\/script>\s*/ig, '').replace(/on[a-z]*?\s*?=".*?"/ig,'');
		},		
		'replaceAll': function(os, ns) {
			return this.replace(new RegExp(os,"gm"),ns); 
		},
		'escapeReg': function() {
			return this.replace(new RegExp("([.*+?^=!:\x24{}()|[\\]\/\\\\])", "g"), '\\\x241');
		},
		'getQueryValue': function(name) {
			var reg = new RegExp("(^|&|\\?|#)" + name.escapeReg() + "=([^&]*)(&|\x24)", "");
			var match = this.match(reg);
   			return (match) ? match[2] : '';
		},
		'getQueryJson': function() {
			if (this.lastIndexOf('?')==-1 || this.right(1)=='?') return {};
			var query = this.substr(this.indexOf('?') + 1), params = query.split('&'), len = params.length, result = {}, key, value, item, param;
			for (var i=0; i < len; i++) {
				param = params[i].split('=');
				key = param[0];
				value = param[1];				
				item = result[key];
				if ('undefined' == typeof item) {
					result[key] = unescape(value);
				} else if (Object.prototype.toString.call(item) == '[object Array]') {
					item.push(value);
				} else {
					result[key] = [item, value];
				}
			}
			return result;
		},
		'getPathName': function() {
			return (this.lastIndexOf('?')==-1)?this.toString():this.substring(0,this.lastIndexOf('?'));
		},
		'getFilePath': function() {
			return this.substring(0,this.lastIndexOf('/')+1);
		},
		'getFileName': function() {
			return this.substring(this.lastIndexOf('/')+1);
		},
		'getFileExt': function() {
			return this.substring(this.lastIndexOf('.')+1);
		},
		'parseDate': function() {
			return (new Date()).parse(this.toString());
		},
		'parseJSON': function() {
			try {
				return !(/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(this.replace(/"(\\.|[^"\\])*"/g, ''))) && eval('(' + this.toString() + ')');
			} catch (e) {
				return false;
			}
		},
		'parseAttrJSON': function() {
			var d = {}, a = this.toString().split(';');
			for (var i=0; i<a.length; i++) {
				if (a[i].trim()=='' || a[i].indexOf(':')<1) continue;
				var b = a[i].split(':');
				if (b[0].trim()!='' && b[1].trim()!='') d[b[0].toCamelCase()] = b[1];
			}
			return d;
		},
		'isURL': function() {
			return (new RegExp(/(http[s]?|ftp):\/\/[^\/\.]+?\..+\w$/i).test(this.trim()));
		},
		'_pad': function(width, ch, side) {
			var str = [side?'':this, side?this:''];
			while (str[side].length < (width?width:0) && (str[side] = str[1] + (ch||' ') + str[0]));
			return str[side];
		},
		'padLeft': function(width, ch) {
			if (this.length>=width) return this.toString();
			return this._pad(width, ch, 0);
		},
		'padRight': function(width, ch) {
			if (this.length>=width) return this.toString();
			return this._pad(width, ch, 1);
		},
		'toHalfWidth': function() {
			return this.replace(/[\uFF01-\uFF5E]/g, function(c) {
				return String.fromCharCode(c.charCodeAt(0) - 65248);
			}).replace(/\u3000/g, " ");
		},
		'toCamelCase': function() {
			if (this.indexOf('-') < 0 && this.indexOf('_') < 0) {
				return this; 
			}
			return this.replace(/[-_][^-_]/g, function (match) {
				return match.charAt(1).toUpperCase();
			});
		},
		'format': function() {
			var result = this;
			if (arguments.length > 0) {
				parameters = $.makeArray(arguments);
				$.each(parameters, function(i, n) {
					result = result.replace(new RegExp("\\{" + i + "\\}", "g"), n);
				});
			}
			return result;
		},
		'substitute': function(data) {
			if (data && typeof(data)=='object') {
				return this.replace(/\{([^{}]+)\}/g, function(match, key) {
					var value = data[key];
					return (value !== undefined) ? ''+value :'';
				});
			} else {
				return this.toString();
			}   
		}
	});
	// Number原型方法扩展
	$.extend(Number.prototype, {
		'comma': function(length) {
			if (!length || length < 1) length = 3;
			source = (''+this).split(".");
			source[0] = source[0].replace(new RegExp('(\\d)(?=(\\d{'+length+'})+$)','ig'),"$1,");
			return source.join(".");
		},
		'randomInt': function(min, max) { 
			return Math.floor(Math.random() * (max - min + 1) + min);
		},
		'padLeft': function(width, ch) {
			return (''+this).padLeft(width, ch);
		},
		'padRight': function(width, ch) {
			return (''+this).padRight(width, ch);
		}
	});
	// Array原型方法扩展
	$.extend(Array.prototype, {
		'indexOf': function(item, it) {
			for (var i=0; i<this.length; i++) {
				if (item == ((it)?this[i][it]:this[i])) return i;
			}
			return -1;			
		},
		'remove': function(item, it) {
			this.removeAt(this.indexOf(item, it));
		},
		'removeAt': function(idx) {
			if (idx >= 0 && idx < this.length) {
				for (var i=idx; i<this.length-1; i++) {
					this[i] = this[i+1];
				}
				this.length --;
			}
		},
		'removeEmpty': function() {
			var arr = [];
			for (var i=0; i<this.length; i++) {
				if (this[i].trim()!='') {
					arr.push(this[i].trim());
				}
			}
			return arr;
		},
		'add': function(item) {
			if (this.indexOf(item)>-1) {
				return false;
			} else {
				this.push(item);
				return true;
			}
		},
		'swap': function(i, j) {
			if (i<this.length && j<this.length && i!=j) {
				var item = this[i];
				this[i] = this[j];
				this[j] = item;
			}
		},
		// 过滤数组，两种过滤情况
		'filter': function(it, item) {
			var arr = [];
			for (var i=0; i<this.length; i++) {
				if (typeof(item)=='undefined') {
					arr.push(this[i][it]);
				} else if (this[i][it] == item) {
					arr.push(this[i]);
				}
			}
			return arr;
		},
		'sortby': function(it, dt, od) {
			// it: item name  dt: int, char  od: asc, desc
			var compareValues = function(v1, v2, dt, od) {
				if (dt == 'int') {
					v1 = parseInt(v1);
					v2 = parseInt(v2);
				} else if (dt == 'float') {
					v1 = parseFloat(v1);
					v2 = parseFloat(v2);
				}
				var ret = 0;
				if (v1 < v2) ret = 1;
				if (v1 > v2) ret = -1;
				if (od == 'desc') {
					ret = 0 - ret;				
				}
				return ret;
			};
			var newdata = new Array();
			for (var i=0; i<this.length; i++) {
				newdata[newdata.length] = this[i];
			}
			for (var i=0; i<newdata.length; i++) {
				var minIdx = i;
				var minData = (it != '') ? newdata[i][it] : newdata[i];
				for (var j=i+1; j<newdata.length; j++) {
					var tmpData = (it != '') ? newdata[j][it] : newdata[j];
					var cmp = compareValues(minData, tmpData, dt, od);
					if (cmp<0) {
						minIdx = j;
						minData = tmpData;
					}
				}
				if (minIdx > i) {
					var _child = newdata[minIdx];
					newdata[minIdx] = newdata[i];
					newdata[i] = _child;
				}
			}
			return newdata;
		}
	});
	// Date原型方法扩展
	$.extend(Date.prototype, {
		'parse': function(time) {			
			if (typeof(time)=='string') {				
				if (time.indexOf('GMT')>0 || time.indexOf('gmt')>0 || !isNaN(Date.parse(time))) {
					return this.parseGMT(time);
				} else if (time.indexOf('UTC')>0 || time.indexOf('utc')>0 || time.indexOf(',')>0) {
					return this.parseUTC(time);
				} else {
					return this.parseCommon(time);
				}
			}
			return new Date();
		},
		'parseGMT': function(time) {
			this.setTime(Date.parse(time));
			return this;			
		},
		'parseUTC': function(time) {
			return (new Date(time));			
		},
		'parseCommon': function(time) {
			var d = time.split(/ |T/), d1 = d.length > 1 ? d[1].split(/[^\d]/) : [0, 0, 0], d0 = d[0].split(/[^\d]/);
			return new Date(d0[0]-0, d0[1]-1, d0[2]-0, d1[0]-0, d1[1]-0, d1[2]-0);
		},
		'dateAdd': function(type, val) {
			var _y = this.getFullYear();
			var _m = this.getMonth();
			var _d = this.getDate();
			var _h = this.getHours();
			var _n = this.getMinutes();
			var _s = this.getSeconds();
			switch(type) {
				case 'y':
					this.setFullYear(_y + val);
					break;
				case 'm':
					this.setMonth(_m + val);
					break;
				case 'd':
					this.setDate(_d + val);
					break;
				case 'h':
					this.setHours(_h + val);
					break;
				case 'n':
					this.setMinutes(_n + val);
					break;
				case 's':
					this.setSeconds(_s + val);
					break;
			}
			return this;
		},
		'dateDiff': function(type, date2) {
			var diff = date2 - this;
			switch(type) {
				case 'w':
					return diff / 1000 / 3600 / 24 / 7;
				case 'd':
					return diff / 1000 / 3600 / 24;
				case 'h':
					return diff / 1000 / 3600;
				case 'n':
					return diff / 1000 / 60;
				case 's':
					return diff / 1000;
			}
		},
		'format': function(format) {
			if (isNaN(this)) return '';
			var o = {
				'm+': this.getMonth()+1,
				'd+': this.getDate(),
				'h+': this.getHours(),
				'n+': this.getMinutes(),
				's+': this.getSeconds(),
				'S':  this.getMilliseconds(),
				'W':  ["日", "一", "二", "三", "四", "五", "六"][this.getDay()],
				'q+': Math.floor((this.getMonth()+3)/3)
			}
			if (format.indexOf('am/pm')>=0) {
				format = format.replace('am/pm', (o['h+']>=12)?'下午':'上午');
				if (o['h+'] >= 12) o['h+'] -= 12;
			}
			if(/(y+)/.test(format)) {
				format = format.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
			}
			for (var k in o) {
				if (new RegExp("("+ k +")").test(format)) {
					format = format.replace(RegExp.$1, RegExp.$1.length==1 ? o[k] : ("00"+ o[k]).substr((""+ o[k]).length));
				}
			}
			return format;
		}
	});
})(jQuery);