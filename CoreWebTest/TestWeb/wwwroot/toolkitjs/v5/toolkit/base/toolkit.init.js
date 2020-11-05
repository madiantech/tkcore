// --------------------------------
// Toolkit init
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.load.add('easysearch', {
	    js: 'toolkit/ui/easysearch.js',
	    css: 'toolkit/ui/easysearch.css'
	});
	Toolkit.load.add('util.dialog', {
		js: 'toolkit/util/toolkit.dialog.js'
	});
	Toolkit.load.add('switcher', {
	    js: 'bootstrap/js/bootstrap-switch.min.js',
	    css: 'bootstrap/css/bootstrap-switch.min.css'
	});
	Toolkit.load.add('datetimepicker', {
	    js: 'bootdatepicker/bootstrap-datetimepicker.min.js',
	    css: 'bootdatepicker/bootstrap-datetimepicker.min.css'
	});
	Toolkit.load.add('timepicker', {
	    js: 'boottimepicker/bootstrap-timepicker.min.js',
	    css: 'boottimepicker/bootstrap-timepicker.min.css'
	});
	Toolkit.load.add('kindeditor', {
	    js: 'kindeditor/kindeditor-all.js'
	});
	Toolkit.load.add('treeview', {
	    js: 'jstree/jstree.min.js',
        css: 'jstree/themes/default/style.min.css'
	});
	//Toolkit.load.add('swfobject', { 
	//	js: 'uploadify/swfobject.js' 
	//});
	//Toolkit.load.add('uploadify', {
	//	js: 'uploadify/jquery.uploadify.v2.1.4.min.js',
	//	swf: 'uploadify/uploadify.swf',
	//	uploadBtn: 'uploadify/upload.png',
	//	cancelBtn: 'uploadify/cancel.png',
	//	requires: 'swfobject'
	//});
	Toolkit.load.add('firebug', {
		js: document.location.protocol +'//getfirebug.com/firebug-lite.js'
	});
	// ----------------------------
	Toolkit.load('util.dialog');
	// ----------------------------
	$(document).ready(function() {
		// ------------------------
		Toolkit.page.init();
		// ------------------------
	});
})(jQuery);