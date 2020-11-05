// --------------------------------
// Toolkit.lang
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.lang.language = 'en';
	// ----------------------------
	Toolkit.lang.set('base-date', {
		'weekday': ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
		'am': 'AM',
		'pm': 'PM'
	});
	// ----------------------------
	Toolkit.lang.set('util-dialog', {
		'close':		'Close',
		'closeTips':	'close dialog',
		'loading':		'Loading...',
		'loadingError':	'Load fail',
		'confirm':		'OK',
		'cancel':		'Cancel',
		'alert':		'Message'
	});
	Toolkit.lang.set('ui-calendar', {
		'year':			'-',
		'month':		' ', 
		'weekday':		'Sun,Mon,Tue,Wed,Thu,Fri,Sat',
		'lastmonth':	'last two months', 
		'nextmonth':	'next two months', 
		'lastyear':		'last year', 
		'nextyear':		'next year'
	});
	// ----------------------------
	Toolkit.load.extend('xheditor', {
		js: 'xheditor/xheditor-1.1.7-en.min.js'
	});
	// ----------------------------
})(jQuery);