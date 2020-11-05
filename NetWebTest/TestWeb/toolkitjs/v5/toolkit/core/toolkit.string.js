// --------------------------------
// Toolkit.string
// 用于Toolkit.valid中的数据类型校验
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.string');
	// ----------------------------
	Toolkit.string.isUrl = function(str) {
		return (new RegExp(/(http[s]?|ftp):\/\/[^\/\.]+?\..+\w$/i).test(str.trim()));
	};
	Toolkit.string.isDate = function(str) {
		var result = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
		if (result==null) return false;
		var d = new Date(result[1], result[3]-1, result[4]);
		return (d.getFullYear()==result[1] && d.getMonth()+1==result[3] && d.getDate()==result[4]);
	};
	Toolkit.string.isTime = function(str) {
		var result = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/); 
		if (result==null) return false;
		if (result[1]>24 || result[3]>60 || result[4]>60) return false;
		return true;
	};
	Toolkit.string.isDateTime = function(str) {
		var result = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/);
		if (result==null) return false;
		var d = new Date(result[1], result[3]-1, result[4], result[5], result[6], result[7]);
		return (d.getFullYear()==result[1] && (d.getMonth()+1)==result[3] && d.getDate()==result[4] && d.getHours()==result[5] && d.getMinutes()==result[6] && d.getSeconds()==result[7]);
	};
	// 整数
	Toolkit.string.isInteger = function(str) {
		return (new RegExp(/^(-|\+)?\d+$/).test(str.trim()));
	};
	// 正整数
	Toolkit.string.isPositiveInteger = function(str) {
		return (new RegExp(/^\d+$/).test(str.trim())) && parseInt(str)>0;
	};
	// 负整数
	Toolkit.string.isNegativeInteger = function(str) {
		return (new RegExp(/^-\d+$/).test(str.trim()));
	};
	Toolkit.string.isNumber = function(str) { 
		return !isNaN(str);
	};
	Toolkit.string.isEmail = function(str) {
		return (new RegExp(/^([_a-zA-Z\d\-\.])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/).test(str.trim()));
	};
	Toolkit.string.isMobile = function(str) {
		return (new RegExp(/^(13|14|15|18)\d{9}$/).test(str.trim()));
	};
	Toolkit.string.isPhone = function(str) {
		return (new RegExp(/^(([0\+]\d{2,3}-)?(0\d{2,3})-)?(\d{7,8})(-(\d{3,}))?$/).test(str.trim()));
	};
	Toolkit.string.isAreacode = function(str) {
		return (new RegExp(/^0\d{2,3}$/).test(str.trim()));
	};
	Toolkit.string.isPostcode = function(str) {
		return (new RegExp(/^\d{6}$/).test(str.trim()));
	};
	Toolkit.string.isLetters = function(str) {
		return  (new RegExp(/^[A-Za-z]+$/).test(str.trim()));
	};
	Toolkit.string.isDigits = function(str) {
		return (new RegExp(/^[1-9][0-9]+$/).test(str.trim()));
	};
	Toolkit.string.isAlphanumeric = function(str) {
		return  (new RegExp(/^[a-zA-Z0-9]+$/).test(str.trim()));
	};
	Toolkit.string.isValidString = function(str) {
		return  (new RegExp(/^[a-zA-Z0-9\s.\-_]+$/).test(str.trim()));
	};
	Toolkit.string.isLowerCase = function(str) {
		return  (new RegExp(/^[a-z]+$/).test(str.trim()));
	};
	Toolkit.string.isUpperCase = function(str) {
		return  (new RegExp(/^[A-Z]+$/).test(str.trim()));
	};
	Toolkit.string.isChinese = function(str) {
		return (new RegExp(/^[\u4e00-\u9fa5]+$/).test(str.trim()));
	};
	Toolkit.string.isIDCard = function(str) {
		//这里没有验证有效性，只验证了格式
		var r15 = new RegExp(/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/);
		var r18 = new RegExp(/^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X|x)$/);
		return (r15.test(str.trim()) || r18.test(str.trim()));
	};
	// ----------------------------
})(jQuery);