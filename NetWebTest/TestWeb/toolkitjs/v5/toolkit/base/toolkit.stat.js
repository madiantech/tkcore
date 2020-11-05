// --------------------------------
// Toolkit.stat
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------	
	Toolkit.namespace('Toolkit.stat');
	// ----------------------------
	Toolkit.stat.funcStat = {};
	Toolkit.stat.addFuncStat = function(name) {
		if (!this.funcStat[name] || this.funcStat[name]<100000000) {
			this.funcStat[name] = (new Date()).getTime();
		} else {
			this.funcStat[name] = (new Date()).getTime() - this.funcStat[name];
		}
	};
	// ----------------------------
})(jQuery);