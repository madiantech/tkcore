// --------------------------------
// Toolkit.data
// --------------------------------
(function($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.data');
    // ----------------------------
    // 返回element类型
    Toolkit.data.getElementType = function(elm) {
        var tag = elm.tagName();
        if (tag == 'input') {
            return elm.attr('type') || 'text';
        }
        return tag;
    };
    // 返回element的值
    Toolkit.data.getElementValue = function(elm) {
        if (elm.size() == 0) return '';
        var type = Toolkit.data.getElementType(elm);
        switch (type) {
            case 'checkbox':
                var control = elm.closest('.tk-control').attr('dataControl') || '';
                return (control == 'CheckBoxList') ? Toolkit.data.getCheckedValue(elm, true).join(',') : Toolkit.data.getCheckedValue(elm).join(',');
            case 'radio':
                return Toolkit.data.getCheckedValue(elm).join(',');
            case 'hidden':
                var control = elm.closest('.tk-control').attr('dataControl') || '';
                return (control == 'MultiEasySearch') ? Toolkit.data.getElementsValue(elm, { quote: true, noEmpty: true }).join(',') : elm.val();
            default:
                return elm.val();
        }
    };
    // 获取多项内容的值
    Toolkit.data.getElementsValue = function(elm, option) {
        var option = option || {};
        var arr = [];
        elm.each(function() {
            if ($(this).val() != '' || !option.noEmpty) {
                arr.push((option.quote) ? ('\"' + $(this).val() + '\"') : $(this).val());
            }
        });
        return arr;
    };
    // 获取多项内容的文本
    Toolkit.data.getElementsText = function(elm, noEmpty) {
        var arr = [];
        elm.each(function() {
            if ($(this).text() != '' || !noEmpty) arr.push($(this).text());
        });
        return arr;
    };
    // 获取选择数量
    Toolkit.data.getCheckedSize = function(elm) {
        var checkedSize = 0;
        elm.each(function() { if (this.checked) checkedSize++; });
        return checkedSize;
    };
    // 获取选择的值
    Toolkit.data.getCheckedValue = function(elm, quote) {
        var arr = [];
        elm.each(function() {
            if (this.checked) arr.push((quote) ? ('\"' + this.value + '\"') : this.value);
        });
        return arr;
    };
    // ----------------------------
    Toolkit.data.getTextInputsIndex = function(elm, type) {
        var container = (type == 'table') ? $(elm).parents('tr') : $(elm).parents('.tk-datatable,.tk-datasearch');
        var inputs = container.find('input[type=text],textarea');
        var size = inputs.size();
        var index = 0;
        inputs.each(function(i) {
            if (elm == this) {
                index = i;
            }
        });
        var lastrow, nextrow;
        if (type == 'table') {
            lastrow = container.prev().find('input[type=text],textarea');
            nextrow = container.next().find('input[type=text],textarea');
        }
        var left = (index > 0) ? inputs.eq(index - 1) : inputs.eq(size + 1);
        var up = (type == 'table') ? lastrow.eq(index) : left;
        var right = inputs.eq(index + 1);
        var down = (type == 'table') ? nextrow.eq(index) : right;
        return {
            size: size,
            index: index,
            left: left,
            up: up,
            right: right,
            down: down
        };
    };
    // 获取EasySearch的Ref Field Value
    Toolkit.data.getRefElement = function(elm, field) {
        var container = (elm.parent().tagName() == 'dd') ? elm.closest('.tk-datatable') : elm.closest('tr');
        var fieldElm = container.find('[name=hd' + field + ']');
        if (fieldElm.size() == 0) fieldElm = container.find('[name=' + field + ']');
        if (fieldElm.size() == 0) fieldElm = container.find('#' + field);
        return fieldElm;
    };
    // ----------------------------
    // 全选
    Toolkit.data.CheckAll = function(elm, chkSource) {
        var checked = true;
        if (typeof (chkSource) == 'object') {
            checked = chkSource.checked;
        } else if (typeof (chkSource) == 'boolean') {
            checked = chkSource;
        }
        elm.each(function() {
            this.checked = checked;
            Toolkit.data.CheckDataRow($(this));
        });
    };
    // 反选
    Toolkit.data.CheckReverse = function(elm) {
        elm.each(function() {
            this.checked = !this.checked;
            Toolkit.data.CheckDataRow($(this));
        });
    };
    // 选中或取消选中当前行
    Toolkit.data.CheckDataRow = function(elm) {
        if (!elm.hasClass('e-checkdatarow')) return;
        if (elm.attr('checked')) {
            elm.parents('tr').addClass('selected');
        } else {
            elm.parents('tr').removeClass('selected');
        }
    };
    // ----------------------------
    // 输入提醒（默认显示）
    Toolkit.data.InputHint = function(elm) {
        elm.each(function() {
            var element = $(this);
            $(this.form).bind('ClearHintBeforeSubmit', function(e) {
                if ($.trim(element.val()) == '' || $.trim(element.val()) == element.attr('dataHint')) {
                    if (element.attr('dataRequired') == 'true') {
                        element.focus();
                        if (e.preventDefault) e.preventDefault();
                        return false;
                    } else {
                        element.val('');
                        return true;
                    }
                }
            });
        });
        elm.bind('focus', function() {
            $(this).removeClass('hint');
            if ($.trim($(this).val()) == $(this).attr('dataHint')) $(this).val('');
        });
        elm.bind('blur', function() {
            if ($.trim($(this).val()) == '' || $.trim($(this).val()) == $(this).attr('dataHint')) {
                $(this).addClass('hint');
                $(this).val($(this).attr('dataHint'));
            }
        });
        elm.blur();
    };
    // ----------------------------
    // 输入限制： 只能输入整数或小数
    Toolkit.data.InputLimit = function(elm) {
        elm.css('ime-mode', 'disabled');
        elm.bind('keypress', function(e) {
            try {
                if (e.shiftKey) return false;
                var code = e.which || e.keyCode || 0;
                var limitType = $(this).attr('dataLimitType') || '';
                switch (limitType) {
                    case 'float': //浮点数/小数
                        return code == 8 || (e.which == 0 && e.keyCode == 37) || (e.which == 0 && e.keyCode == 39) || (code >= 48 && code <= 57) || code == 46;
                    case 'integer': //整数
                        return code == 8 || (e.which == 0 && e.keyCode == 37) || (e.which == 0 && e.keyCode == 39) || (code >= 48 && code <= 57) || (e.which == 0 && e.keyCode == 46);
                }
                // 8:backspace / 37:left / 39:right / 46:del (FF: del为which=0,keyCode=46)
            } catch (err) { }
        });
        elm.bind('paste', function() { return !clipboardData.getData('text').match(/\D/); });
        elm.bind('dragenter', function() { return false; });
    };
    // ----------------------------
    // 输入校验：
    $.fn.showInputValidStatus = function(error) {
        this.data('error', false).data('valid', true);
        this.closest('.tk-control').parent().removeClass('error').attr('title', '');
        if (error) {
            this.data('error', true);
            this.closest('.tk-control').parent().addClass('error').attr('title', error);
            // 为没有valid的元素增加一个change事件，用于后台校验返回后的内容修改后清除error
            if (!this.data('validBinded')) {
                this.change(function() {
                    $(this).showInputValidStatus();
                });
                this.data('validBinded', true);
            }
        }
    };
    Toolkit.data.InputValid = function(elm) {
        elm.each(function() {
            var element = $(this);
            var type = Toolkit.data.getElementType(element);
            switch (type) {
                case 'radio':
                case 'checkbox':
                    break;
                case 'text':
                case 'password':
                case 'textarea':
                    element.bind('valid', function() {
                        var element = $(this);
                        var val = element.val().trim();
                        var title = element.attr('dataTitle') || '';
                        var hint = element.attr('dataHint') || '';
                        var required = element.attr('dataRequired') || '';
                        var validtype = element.attr('dataValidType') || '';
                        var validreg = element.attr('dataValidRegex') || '';
                        var empty_error = '请输入' + title;
                        var valid_error = '请输入正确的' + title;
                        //
                        if (val == hint || val == '') {
                            if (required == 'true') {
                                element.showInputValidStatus(empty_error);
                                return false;
                            }
                        } else {
                            if (validtype != 'regex') {
                                validtype = ('is-' + validtype).toCamelCase();
                                if (Toolkit.string[validtype] && !Toolkit.string[validtype](val)) {
                                    element.showInputValidStatus(valid_error);
                                    return false;
                                }
                            } else if (validreg != '') {
                                if (!(new RegExp(validreg)).test(val)) {
                                    element.showInputValidStatus(valid_error);
                                    return false;
                                }
                            }
                        }
                        element.showInputValidStatus(false);
                    });
                    element.data('validBinded', true);
                    element.blur(function() {
                        $(this).trigger('valid');
                    });
                    break;
                case 'select':
                    element.bind('valid', function() {
                        var element = $(this);
                        var val = element.val().trim();
                        var title = element.attr('dataTitle') || '';
                        var filter = element.attr('dataFilter') || '';
                        var required = element.attr('dataRequired') || '';
                        //
                        if (required == 'true' && (val == filter || val == '')) {
                            element.showInputValidStatus('请选择' + title);
                            return false;
                        }
                        element.showInputValidStatus(false);
                    });
                    element.data('validBinded', true);
                    element.change(function() {
                        $(this).trigger('valid');
                    });
                    break;
            }
        });
    };
    Toolkit.data.CheckInputValid = function(elm) {
        if (elm.data('valid')) return;
        var type = Toolkit.data.getElementType(elm);
        switch (type) {
            case 'radio':
            case 'checkbox':
                var title = elm.attr('dataTitle') || '';
                var required = elm.attr('dataRequired') || '';
                if (required == 'true') {
                    if (Toolkit.data.getCheckedSize(elm) == 0) {
                        elm.showInputValidStatus('请选择' + title);
                    } else {
                        elm.showInputValidStatus(false);
                    }
                }
                break;
            case 'text':
            case 'password':
            case 'textarea':
            case 'select':
                elm.trigger('valid');
                break;
        }
    };
    // 更新表单元素的验证状态（根据post返回的错误信息）
    Toolkit.data.UpdateInputsValid = function(elm, errors) {
        var dataSetId = elm.attr('dataSet');
        for (var i = 0; i < errors.length; i++) {
            var table = errors[i]['TABLE_NAME'];
            var row = errors[i]['ERROR_POS'];
            var field = errors[i]['FIELD_NAME'];
            var error = errors[i]['ERROR_VALUE'];
            Toolkit.dataset[dataSetId][table].elements[row][field].showInputValidStatus(error);
            if (i == 0) {
                Toolkit.dataset[dataSetId][table].elements[row][field].focus();
            }
        }
		elm.trigger('AfterServerValidError');
    };
    // ----------------------------
    Toolkit.data.LoadDataSet = function(frm) {
        var dataTables = frm.attr('dataTables') || '';
        if (dataTables == '') {
            // 没有dataset的表单
            frm.addClass('ui-form-nodataset');
        } else {
            // 初始化form dataset
            Toolkit.dataset.initForm(frm);
        }
    };
    Toolkit.data.InsertDataSetRow = function(elm) {
        var dataSetId = elm.parents('form').attr('dataSet') || '';
        var table = elm.parents('table').attr('id') || '';
        if (dataSetId == '' || table == '') return;
        var fields = Toolkit.dataset[dataSetId][table].fields;
        Toolkit.dataset.addElement(dataSetId, elm, table, fields);
    };
    Toolkit.data.DeleteDataSetRow = function(elm, rowlist) {
        var dataSetId = elm.parents('form').attr('dataSet') || '';
        var table = elm.attr('id') || '';
        if (dataSetId == '' || table == '') return;
        for (var i = rowlist.length - 1; i >= 0; i--) {
            Toolkit.dataset.removeElement(dataSetId, table, rowlist[i] - 1);
        }
    };
    Toolkit.data.DeleteAllDataSetRow = function(elm) {
        var dataSetId = elm.parents('form').attr('dataSet') || '';
        var table = elm.attr('id') || '';
        if (dataSetId == '' || table == '') return;
        Toolkit.dataset.emptyElements(dataSetId, table);
    };
    Toolkit.data.UpdateDataSetRowElement = function(ctrlElement, field) {
        var dataSetId = ctrlElement.parents('form').attr('dataSet');
        var table = ctrlElement.parents('.tk-datatable,table').attr('id') || '';
        var row = (ctrlElement.tagName() == 'dd') ? 0 : (ctrlElement.parent().find('input.row-index').val() - 1);
        try {
            Toolkit.dataset[dataSetId][table].elements[row][field] = ctrlElement.find('[name=' + field + ']');
        } catch (e) { }
    };
    // ----------------------------
    // 搜索表单提交处理
    Toolkit.data.SearchSubmit = function(frm) {
        if (frm.hasClass('ui-form-nodataset')) {
            return Toolkit.data.CommonSubmit(frm);
        }
        //
        var ret = frm.triggerHandler('BeforeFormDataCheck');
        if (typeof (ret) == 'boolean' && !ret) return false;
        //
		frm.trigger('ClearHintBeforeSubmit');
        var guid = $('.ui-list-frame .ui-list-table').attr('dataGuid') || '';
        var url = frm.attr('action');
        var dataSetId = frm.attr('dataSet');
        var data = Toolkit.dataset.getData(dataSetId);
        if (data) {
            data.OtherInfo = [{ DataSet: guid, Save: ""}];
            //
            var ret = frm.triggerHandler('BeforeFormSubmit');
            if (typeof (ret) == 'boolean' && !ret) return false;
            //
            Toolkit.data.loadHTML($('.ui-list-frame'), 'post', url, data);
        }
        return false;
    };
    // 修改表单提交处理
    Toolkit.data.UpdateSubmit = function (frm, option) {
        return Toolkit.data.InternalUpdateCommit(frm, option, function (req) {
            if (req.left(2) == 'OK') {
                var retUrl = req.sliceAfter('OK');
                var ret = frm.triggerHandler('FormSubmitSuccess', retUrl);
                if (typeof (ret) == 'boolean' && !ret) return false;
                document.location.href = retUrl;
            } else {
                var data = req.parseJSON();
                if (data && data.ERROR) {
                    Toolkit.page.showError(data.ERROR.filter('ERROR_VALUE').join('<br>'), function () {
                        Toolkit.data.UpdateInputsValid(frm, data.ERROR);
                    });
                } else {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            }
        });
    };
    // 修改表单提交处理,处理后关闭对话框
    Toolkit.data.InternalUpdateCommit = function (frm, option, successCallback) {
        //if (frm.hasClass('ui-form-nodataset')) {
        //	return Toolkit.data.CommonSubmit(frm);
        //}
        //
        var ret = frm.triggerHandler('BeforeFormDataCheck');
        if (typeof (ret) == 'boolean' && !ret) return false;
        //
        Toolkit.page.showLoading('正在检查表单数据……');
        // 
        var url = '';
        if (typeof (option) == 'string') {
            url = option;
        } else if ($.isFunction(option)) {
            url = option(frm);
        } else if (typeof (option) == 'object' && option != null) {
            if (!option.url && !option.getUrl) {
                var _url = frm.attr('action');
                var _path = _url.getPathName();
                var _param = _url.getQueryJson();
                $.extend(_param, option);
                url = _path + '?' + $.param(_param);
            } else {
                var _url = (option.getUrl) ? option.getUrl(frm) : option.url;
                if (option.params) {
                    var _path = _url.getPathName();
                    var _param = _url.getQueryJson();
                    $.extend(_param, option.params);
                    _url = _path + '?' + $.param(_param);
                }
                url = _url;
            }
        } else {
            url = frm.attr('action');
        }
        //
        frm.trigger('ClearHintBeforeSubmit');
        var data = {};
        if (!frm.hasClass('ui-form-nodataset')) {
            var dataSetId = frm.attr('dataSet');
            data = Toolkit.dataset.getData(dataSetId);
        }
        if (data) {
            //
            var ret = frm.triggerHandler('BeforeFormSubmit');
            if (typeof (ret) == 'boolean' && !ret) return false;
            //
            Toolkit.page.showLoading('正在提交表单数据……');
            $.ajax({
                type: 'post', dataType: 'text', url: url, data: Toolkit.json.stringify(data), success: successCallback,
                error: function () {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            });
        }
        return false;
    };

    // 修改表单提交处理,处理后关闭对话框
    Toolkit.data.UpdateSubmitDialog = function (frm, option) {
        return Toolkit.data.InternalUpdateCommit(frm, option, function (req) {
            if (req.left(2) === 'OK') {
                var idxBegin = req.indexOf(";Alert;");
                var idxEnd = req.lastIndexOf(";Alert;");
                var retUrl;
                var message;
                if (idxBegin < idxEnd && idxBegin > 0) {
                    message = req.substring(idxBegin + 7, idxEnd);
                    retUrl = req.substring(idxEnd + 7, req.length); //"../WebForm1.aspx";
                }
                else {
                    message = null;
                    retUrl = req.sliceAfter('OK');
                }
                parent.Toolkit.page.dataList.insertSuccess(message, retUrl);
                parent.Toolkit.util.dialog.close();
            } else {
                var data = req.parseJSON();
                if (data && data.ERROR) {
                    Toolkit.page.showError(data.ERROR.filter('ERROR_VALUE').join('<br>'), function () {
                        Toolkit.data.UpdateInputsValid(frm, data.ERROR);
                    });
                } else {
                    Toolkit.page.showError('数据提交失败！请稍候重试。');
                }
            }
        });
    };
    // 普通表单提交处理
    Toolkit.data.CommonSubmit = function(frm) {
        //
        var ret = frm.triggerHandler('BeforeFormSubmit');
        if (typeof (ret) == 'boolean' && !ret) return false;
        //
        frm.submit();
    };
    // ----------------------------
    // 数据列表读取，主要用于DetailList页等
    Toolkit.data.LoadDataListByCustom = function(elm, url) {
        var path = url.getPathName();
        var param = url.getQueryJson();
        Toolkit.data.loadHTML(elm, 'get', path, param);
    };
    // Tab数据读取，主要用于List页
    Toolkit.data.LoadDataListByTab = function(elm, tab) {
        Toolkit.data.LoadDataList(elm, 'tab', { 'Tab': tab, 'Page': 0 });
    };
    // 分页数据读取，用于List/DetailList页
    Toolkit.data.LoadDataListByPage = function(elm, page) {
        Toolkit.data.LoadDataList(elm, 'page', { 'Page': page });
    };
    // 排序数据读取，用于List/DetailList页
    Toolkit.data.LoadDataListByOrder = function(elm, data) {
        Toolkit.data.LoadDataList(elm, 'order', { 'Sort': data.sort, 'Order': data.order });
    };
    // 排序数据读取，用于List/DetailList页
    Toolkit.data.DataListRefresh = function(elm) {
        Toolkit.data.LoadDataList(elm, 'refresh');
    };
    Toolkit.data.LoadDataList = function(elm, type, data) {
        var data = data || {};
        var url = elm.find('.ui-list-table').attr('dataUrl') || '';
        if (url == '') {
            Toolkit.page.showError('数据加载失败！');
            return;
        }
        var path = url.getPathName();
        var param = url.getQueryJson();
        $.extend(param, data);
        delete param['_'];
        Toolkit.data.loadHTML(elm, 'get', path, param);
    };
    // 加载请求的数据列表
    Toolkit.data.loadHTML = function(element, type, url, data) {
		Toolkit.page.showLoading('正在加载内容……');
        if (type == 'post') {
            data = Toolkit.json.stringify(data);
        } else if (data) {
            data.GetData = "Page";
            data.TotalPage = $("#_TotalPage").val();
            data.TotalCount = $("#_TotalCount").val();
            data._ = (new Date()).getTime();
        }
        $.ajax({ type: type, url: url, data: data, success: function(req) {
            if ($.isFunction(element)) {
                element();
            } else {
                element.empty();
                element.html(req);
                element.data('resize', false);
				element.trigger('HtmlLoadComplete');
				element.bindToolkitUI('PageContentExecute');
				element.find(".TitleSet").each(function () { if ($(this).text().trim() != "") $(this).attr("title", $(this).text()); });
            }
            Toolkit.page.hideLoading();
        }, error: function() {
            Toolkit.page.showError('内容加载失败！');
        }});
    };
})(jQuery);