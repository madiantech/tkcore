// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.ui');
    Toolkit.ui.PageContentExecute = function (elm) {
        if (elm.tagName() == 'body' || elm.hasClass('tk-dataPage')) {
            // iframe/ajax加载页面时处理
            elm.find('.tk-datasearch').bindToolkitUI('DataSearchExecute');
            elm.find('.tk-datatab').bindToolkitUI('DataTabExecute');
            elm.find('.tk-datadetail').bindToolkitUI('DataDetailExecute');
            elm.find('.tk-datalist').bindToolkitUI('DataListLoad').bindToolkitUI('DataListExecute');
            elm.find('.tk-dataform').bindToolkitUI('DataFormExecute');
        } else if (elm.hasClass('ui-list-sction')) {
            // 预留加载section时处理
        } else if (elm.hasClass('ui-list-frame')) {
            // 只加载数据列表时处理
            elm.bindToolkitUI('DataTableExecute');
        }
        elm.bindToolkitUI('LoadDataSection');
        elm.bindToolkitUI('DataLinksExecute');
    };
    // Insert/Update页的表单处理
    Toolkit.ui.DataFormExecute = function (elm) {
        // 折叠/展开处理
        //elm.find('.mod-header').bindToolkitUI('DataHeaderCollapse', 'form');
        elm.bindToolkitUI('DataFormDetailExecute');
        elm.bindToolkitUI('DataFormListExecute');
        //Toolkit.ui.UpdateDataFormHeight();
        //
        //elm.find('form').bindToolkitData('LoadDataSet');
        // 提交搜索
        //elm.find('.btn-submit').click(function () {
        //    var url = $(this).attr('href') || '#';
        //    if (url == "#" || url == "##" || url == "###") url = null;
        //    $(this).parents('form').bindToolkitData('UpdateSubmit', url);
        //    return false;
        //});
    };
    Toolkit.ui.DataFormDetailExecute = function (elm) {
        // 避免list-table下的隐藏模板里的tk-datatable被解析
        elm.find('.tk-datatable').each(function () {
            if ($(this).parent().tagName() == 'td') return;
            $(this).bindToolkitUI('DataDetailElementExecute');
            $(this).bindToolkitUI('DataFormElementExecute', 'detail');
        });
    };
    Toolkit.ui.DataDetailElementExecute = function (elm) {
        elm.find('[dataControl=DetailHTML]').bindToolkitUI('HtmlEditor');
    };
    Toolkit.ui.DataFormElementExecute = function (elm, type) {
        // 基本的UI处理
        //elm.find('[dataControl=Date], [dataControl=DateTime]').bindToolkitUI('DatePicker');
        elm.find('[data-control=EasySearch]').bindToolkitUI('easySearchControl');
        elm.find('[data-control=HTML]').bindToolkitUI('htmlEditor');
        elm.find('[data-control=Date]').bindToolkitUI('dateControl');
        elm.find('[data-control=DateTime]').bindToolkitUI('dateTimeControl');
        elm.find('[data-control=Time]').bindToolkitUI('timeControl');
        //elm.find('[dataControl=MultiEasySearch]').bindToolkitUI('MultiEasySearch');
        //elm.find('[dataControl=Upload]').bindToolkitUI('UploadFile');

        // 基本的Data处理
        //elm.find('input[dataHint],textarea[dataHint]').bindToolkitData('InputHint');
        //elm.find('input[dataLimitType],textarea[dataLimitType]').bindToolkitData('InputLimit');
        //elm.find('input[dataValidType],textarea[dataValidType],input[dataRequired],textarea[dataRequired],select[dataRequired]').bindToolkitData('InputValid');
        // KeyBoard相关处理
        //var type = type || '';
        //if (type == 'detail') {
        //    elm.find('.tk-control input[type=text], .tk-control input[type=password], .tk-control textarea').bindToolkitUI('KeyBoardExecute', 'dl');
        //} else if (type == 'list') {
        //    elm.find('.tk-control input[type=text], .tk-control input[type=password], .tk-control textarea').bindToolkitUI('KeyBoardExecute', 'table');
        //    // 添加折叠和展开
        //    elm.find('.tk-datatable').bindToolkitUI('AddRowFormCollapse');
        //}
    };
    // 绑定EasySearch处理
    Toolkit.ui.easySearchControl = function (elm) {
        Toolkit.load('easysearch', function () {
            elm.each(function () {
                var element = $(this);
                var inputElement = element.find('input[type=text]');
                //var fieldStr = element.attr('dataRefField') || '';
                var refFields = null;
                //if (fieldStr != '') {
                //    var fields = fieldStr.split(';');
                //    refFields = {};
                //    for (var i = 0; i < fields.length; i++) {
                //        if (fields[i].indexOf(',') < 1) continue;
                //        var field_name = fields[i].split(',')[0];
                //        var element_name = fields[i].split(',')[1];
                //        var refElement = Toolkit.data.getRefElement(element, element_name);
                //        refElement.bind('change', function () {
                //            inputElement.bindToolkitUI('EasySearchEmpty');
                //        });
                //        refFields[field_name] = refElement;
                //    }
                //}
                inputElement.bindToolkitUI('suggest', {
                    source: element.attr('data-url'),
                    refFields: refFields,
                    onBeforeSuggest: function () {
                        inputElement.trigger('BeforeEasySearch');
                    },
                    onSelect: function (input, id, text) {
                        input.bindToolkitUI('EasySearchChecked', { id: id, text: text });
                    }
                });
                inputElement.bindToolkitUI('EasySearchChecked');
                element.find('span.ico').click(function () {
                    inputElement.trigger('BeforeEasySearch');
                    inputElement.bindToolkitUI('EasySearchPopWin', {
                        title: inputElement.attr('data-title') || '',
                        source: element.attr('data-page') || '',
                        exId: inputElement.siblings('[name=hd' + inputElement.attr('name') + ']').val() || '',
                        refFields: refFields
                    });
                });
            });
        });
    };
    Toolkit.ui.EasySearchEmpty = function (input) {
        var hdinput = input.siblings('[name=hd' + input.attr('name') + ']');
        input.siblings('.checked').remove();
        hdinput.val('').trigger('change');
        input.val('').show();
    };
    // EasySearch选中后赋值
    Toolkit.ui.EasySearchChecked = function (input, data) {
        var hdinput = input.siblings('[name=hd' + input.attr('name') + ']');
        if (data) {
            hdinput.val(data.id).trigger('change');
            input.val(data.text).blur();
            input.showInputValidStatus();
        }
        if (input.val() === '') return;
        input.hide();
        input.before('<span class="easysearch-checked rad3">' + input.val() + ' <button type="button" title="删除" class="close easysearch-close" aria-hidden="true">&times;</button></span>');
        input.siblings('.easysearch-checked').find('button').click(function () {
            $(this).parent().remove();
            hdinput.val('').trigger('change');
            input.val('').show();
        });
        input.trigger('AfterItemSelected', data);
    };
    // 打开EasySearch弹窗
    Toolkit.ui.EasySearchPopWin = function (input, data) {
        Toolkit.page.currentEasySearchElement = input;
        var url = data.source.substitute(data);
        if (data.refFields) {
            var refdata = {};
            refdata['REF'] = [];
            for (var item in data.refFields) {
                var itemValue = Toolkit.data.getElementValue(data.refFields[item]);
                refdata['REF'].push({ Field: item, RefValue: itemValue });
            }
            url += '&RefValue=' + encodeURIComponent(Toolkit.json.stringify(refdata)) + '&Format=Json';
        }
        Toolkit.page.dialog.pop({
            title: '选择' + data.title,
            url: url,
            width: 250,
            height: 242
        });
    };
    // 弹窗中返回EasySearch所选数据
    Toolkit.ui.setEasySearchData = function (data) {
        var input = Toolkit.page.currentEasySearchElement;
        var controlType = input.parents('.tk-control').attr('dataControl');
        if (controlType == 'MultiEasySearch') {
            input.bindToolkitUI('MultiEasySearchChecked', data);
        } else {
            input.siblings('span.easysearch-checked').remove();
            input.bindToolkitUI('EasySearchChecked', data);
        }
    };
    // ----------------------------
    // 绑定日期控件
    Toolkit.ui.dateControl = function (elm) {
        Toolkit.load('datetimepicker', function () {
            elm.each(function () {
                var element = $(this);
                element.datetimepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    todayBtn: true,
                    minView: 2,
                });
            });
        });
    };
    Toolkit.ui.dateTimeControl = function (elm) {
        Toolkit.load('datetimepicker', function () {
            elm.each(function () {
                var element = $(this);
                element.datetimepicker({
                    language: 'zh-CN',
                    todayBtn: true,
                    autoclose: true,
                    todayHighlight: true,
                    showMeridian: true
                });
            });
        });
    };
    Toolkit.ui.timeControl = function (elm) {
        Toolkit.load('datetimepicker', function () {
            elm.each(function () {
                var element = $(this);
                element.datetimepicker({
                    language: 'zh-CN',
                    todayBtn: true,
                    autoclose: true,
                    todayHighlight: true,
                    startView: 1,
                    minView: 0,
                    maxView: 1,
                    forceParse: false
                });
            });
        });
    };
    // ----------------------------
    // 绑定HTML编辑器
    Toolkit.ui.htmlEditor = function (elm) {
        Toolkit.load('kindeditor', function () {
            elm.each(function (i) {
                var textarea = $(this);
                var isReadOnly = textarea.attr('data-control') == 'DetailHTML';
                var options = isReadOnly ? { readonlyMode: true } : {};
                var editor = KindEditor.create('textarea[name="' + textarea.attr("name") + '"]', options);
                textarea.data("editor", editor);
            });
        });
    };
})(jQuery);