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
	Toolkit.load.add('bootbox', {
	    js: 'bootstrap/js/bootbox.min.js'
	});
	//Toolkit.load.add('util.dialog', {
	//	js: 'toolkit/util/dialog.js',
	//	css: 'toolkit/util/dialog.css'
	//});
	Toolkit.load.add('datetimepicker', {
	    js: 'bootdatepicker/bootstrap-datetimepicker.min.js',
	    css: 'bootdatepicker/bootstrap-datetimepicker.min.css'
	});
	Toolkit.load.add('kindeditor', {
	    js: 'kindeditor/kindeditor-all.js'
	});
	Toolkit.load.add('swfobject', { 
		js: 'uploadify/swfobject.js' 
	});
	Toolkit.load.add('uploadify', {
		js: 'uploadify/jquery.uploadify.v2.1.4.min.js',
		swf: 'uploadify/uploadify.swf',
		uploadBtn: 'uploadify/upload.png',
		cancelBtn: 'uploadify/cancel.png',
		requires: 'swfobject'
	});
	Toolkit.load.add('treeview', {
		js: 'treeview/jquery.treeview.js',
		css: 'treeview/jquery.treeview.css'
	});
	Toolkit.load.add('jqueryui', {
		js: 'lib/jquery-ui-1.8.16.custom.min.js'
	});
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