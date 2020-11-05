(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.data');

    Toolkit.data.getElementType = function (elm) {
        var tag = elm.tagName();
        if (tag === 'input') {
            return elm.attr('type') || 'text';
        }
        return tag;
    };

    Toolkit.data.getElement = function (frm, field) {
        var type = field.Type;
        switch (type) {
            case "Radio":
                return frm.find("input[name=" + field.Name + "]");
            default:
                return $("#" + field.Name);
        }
    };

    // 获取选择的值
    Toolkit.data.getCheckedValue = function (elm, quote) {
        var arr = [];
        elm.each(function () {
            if (this.checked) arr.push((quote) ? ('\"' + this.value + '\"') : this.value);
        });
        return arr;
    };

    Toolkit.data.getElementValue = function (elm, type) {
        if (elm.size() === 0) return '';
        switch (type) {
            case 'CheckBox':
                //var control = elm.closest('.tk-control').attr('dataControl') || '';
                //return (control == 'CheckBoxList') ? Toolkit.data.getCheckedValue(elm, true).join(',') : Toolkit.data.getCheckedValue(elm).join(',');
                if (elm.get(0).checked)
                    return elm.val();
                else
                    return elm.attr("data-uncheck");
            case 'Radio':
                return Toolkit.data.getCheckedValue(elm).join(',');
            case 'Detail':
                return elm.attr("data-value");
            case 'Custom':
                var func = elm.attr("data-getValue");
                if (func !== undefined && func !== "") {
                    var resp = window[func];
                    if ($.isFunction(resp))
                        return resp(elm);
                }
                return elm.val();
            default:
                return elm.val();
        }
    };

    Toolkit.data.getJson = function (frm) {
        var postObj = frm.attr("data-post");
        if (!postObj)
            return undefined;

        var postTables = postObj.parseJSON().Tables;
        var postData = {};
        for (var i = 0; i < postTables.length; i++) {
            var table = postTables[i];
            var array = [];
            var item = {};
            for (var j = 0; j < table.Fields.length; j++) {
                var field = table.Fields[j];
                var elm = Toolkit.data.getElement(frm, field);
                Toolkit.data.checkInputValid(elm, field.Type);
                if (elm.data('valid') && elm.data('error')) {
                    Toolkit.page.alert(elm.data("errorText"), function () {
                        if (elm.is(':visible')) {
                            elm.focus();
                        }
                    });
                    return false;
                }

                var fieldValue = Toolkit.data.getElementValue(elm, field.Type);
                item[field.Name] = fieldValue;
            }
            array.push(item);
            postData[table.Table] = array;
        }
        return postData;
    };

    Toolkit.data.getUrl = function (frm) {
        var obj = $(this);
        var url = obj.attr("data-url");
        if (!url || url === "") {
            url = frm.attr("action");
            if (!url || url === "." || url === "")
                url = window.location.href;
        }
        return url;
    };

    Toolkit.data.updateInputsValid = function (errors) {
        for (var i = 0; i < errors.length; i++) {
            var error = errors[i];
            var field = error.NickName;
            var msg = error.Message;
            var input = $("#" + field);
            input.parents("span.tk-control").children("label.errorText").text(msg).show();
            if (i === 0) {
                if (input.is(':visible'))
                    input.focus();
            }
        }
    };

    _successCommit = function (req) {
        var data = req.parseJSON();
        if (data && data.Result) {
            var result = data.Result.Result;
            if (result === "Success") {
                if (data.AlertMessage && data.AlertMessage != "")
                    //Toolkit.page.alert(data.AlertMessage);
                    alert(data.AlertMessage);
                var message = data.Result.Message;
                if (message === undefined || message === "") {
                }
                else if (message === "WeixinClose") {
                    Toolkit.page.weixinClose();
                }
                else if (message === "Refresh") {
                    document.location.reload();
                }
                else if (message === "Back") {
                    //history.back();
                    window.location.href = document.referrer;
                }
                else
                    document.location.href = data.Result.Message;
            }
            else if (result === "Error") {
                if (data.FieldInfo) {
                    Toolkit.data.updateInputsValid(data.FieldInfo);
                }
                else
                    Toolkit.page.showError(data.Result.Message);
            }
        }
        else {
            Toolkit.page.showError('数据提交失败！请稍候重试。');
        }
    };

    Toolkit.data._getForm = function (frmId, currentObj, defaultName) {
        var formId = defaultName;
        if (typeof (frmId) == "string" && frmId != null)
            formId = frmId;
        else if (typeof (frmId) == "object") {
            var dataForm = currentObj.attr("data-form");
            if (dataForm)
                formId = dataForm;
        }
        var frm = $("#" + formId);
        return frm;
    };

    Toolkit.data.post = function (frmId) {
        var currentObj = $(this);
        var frm = Toolkit.data._getForm(frmId, currentObj, "PostForm");
        if (!frm)
            return;
        $("label.errorText").text("").hide();

        var postData = Toolkit.data.getJson(frm);
        var url = Toolkit.data.getUrl(frm);
        var response = currentObj.attr("data-response");
        var func = response ? window[response] : _successCommit;
        if (func)
            if (postData) {
                //alert(Toolkit.json.stringify(postData));
                $.ajax({
                    type: 'post', dataType: 'text', url: url, data: Toolkit.json.stringify(postData),
                    success: func,
                    error: function () {
                        Toolkit.page.showError('数据提交失败！请稍候重试。');
                    }
                });
            }
        return false;
    };

    // 获取选择数量
    Toolkit.data.getCheckedSize = function (elm) {
        var checkedSize = 0;
        elm.each(function () { if (this.checked) checkedSize++; });
        return checkedSize;
    };

    Toolkit.data.checkInputValid = function (elm, type) {
        if (elm.data('valid')) return;
        elm.data('valid', true).data('error', false);
        switch (type)
        {
        	case "Text":
            case "TextArea":
                if (elm.attr("required")) {
                    if (elm.val() == "") {
                        elm.data("errorText", "请输入" + elm.attr("data-title")).data("error", true).data("valid", false);
                    }
                }
        		break;
        }
        elm.trigger('valid');
    };

    Toolkit.data.clearError = function () {
        $("span.tk-control>label.errorText").hide();
    };

    Toolkit.data.showError = function (elm, error) {
        elm.next().text(error).show();
    };

    _deletePage = function (obj) {
        url = obj.attr("data-url");
        $.ajax({
            type: 'get', dataType: 'text', url: url,
            success: _successCommit,
            error: function () {
                Toolkit.page.showError('数据提交失败！请稍候重试。');
            }
        });
    };

    Toolkit.data.deleteData = function () {
        var obj = $(this);
        var prompt = obj.attr("data-prompt");
        if (prompt === undefined || prompt === "")
            prompt = "确定删除吗？";
        Toolkit.page.confirm(prompt, function () {
            _deletePage(obj);
        });
    };

    $.fn.showInputValidStatus = function (error) {
        this.data('error', false).data('valid', true);
        var errorObj = this.parents("span.tk-control").children("label.errorText");
        errorObj.hide();
        // this.closest('.tk-control').parent().removeClass('error').attr('title', '');
        if (error) {
            this.data('error', true);
            errorObj.text(error).show();
            //this.closest('.tk-control').parent().addClass('error').attr('title', error);
            // 为没有valid的元素增加一个change事件，用于后台校验返回后的内容修改后清除error
            if (!this.data('validBinded')) {
                this.change(function () {
                    $(this).showInputValidStatus();
                });
                this.data('validBinded', true);
            }
        }
    };
    // ----------------------------
    // 获取tagName
    $.fn.tagName = function () {
        if (this.length === 0) return '';
        return this[0].tagName.toLowerCase();
    };
    // 获取select的文本
    $.fn.optionText = function () {
        if (this.length === 0) return '';
        var sel = this[0];
        if (sel.selectedIndex === -1) return '';
        return sel.options[sel.selectedIndex].text;
    };
    // 获取element属性的JSON值
    $.fn.attrJSON = function (attr) {
        return (this.attr(attr || 'rel') || '').parseAttrJSON();
    };

    $(document).ready(function () {
        Toolkit.data.clearError();
        $("a.delete").click(Toolkit.data.deleteData).attr("href", "#");
    });
})(jQuery);