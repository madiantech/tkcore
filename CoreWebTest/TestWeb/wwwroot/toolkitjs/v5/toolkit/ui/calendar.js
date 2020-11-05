// --------------------------------
// Toolkit.ui.calendar
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.base');
	Toolkit.namespace('Toolkit.ui');
	// ----------------------------
	if (Toolkit.ui.calendar) return;
	// ----------------------------
	Toolkit.ui.calendar = function(elm, params) {
		elm.each(function() {
			var element = $(this);
			var param = params || element.attrJSON();;
			new Toolkit.base.calendar(element, param);
		});
	};
	Toolkit.base.calendar = function(elm, param) {
		var m_this = this;
		this.data = {
			character: { 
				a:"\u5e74", b:"\u6708", 
				weekday:"\u65e5\u4e00\u4e8c\u4e09\u56db\u4e94\u516d", 
				lastmonth:['\u4e0a\u4e00\u6708','\u4e0a\u4e24\u6708'], 
				nextmonth:['\u4e0b\u4e00\u6708','\u4e0b\u4e24\u6708'], 
				lastyear:'\u4e0a\u4e00\u5e74', 
				nextyear:'\u4e0b\u4e00\u5e74'
			},
			id: 'util-calendar',
			today: new Date(),
			viewMonth: param.viewMonth || 2
		};
		this.showCalendar = function() {
			this.data.bdate = (this.data.element.attr('bdate')) ? this.data.element.attr('bdate').parseDate() : null;
			this.data.edate = (this.data.element.attr('edate')) ? this.data.element.attr('edate').parseDate() : null;
			this.data.cdate = (Toolkit.string.isDate(this.data.element.val())) ? this.data.element.val().parseDate() : this.data.today;
			this.displayDate(this.data.cdate.getFullYear(), this.data.cdate.getMonth());
		};
		this.hideCalendar = function() {
			setTimeout(function() {
				if (m_this.container.attr('status')=='hide') {
					m_this.container.fadeOut();
				}
			}, 200);
		};
		this.displayDate = function(y, m) {
			this.container.find('.date-list').remove();
			this.container.find('.date-action').remove();
			this.data.year = new Date(y, m, 1).getFullYear();
			this.data.month = new Date(y, m, 1).getMonth();
			for (var j=0; j<=this.data.viewMonth-1; j++) {
				var f = new Date(y, m+j, 1);
				var w = f.getDay();
				var c = f.getDate();
				var m = f.getMonth();
				var y = f.getFullYear();
				var a = [];
				this.container.append('<div class="date-list datelist'+j+'"><label class="mod-header"></label><dl></dl></div>');
				var ya = this.data.character.weekday.split("");
				var yb =[];
				for (i=0; i<=ya.length-1; i++) {
					yb.push('<dt class="week'+i+'">'+ ya[i] +'</dt>');
				}
				this.container.find('.datelist'+j+' dl').append(yb.join(''));
				this.container.find('.datelist'+j+' label').html(y+this.data.character.a+(m+1)+this.data.character.b);
				for (var i=1; i<=w; i++) {
					a.push('<span class="foretime"></span>');
				}
				while (f.getMonth() == m) {
					var s_f = f.format('yyyy-mm-dd');
					if ((this.data.bdate!=null && s_f<this.data.bdate.format('yyyy-mm-dd')) || (this.data.edate!=null && s_f>this.data.edate.format('yyyy-mm-dd'))) {
						a.push('<span class="foretime">' + c + '</span>');
					} else if (s_f == this.data.cdate.format('yyyy-mm-dd')) {
						a.push('<a href="javascript:void(0);" class="current" date="'+ f.format(this.data.format) +'">' + c + '</a>');
					} else if (s_f == this.data.today.format('yyyy-mm-dd')) {
						a.push('<a href="javascript:void(0);" class="today" date="'+ f.format(this.data.format) +'">' + c + '</a>');
					} else {
						a.push('<a href="javascript:void(0);" date="'+ f.format(this.data.format) +'">' + c + '</a>');
					}
					f.setDate(++c);
				}
				while (c + w <= 42) {
					a.push('<span class="foretime"></span>');
					c++;
				}
				this.container.find('.datelist'+j+' dl').append('<dd>' + a.join('') + '</dd>');
			}
			this.container.append('<div class="date-action"><a class="nextyear ico-next" title="'+this.data.character.nextyear+'" step="12"></a><a class="nextmonth ico-next" title="'+this.data.character.nextmonth[this.data.viewMonth-1]+'" step="'+this.data.viewMonth+'"></a><a class="lastyear ico-prev" title="'+this.data.character.lastyear+'" step="-12"></a><a class="lastmonth ico-prev" title="'+this.data.character.lastmonth[this.data.viewMonth-1]+'" step="-'+this.data.viewMonth+'"></a></div>');
			this.container.find('.date-action a').click(function(){ m_this.displayDate(m_this.data.year, m_this.data.month+parseInt($(this).attr('step'))); }).show();
			this.container.find('dd a').click(function() {
				var date = $(this).attr('date');
				m_this.data.element.val(date);
				m_this.container.hide();
				m_this.container.attr('status','hide');
				if (m_this.data.callback) m_this.data.callback(date);
				if (m_this.data.element.attr('dataEndLimit')) {
					$('#'+m_this.data.element.attr('dataEndLimit')).attr('bdate', date);
				}
				if (m_this.data.element.attr('dataStartLimit')) {
					$('#'+m_this.data.element.attr('dataStartLimit')).attr('edate', date);
				}
				return false;
			});
			this.container.show().css({top:0,left:0});
			this.container.attr('status','show');
			//
			var o = this.data.element.offset();
			var bw = $(document).width();
			var bh = $(document).height();
			var ew = this.data.element.outerWidth() || (this.data.element.width()+2);
			var eh = this.data.element.outerHeight() || (this.data.element.height()+2);
			var left = (bw - o.left >= this.container.width()) ? o.left : (ew+o.left-this.container.width()-2);
			var top = (bh - o.top - eh >= this.container.height()) ? (o.top+eh-1) : (o.top-this.container.height()-2); 
			this.container.css({top:top, left:left, right:'auto'});
		};
		this.init = function(elm, param) {
			this.data.element = elm;
			this.data.element.addClass('ui-calendar-source');
			this.data.format = param.format || 'yyyy-mm-dd';
			this.data.callback = param.callback;
			if (param.bdate) elm.attr('bdate', param.bdate);
			if (param.edate) elm.attr('edate', param.edate);
			elm.focus(function(){ m_this.container.attr('status','show'); m_this.showCalendar(); });
			elm.blur(function() { m_this.container.attr('status','hide'); m_this.hideCalendar(); });
			//
			this.container = $('#'+this.data.id);
			if (this.container.size() == 0) {
				$('body').append('<div id="'+this.data.id+'" class="mod-frame month-col'+this.data.viewMonth+'" style="display:none;"></div>');
				this.container = $('#'+this.data.id);
				this.container.mouseover(function(){ $(this).attr('status','show'); });
				$(document).click(function(e) {
					var evt = e || window.event || null;
					var src = evt ? (evt.srcElement || evt.target) : null;
					if (typeof(src)=='object') {
						if ($(src).tagName()=='input' && $(src).hasClass('ui-calendar-source')) return false;
						if ($(src).tagName()=='a' && $(src).parents('#'+ m_this.data.id).size()>0) return false;
					}
					m_this.container.attr('status','hide');
					m_this.hideCalendar();
				});
			}
		};
		this.init(elm, param);
	};
	// ----------------------------
	Toolkit.debug('calendar.js', '初始化成功');
})(jQuery);