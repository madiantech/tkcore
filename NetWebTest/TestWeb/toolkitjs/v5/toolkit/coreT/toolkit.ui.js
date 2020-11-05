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
        elm.bindToolkitUI('loadDataSection');
        elm.bindToolkitUI('dataLinkExecute');
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
    Toolkit.ui.DataFormListExecute = function (elm) {
        elm.bindToolkitUI('dataFormListActionExecute');
    };
    Toolkit.ui.DataSearchExecute = function (elm) {
        elm.bindToolkitUI('DataFormElementExecute', 'detail');
    };
    Toolkit.ui.DataFormDetailExecute = function (elm) {
        // 避免list-table下的隐藏模板里的tk-datatable被解析
        elm.find('.tk-datatable').each(function () {
            //if ($(this).parent().tagName() == 'td') return;
            $(this).bindToolkitUI('DataDetailElementExecute');
            $(this).bindToolkitUI('DataFormElementExecute', 'detail');
        });
    };
    Toolkit.ui.DataDetailElementExecute = function (elm) {
        elm.find('[dataControl=DetailHTML]').each(
            function (index, element) {
                $(element).bindToolkitUI('HtmlEditor');
            });
    };
    Toolkit.ui.DataFormElementExecute = function (elm, type) {
        // 基本的UI处理
        //elm.find('[dataControl=Date], [dataControl=DateTime]').bindToolkitUI('DatePicker');
        elm.find('[data-control=EasySearch]').not('tbody.template [data-control=EasySearch]').each(
            function (index, element) {
                $(element).bindToolkitUI('easySearchControl');
            });
        elm.find('[data-control=MultipleEasySearch]').not('tbody.template [data-control=MultipleEasySearch]').each(
            function (index, element) {
                $(element).bindToolkitUI('multiEasySearchControl');
            });
        elm.find('[data-control=HTML]').not('tbody.template [data-control=HTML]').each(
            function (index, element) {
                $(element).bindToolkitUI('htmlEditor');
            });
        elm.find('[data-control=DetailHTML]').not('tbody.template [data-control=DetailHTML]').each(
            function (index, element) {
                $(element).bindToolkitUI('htmlEditor');
            });
        elm.find('[data-control=Date]').not('tbody.template [data-control=Date]').each(
            function (index, element) {
                $(element).bindToolkitUI('dateControl');
            });
        elm.find('[data-control=DateTime]').not('tbody.template [data-control=DateTime]').each(
            function (index, element) {
                $(element).bindToolkitUI('dateTimeControl');
            });
        elm.find('[data-control=Time]').not('tbody.template [data-control=Time]').each(
            function (index, element) {
                $(element).bindToolkitUI('timeControl');
            });
        elm.find('[data-control=Upload]').not('tbody.template [data-control=Upload]').each(
            function (index, element) {
                $(element).bindToolkitUI('uploadControl');
            });
        elm.find('[data-control=CheckBox]').not('tbody.template [data-control=CheckBox]').each(
            function (index, element) {
                $(element).bindToolkitUI('checkBoxControl');
            });
        //elm.find('[dataControl=MultiEasySearch]').bindToolkitUI('MultiEasySearch');

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
    Toolkit.ui.dataFormListActionExecute = function (elm) {
        // 选中状态修改
        elm.find('tbody.list input.row-index').bindToolkitUI('_addDataRowCheck');
        // 全选处理
        elm.find('thead input.e-checkall').click(function () {
            $(this).parents('table').find('tbody.list input.row-index').bindToolkitUI('_checkAll', this);
        });
        // 选择按钮处理
        elm.find('Button.ui-btn-checkall').click(function () {
            elm.find('thead input.e-checkall').prop('checked', true);
            elm.find('tbody.list input.row-index').bindToolkitUI('_checkAll', true);
            return false;
        });
        elm.find('Button.ui-btn-checknone').click(function () {
            elm.find('thead input.e-checkall').prop('checked', false);
            elm.find('tbody.list input.row-index').bindToolkitUI('_checkAll', false);
            return false;
        });
        elm.find('Button.ui-btn-checkreverse').click(function () {
            elm.find('tbody.list input.row-index').bindToolkitUI('_checkReverse');
            return false;
        });
        // 删除处理
        elm.find('.ui-btn-delrow').click(function () {
            var rowlist = elm.find('tbody.list input.row-index:checked');
            var rowCount = rowlist.length;
            if (rowCount === 0) {
                Toolkit.page.alert('请选择要删除的数据行');
                return false;
            }
            var element = $(this);
            Toolkit.page.confirm('确认删除所选择的数据行？', function () {
                Toolkit.stat.addFuncStat('Toolit.ui.DeleteDataRow-' + rowCount);
                var table = element.parents('.panel').find('table');
                if (rowCount === table.find('tbody.list tr').size()) {
                    table.find('tbody.list>tr').remove();
                    //table.bindToolkitData('DeleteAllDataSetRow');
                } else {
                    for (var i = rowCount - 1; i >= 0; i--) {
                        var selector = 'tbody.list>tr:eq(' + ($(rowlist[i]).val() - 1) + ')';
                        table.find(selector).remove();
                    }
                    table.find('tbody.list>tr').each(function (i) {
                        $(this).find('input.row-index').val(i + 1);
                        $(this).find('span.row-index').html(i + 1);
                    });
                    //table.bindToolkitData('DeleteDataSetRow', rowlist);
                }
                //Toolkit.ui.UpdateDataFormHeight();
                Toolkit.ui._onAddDelRow(element, "rowdeleted", rowCount);
                Toolkit.stat.addFuncStat('Toolit.ui.DeleteDataRow-' + rowCount);
            });
            return false;
        });
        elm.find('.ui-btn-delall').click(function () {
            var element = $(this);
            Toolkit.page.confirm('确认删除全部数据行？', function () {
                var table = element.parents('.panel').find('table');
                table.find('tbody.list tr').remove();
                Toolkit.ui._onAddDelRow(element, "rowdeleted", -1);
                //table.bindToolkitData('DeleteAllDataSetRow');
                //Toolkit.ui.UpdateDataFormHeight();
            });
            return false;
        });
        // 新建列表
        elm.find('.ui-newrow input[type=text]').keydown(function (e) {
            try {
                var code = e.which || e.keyCode || 0;
                if (code === 13) {
                    $(this).blur();
                    $(this).next().click();
                }
            } catch (err) { }
        });
        elm.find('.ui-newrow button.ui-btn-new').click(function () {
            var element = $(this);
            var txtrow = element.prev();
            var count = txtrow.val();
            if (count === '' || !Toolkit.string.isPositiveInteger(count)) {
                Toolkit.page.alert('请输入要新建的行数', function () {
                    txtrow.focus();
                });
                return false;
            }
            if (count > 100) {
                Toolkit.page.alert('一次最多只能新建100行，请修改行数', function () {
                    txtrow.focus();
                });
                return false;
            }
            //if (count > 10) Toolkit.page.showLoading('正在创建数据行……');
            setTimeout(function () {
                element.parents('.panel').find('table').bindToolkitUI('_createDataRow', count);
                Toolkit.ui._onAddDelRow(element, "rowadded", count);
                //if (count > 10) Toolkit.page.hideLoading();
            }, 100);
            return false;
        });
    };

    Toolkit.ui._onAddDelRow = function (elm, name, count) {
        var div = elm.closest("div.tk-datatable");
        var funcName = div.data(name);
        var func = Toolkit.data.getFunction(funcName);
        if (func !== null) {
            func(div.attr("id"), count);
        }
    };

    Toolkit.ui._createDataRow = function (elm, count, defaultValue) {
        Toolkit.stat.addFuncStat('Toolit.ui.CreateDataRow-' + count);
        //elm.find('tbody.list .tk-datatable').trigger('collapseRowForm');
        var row = elm.find('tbody.template>tr');
        var rowCount = elm.find('tbody.list>tr').size();
        var useDefault = defaultValue && $.isArray(defaultValue) && defaultValue.length == count;
        for (var i = 0; i < count; i++) {
            var newrow = row.clone();
            newrow.addClass("table-row");
            if (useDefault) {
                var obj = defaultValue[i];
                $.each(obj, function (key, value) {
                    var ctrl = newrow.find('[name=' + key + ']');
                    if (ctrl.attr("type") == "checkbox") {
                        ctrl.prop("checked", value == "1");
                    }
                    else
                        ctrl.val(value);
                });
            }
            elm.find('tbody.list').append(newrow);
        }
        var rows = (rowCount > 0) ? elm.find('tbody.list>tr:gt(' + (rowCount - 1) + ')') : elm.find('tbody.list>tr');
        rows.each(function (i) {
            $(this).find('input.row-index').val(rowCount + i + 1);
            $(this).find('span.row-index').html(rowCount + i + 1);
        });
        rows.find('input.row-index').bindToolkitUI('_addDataRowCheck');
        rows.bindToolkitUI('DataDetailElementExecute');
        rows.bindToolkitUI('DataFormElementExecute', 'list');
        //rows.bindToolkitUI('UpdateFormRadioGroup');
        //Toolkit.ui.UpdateDataFormHeight();
        Toolkit.stat.addFuncStat('Toolit.ui.CreateDataRow-' + count);
    };
    Toolkit.ui._addDataRowCheck = function (elm) {
        elm.click(function () {
            $(this).bindToolkitUI('_checkDataRow');
        });
    };
    // ----------------------------
    // 全选
    Toolkit.ui._checkAll = function (elm, chkSource) {
        var checked = true;
        if (typeof (chkSource) == 'object') {
            checked = chkSource.checked;
        } else if (typeof (chkSource) == 'boolean') {
            checked = chkSource;
        }
        elm.each(function () {
            this.checked = checked;
            Toolkit.ui._checkDataRow($(this));
        });
    };
    // 反选
    Toolkit.ui._checkReverse = function (elm) {
        elm.each(function () {
            this.checked = !this.checked;
            Toolkit.ui._checkDataRow($(this));
        });
    };
    // 选中或取消选中当前行
    Toolkit.ui._checkDataRow = function (elm) {
        if (!elm.hasClass("e-checkdatarow")) return;
        if (elm.prop("checked")) {
            elm.parents("tr").addClass("active");
        } else {
            elm.parents("tr").removeClass("active");
        }
    };
    Toolkit.ui._locationUrl = function (url, isRetUrl, newTarget) {
        if (isRetUrl) {
            if (newTarget)
                window.open(url);
            else
                document.location.href = url;
            return;
        }
        var retUrl = $("body").triggerHandler("SelfUrl");
        var destUrl = "";
        if (retUrl === undefined || retUrl === "")
            destUrl = url;
        else {
            var uri = new URI(url);
            var params = uri.search(true);
            params.RetURL = retUrl;
            uri.search(params);
            destUrl = uri.toString();
        }
        if (newTarget)
            window.open(destUrl);
        else
            document.location.href = destUrl;
    };

    Toolkit.ui._ajaxUrl = function (url, method) {
        $.ajax({
            type: method, dataType: 'text', url: url,
            success: _successCommit,
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    }

    Toolkit.ui.dataLinkExecute = function (elm) {
        elm.find("button[data-url],a[data-url]").click(function () {
            var obj = $(this);
            var url = obj.attr("data-url");
            var useRetUrl = obj.attr("data-action") === "return";
            if (url === undefined || url === "" || url === "#")
                return;
            var confirm = obj.attr("data-confirm");
            var newTarget = obj.attr("data-target") == "_blank";
            if (confirm === undefined || confirm === "")
                Toolkit.ui._locationUrl(url, useRetUrl, newTarget);
            else
                Toolkit.page.confirm(confirm, function () { Toolkit.ui._locationUrl(url, useRetUrl, newTarget); });
        });
        elm.find("button[data-ajax-url],a[data-ajax-url]").click(function () {
            var obj = $(this);
            var url = obj.attr("data-ajax-url");
            if (url === undefined || url === "" || url === "#")
                return;
            var confirm = obj.attr("data-confirm");
            var method = obj.attr("data-method");
            if (method != "post")
                method = "get";
            if (confirm === undefined || confirm === "")
                Toolkit.ui._ajaxUrl(url, method);
            else
                Toolkit.page.confirm(confirm, function () { Toolkit.ui._ajaxUrl(url, method); });
        });
        elm.find("button[data-dialog-url],a[data-dialog-url]").click(function () {
            var obj = $(this);
            var url = obj.attr("data-dialog-url");
            if (url === undefined || url === "")
                return;
            var title = obj.attr("data-title");
            var height = $("body").attr("data-dialog-height");
            if (height == 0)
                height = 400;
            var options = { "title": title, "url": url, "height": height };
            $.extend(options, obj.attrJSON('data-dialog-param'));
            Toolkit.page.dialog.pop(options);
        });
        elm.find("button[data-dialog-page],a[data-dialog-page]").click(function () {
            var obj = $(this);
            var url = obj.attr("data-dialog-page");
            if (url === undefined || url === "")
                return;

            var title = obj.attr("data-title");
            var height = $("body").attr("data-dialog-height");
            if (height == 0)
                height = 400;
            var options = { "title": title, "url": url, "height": height };
            $.extend(options, obj.attrJSON('data-dialog-param'));
            Toolkit.page.dialog.ajax(options);
        });
        elm.find("button[data-dialog-action=close]").click(function () {
            Toolkit.page.closeDialog();
        });
    };
    // 绑定EasySearch处理
    Toolkit.ui.easySearchControl = function (elm) {
        Toolkit.load('easysearch', function () {
            elm.each(function () {
                var element = $(this);
                var inputElement = element.find('input[type=text]');
                //var fieldStr = element.attr('dataRefField') || '';
                var refFields = inputElement.attr("data-refFields");
                if (refFields && refFields != "") {
                    var refs = refFields.parseJSON();
                    for (var i = 0; i < refs.length; ++i) {
                        var item = refs[i];
                        //var itemValue = Toolkit.data.getElementValue(options.refFields[item]);
                        var ctrl = Toolkit.data._getCtrl(inputElement, item.RefField, item.CtrlType);
                        var retCtrl = null;
                        if (ctrl.HiddenControl)
                            retCtrl = ctrl.HiddenControl;
                        else
                            retCtrl = ctrl.Control;
                        retCtrl.on('change', function () {
                            inputElement.bindToolkitUI('easySearchEmpty');
                        });
                    }
                }

                var selectFunc = Toolkit.data.getFunction(inputElement.attr("data-selectedFunc"));
                if (selectFunc !== null) {
                    inputElement.on("afterItemSelected", selectFunc);
                }
                inputElement.bindToolkitUI('suggest', {
                    source: element.attr('data-url'),
                    refFields: refFields,
                    onBeforeSuggest: function () {
                        inputElement.trigger('beforeEasySearch');
                    },
                    onSelect: function (input, id, text) {
                        input.bindToolkitUI('easySearchChecked', { id: id, text: text });
                    }
                });
                inputElement.bindToolkitUI('easySearchChecked');
                element.find('button.btn').click(function () {
                    inputElement.trigger('beforeEasySearch');
                    var hiddenElement = element.find("input[type=hidden]");
                    inputElement.bindToolkitUI('easySearchDialog', {
                        url: element.attr("data-dialog-url"),
                        initValue: hiddenElement.val() || '',
                        title: inputElement.attr("data-title"),
                        refFields: refFields
                    });
                });
            });
        });
    };
    Toolkit.ui.easySearchEmpty = function (input) {
        var hdinput = input.siblings('[name=hd' + input.attr('name') + ']');
        input.siblings('.easysearch-checked').remove();
        hdinput.val('').trigger('change');
        input.val('').show();
    };
    // EasySearch选中后赋值
    Toolkit.ui.easySearchChecked = function (input, data) {
        var hdinput = input.siblings('[name=hd' + input.attr('name') + ']');
        if (data) {
            hdinput.val(data.id).trigger('change');
            input.val(data.text).blur();
            input.showInputValidStatus();
        }
        if (input.val() === '') return;
        input.hide();
        input.before('<span class="easysearch-checked rad3">' + input.val()
            + ' <button type="button" title="删除" class="close easysearch-close" aria-hidden="true">&times;</button></span>');
        input.siblings('.easysearch-checked').find('button').click(function () {
            $(this).parent().remove();
            hdinput.val('').trigger('change');
            input.val('').show();
        });
        input.trigger('afterItemSelected', data);
    };
    // 打开EasySearch弹窗
    Toolkit.ui.easySearchDialog = function (input, data) {
        Toolkit.page.currentEasySearchElement = input;
        var refString = "";
        if (data.refFields && data.refFields != "") {
            var refs = data.refFields.parseJSON();
            var refField = [];
            for (var i = 0; i < refs.length; ++i) {
                var item = refs[i];
                //var itemValue = Toolkit.data.getElementValue(options.refFields[item]);
                var ctrl = Toolkit.data._getCtrl(input, item.RefField, item.CtrlType);
                var refValue;
                if (ctrl.HiddenControl)
                    refValue = ctrl.HiddenControl.val();
                else
                    refValue = ctrl.Control.val();
                refField.push({ "NickName": item.Field, "Value": refValue });
            }
            refString = Toolkit.json.stringify(refField);
        }
        var url = Toolkit.page.addQueryString(data.url, { "InitValue": data.initValue, "RefValue": refString });
        Toolkit.page.dialog.pop({ "title": "选择" + data.title + "...", "url": url });
    };
    Toolkit.ui.easySearchDialogTreeClick = function (node, selected) {
        if (selected.node) {
            var id = selected.node.id;
            var text = selected.node.text;
            parent.Toolkit.ui.setEasySearchData({ id: id, text: text });
            Toolkit.page.closeDialog();
        }
    };
    // 弹窗中返回EasySearch所选数据
    Toolkit.ui.setEasySearchData = function (data) {
        var input = Toolkit.page.currentEasySearchElement;
        var controlType = input.parents('div[data-control]').attr('data-control');
        if (controlType === 'MultipleEasySearch') {
            input.bindToolkitUI('multiEasySearchChecked', data);
        } else {
            input.siblings('span.easysearch-checked').remove();
            input.bindToolkitUI('easySearchChecked', data);
        }
    };
    // ----------------------------
    // 绑定MultiEasySearch处理
    Toolkit.ui.multiEasySearchControl = function (elm) {
        Toolkit.load('easysearch', function () {
            elm.each(function () {
                var element = $(this);
                var inputElement = element.find('input[type=text]');
                var refFields = inputElement.attr("data-refFields");
                if (refFields && refFields != "") {
                    var refs = refFields.parseJSON();
                    for (var i = 0; i < refs.length; ++i) {
                        var item = refs[i];
                        var ctrl = Toolkit.data._getCtrl(inputElement, item.RefField, item.CtrlType);
                        var retCtrl = null;
                        if (ctrl.HiddenControl)
                            retCtrl = ctrl.HiddenControl;
                        else
                            retCtrl = ctrl.Control;
                        retCtrl.on('change', function () {
                            inputElement.bindToolkitUI('multiEasySearchEmpty');
                        });
                    }
                }
                inputElement.bindToolkitUI('suggest', {
                    source: element.attr('data-url'),
                    refFields: refFields,
                    onBeforeSuggest: function () {
                        inputElement.trigger('beforeEasySearch');
                    },
                    onSelect: function (input, id, text) {
                        input.bindToolkitUI('multiEasySearchChecked', { id: id, text: text });
                    }
                });
                inputElement.bind('keypress', function (e) {
                    try {
                        if (e.shiftKey) return false;
                        var code = e.which || e.keyCode || 0;
                        if (this.value == '' && code == 8) {
                            var data = element.prev().find('span.easysearch-checked:last');
                            if (!data.hasClass('hidden')) {
                                data.find('button').click();
                            }
                        }
                    } catch (err) { }
                });
                inputElement.bindToolkitUI('multiEasySearchChecked');
                element.find('span.mr20').click(function () {
                    inputElement.focus();
                });
                element.find('button.btn').click(function () {
                    inputElement.trigger('beforeEasySearch');
                    inputElement.bindToolkitUI('easySearchDialog', {
                        title: inputElement.attr("data-title") || '',
                        url: element.attr("data-dialog-url") || '',
                        refFields: refFields
                    });
                });
            });
        });
    };
    // MultiEasySearch选中后赋值
    Toolkit.ui.multiEasySearchChecked = function (input, data) {
        if (data) {
            var multiData = input.parent().prev();
            if (multiData.find('span.easysearch-checked[data-id=' + data.id + ']').size() > 0) return;
            var elm = multiData.find('span.easysearch-checked:first').clone();
            elm.removeClass('hidden').addClass('multi-item');
            elm.attr("data-id", data.id);
            elm.attr("data-name", data.text);
            elm.find('em').html(data.text);
            multiData.append(elm);
            multiData.find('span.hidden').remove();
            input.bindToolkitUI('multiEasySearchRemove');
            input.bindToolkitUI('multiEasySearchResize');
            //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
            //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
            input.trigger('afterItemSelected', data);
        } else {
            input.bindToolkitUI('multiEasySearchRemove', 'all');
            input.bindToolkitUI('multiEasySearchResize');
        }
    };
    Toolkit.ui.multiEasySearchEmpty = function (input) {
        input.siblings('span.easysearch-checked:gt(0)').remove();
        input.siblings('span.easysearch-checked').removeClass('multi-item').addClass('hidden')
            .attr("data-id", "").attr("data-name", "");
        input.siblings('span.easysearch-checked').find('input').val('');
        input.siblings('span.easysearch-checked').find('em').html('');
        input.bindToolkitUI('multiEasySearchResize');
        //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
        //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
    };
    Toolkit.ui.multiEasySearchRemove = function (input, type) {
        var multiDatas = input.parent().prev().find('span.easysearch-checked');
        var element = (type) ? multiDatas : multiDatas.last();
        element.find('button').click(function () {
            var datas = $(this).parents('div.multi-data').find('span.easysearch-checked');
            if (datas.size() > 1) {
                $(this).parent().remove();
            } else {
                var data = $(this).parent();
                data.removeClass('multi-item').addClass('hidden');
                data.attr("data-id", "").attr("data-name", "");
                data.find('em').html('');
            }
            input.bindToolkitUI('multiEasySearchResize');
            //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
            //input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
        });
    };
    Toolkit.ui.multiEasySearchResize = function (input) {
        var w = 0;
        var div = input.parent();
        div.siblings().find("span.multi-item").each(function () {
            w += $(this).outerWidth() + 5;
        });
        div.css('width', (div.parent().parent().width() - w - 15) + 'px');
        input.val('');
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
                    todayBtn: false,
                    autoclose: true,
                    todayHighlight: true,
                    startView: 1,
                    minView: 0,
                    maxView: 1,
                    minuteStep: 3,
                    forceParse: false
                });
            });
        });
    };
    Toolkit.ui.checkBoxControl = function (elm) {
        if (elm.hasClass("switch")) {
            //var options = { "onText": elm.attr("data-on-label"), "offText": elm.attr("data-off-label") };
            Toolkit.load('switcher', function () {
                elm.find(":checkbox").bootstrapSwitch();
            });
        }
    };

    // ----------------------------
    // 绑定HTML编辑器
    // 上传HtmlUpload页面的参数说明：MaxSize：上传文件的最大长度；RelativePath：返回相对路径(true/false)
    //    UploadPath：上传文件的存储目录；VirtualPath：上传文件的虚拟目录
    Toolkit.ui.htmlEditor = function (elm) {
        Toolkit.load('kindeditor', function () {
            elm.each(function (i) {
                var textarea = $(this);
                var uploadPath = textarea.attr('data-uploadPath');
                if (uploadPath === undefined)
                    uploadPath = Toolkit.page.getAbsoluteUrl("Library/WebInitPage.tkx?Source=HtmlUpload");
                var params = { "RelativePath": false, "VirtualPath": "" };
                var useRelative = textarea.attr("data-useRelative");
                if (useRelative === "true")
                    params.RelativePath = true;
                params.VirtualPath = textarea.attr("data-virtualPath");
                uploadPath = Toolkit.page.addQueryString(uploadPath, params);

                var baseOptions = { "uploadJson": uploadPath, "readonlyMode": false, "formatUploadUrl": false };
                var isReadOnly = textarea.attr('data-control') == 'DetailHTML';
                if (isReadOnly)
                    baseOptions.readonlyMode = true;
                var editor = KindEditor.create('textarea[name="' + textarea.attr("name") + '"]', baseOptions);
                textarea.data("editor", editor);
            });
        });
    };

    // ----------------------------
    // 绑定Upload控件

    Toolkit.ui._previewImg = function (source, div) {
        var img = $("<img></img>");
        img.attr("src", source);
        div.empty().append(img);
    };

    Toolkit.ui._deleteUploaded = function () {
        var obj = $(this);
        var elm = obj.parents("div[data-control=Upload]");
        //elm.data("upload", undefined);
        elm.removeData("upload");
        elm.find(".upload-status").hide();
        elm.find(".upload-info").html("");
        var input = elm.find(":file");
        input.show();
        input.replaceWith(input.val('').clone(true));
    };

    Toolkit.ui._successUploaded = function (data, elm, file, info) {
        if (data && data.Result) {
            var result = data.Result.Result;
            if (result === "Success") {
                var upload = data.UploadInfo;
                elm.data("upload", upload);
                //elm.attr("data-upload", upload);
                Toolkit.ui._showUploadItem(upload, elm, file, elm.find(".upload-status"));
                // Toolkit.ui._showUploadItem(upload, elm, file, info); Update by glq 2016.4.11
            }
            else if (result === "Error") {
                Toolkit.page.showError(data.Result.Message);
            }
        }
        else {
            Toolkit.page.showError('上传失败，请稍后重试。');
        }
    };

    Toolkit.ui._showUploadItem = function (upload, elm, file, info) {
        file.hide();
        //elm.hide();
        info.html('<span><a href="../' + upload.WebPath + '" target="_blank">' + upload.FileName + '</a><button type="button" class="close easysearch-close" aria-hidden="true">&times;</button></span>');
        info.find("button.close").click(Toolkit.ui._deleteUploaded);
        elm.trigger("afterItemSelected", { "Upload": upload, "Element": elm });
    };

    Toolkit.ui._onFileChange = function () {
        var fileInput = $(this);
        var parent = fileInput.parent("[data-control=Upload]");
        var response = fileInput.siblings(".upload-status").show();
        var responseInfo = response.find("div.upload-info").html('<p><i class="icon-spinner icon-spin mr5 ml5"></i>正在上传……</p>');

        var formdata = false;
        if (window.FormData) {
            formdata = new FormData();
        }

        var len = this.files.length;
        for (var i = 0; i < len; i++) {
            var file = this.files[i];

            if (window.FileReader) {
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    //if (ctrl.attr("data-view") == "true")
                    //    Toolkit.ui._showUploadedItem(e.target.result, resDiv.find("div.responseImage"));
                };
                reader.readAsDataURL(file);
            }
            if (formdata) {
                formdata.append("Filedata", file);
            }
        }

        if (formdata) {
            var url = parent.attr("data-url");
            if (url === undefined || url === "") {
                url = Toolkit.page.getAbsoluteUrl("Library/WebInitPage.tkx?Source=WebUpload");
            }
            else
                url = Toolkit.page.getAbsoluteUrl(url);
            $.ajax({
                url: url,
                type: "Post",
                data: formdata,
                processData: false,
                contentType: false,
                success: function (res) {
                    Toolkit.ui._successUploaded(res, parent, fileInput, responseInfo);
                },
                error: function (res) {
                    Toolkit.page.showError('上传失败，请稍后重试。');
                }
            });
        }
    };

    Toolkit.ui.uploadControl = function (elm) {
        var input = elm.find(":file");
        var value = elm.attr("data-value");
        var response = elm.find(".upload-status")
        if (value === undefined) {
            response.hide();
            elm.data("upload", undefined);
        }
        else {
            response.show();
            var upload = value.parseJSON();
            elm.data("upload", upload);
            Toolkit.ui._showUploadItem(upload, response.find("div.upload-info"), input, response);
        }
        if (!window.FormData) {
            alert("系统不支持HTML5上传");
            input.attr("disabled", "disabled");
        }
        var selectFunc = Toolkit.data.getFunction(elm.attr("data-selectedFunc"));
        if (selectFunc !== null) {
            elm.on("afterItemSelected", selectFunc);
        }
        input.change(Toolkit.ui._onFileChange);
    };

    Toolkit.ui.enableButton = function (isEnabled) {
        if (isEnabled) {
            $("select[data-auto-query=true]").removeAttr("disabled");
            $("button.auto-disabled").removeClass("auto-disabled").removeAttr("disabled");
        }
        else {
            $("select[data-auto-query=true]").attr("disabled", "disabled");
            $("button:enabled").addClass("auto-disabled").attr("disabled", "disabled");
        }
    };

    Toolkit.ui.loadDataSection = function (elm) {
        elm.find('div[data-load-url]').each(function () {
            var element = $(this);
            var url = element.attr("data-load-url");
            element.load(url, function (responseText, textStatus, XMLHttpRequest) {
                element.html(responseText);
            });
        });
    };
})(jQuery);