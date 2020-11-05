// --------------------------------
// Toolkit.lang
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.lang.language = 'zh-cn';
	// ----------------------------
	Toolkit.lang.set('base-date', {
		'weekday': ["日", "一", "二", "三", "四", "五", "六"],
		'am': '上午',
		'pm': '下午'
	});
	// ----------------------------
	Toolkit.lang.set('util-dialog', {
		'close':		'关闭',
		'closeTips':	'关闭浮层',
		'loading':		'正在加载中……',
		'loadingError':	'加载失败',
		'confirm':		'确认',
		'cancel':		'取消',
		'alert':		'提醒'
	});
	Toolkit.lang.set('ui-calendar', {
		'year':			'年',
		'month':		'月', 
		'weekday':		'日,一,二,三,四,五,六',
		'lastmonth':	'上两月', 
		'nextmonth':	'下两月', 
		'lastyear':		'上一年', 
		'nextyear':		'下一年'
	});
	// ----------------------------
})(jQuery);