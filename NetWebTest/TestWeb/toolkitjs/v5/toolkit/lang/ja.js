// --------------------------------
// Toolkit.lang
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.lang.language = 'ja';
	// ----------------------------
	Toolkit.lang.set('base-date', {
		'weekday': ["日", "月", "火", "水", "木", "金", "土"],
		'am': 'モーニング',
		'pm': '午後'
	});
	// ----------------------------
	Toolkit.lang.set('util-dialog', {
		'close':		'閉じる',
		'closeTips':	'ダイアログを閉じます',
		'loading':		'Loading...',
		'loadingError':	'Load fail',
		'confirm':		'確認',
		'cancel':		'キャンセル',
		'alert':		'警告'
	});
	Toolkit.lang.set('ui-calendar', {
		'year':			'年',
		'month':		'月', 
		'weekday':		'日,月,火,水,木,金,土',
		'lastmonth':	'前の2ヶ月', 
		'nextmonth':	'次の2ヶ月', 
		'lastyear':		'前の年', 
		'nextyear':		'次の年'
	});
	// ----------------------------
	Toolkit.load.extend('xheditor', {
		js: 'xheditor/xheditor-1.1.7-en.min.js'
	});
	// ----------------------------
})(jQuery);