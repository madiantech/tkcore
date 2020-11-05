// --------------------------------
// Toolkit.lang
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.lang.language = 'zh-tw';
	// ----------------------------
	Toolkit.lang.set('base-date', {
		'weekday': ["日", "一", "二", "三", "四", "五", "六"],
		'am': '上午',
		'pm': '下午'
	});
	// ----------------------------
	Toolkit.lang.set('util-dialog', {
		'close':		'關閉',
		'closeTips':	'關閉浮層',
		'loading':		'正在加載中……',
		'loadingError':	'加載失败',
		'confirm':		'確認',
		'cancel':		'取消',
		'alert':		'提醒'
	});
	Toolkit.lang.set('ui-calendar', {
		'year':			'年',
		'month':		'月', 
		'weekday':		'日,一,二,三,四,五,六',
		'lastmonth':	'上兩月', 
		'nextmonth':	'下兩月', 
		'lastyear':		'上一年', 
		'nextyear':		'下一年'
	});
	// ----------------------------
	Toolkit.load.extend('xheditor', {
		js: 'xheditor/xheditor-1.1.7-zh-tw.min.js'
	});
	// ----------------------------
})(jQuery);