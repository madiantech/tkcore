/*
搜索控件：
获取控件值：
设置控件值：
*/
(function($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.data');

    Toolkit.data._showError = function(elm, msg) {
        var div = elm.parents("div.form-group");
        div.addClass("has-error");
        var label = div.find("span.tk-control>label.help-block");
        label.text(msg).show();
    };

    Toolkit.data._clearError = function(frm) {
        frm.find("div.form-group").removeClass("has-error");
        frm.find("span.tk-control>label.help-block").text("").hide();
    };

    Toolkit.data._getCustomCtrl = function(field, ctrl, result) {
        var func = Toolkit.data.getFunction(ctrl.attr("data-getElement"));
        if (func !== null) {
            var customCtrl = func(field);
            if (customCtrl !== null)
                result.value.Control = customCtrl;
        }
        result.value["GetValue"] = ctrl.attr("data-getValue");
        result.value["SetValue"] = ctrl.attr("data-setValue");
    };

    Toolkit.data._getElementById = function(field) {
        var ctrl = $("#" + field.Name);
        var result = { "Control": ctrl };
        switch (field.Type) {
            case "RadioGroup":
                var checkedCtrl = ctrl.find("input[name=" + field.Name + "]:checked");
                result["Checked"] = checkedCtrl;
                break;
            case "EasySearch":
                var hdCtrl = $("#hd" + field.Name);
                result["HiddenControl"] = hdCtrl;
                break;
            case "MultipleEasySearch":
                var dataSpans = ctrl.parent().prev().find("span.multi-item");
                result["DataControl"] = dataSpans;
                break;
            case "HTML":
                var editor = ctrl.data("editor");
                if (editor)
                    editor.sync();
                break;
            case "Custom":
                var resultRef = { value: result };
                Toolkit.data._getCustomCtrl(field, ctrl, resultRef);
                result = resultRef.value;
                break;
            default:
                break;
        }
        return result;
    };

    Toolkit.data._getElementByName = function(rowDiv, field) {
        var ctrl;
        switch (field.Type) {
            case "Text":
            case "Hidden":
            case "Date":
            case "DateTime":
            case "Time":
            case "CheckBox":
            case "CheckBoxList":
            case "Password":
                ctrl = rowDiv.find("input[name=" + field.Name + "]");
                return { "Control": ctrl };
            case "Combo":
                ctrl = rowDiv.find("select[name=" + field.Name + "]");
                return { "Control": ctrl };
            case "TextArea":
            case "HTML":
                ctrl = rowDiv.find("textarea[name=" + field.Name + "]");
                if (field.Type === "HTML") {
                    var editor = ctrl.data("editor");
                    if (editor)
                        editor.sync();
                }
                return { "Control": ctrl };
            case "Detail":
            case "Upload":
                ctrl = rowDiv.find("div[name=" + field.Name + "]");
                return { "Control": ctrl };
            case "EasySearch":
                ctrl = rowDiv.find("input[name=" + field.Name + "]");
                var hdCtrl = rowDiv.find("input[name=hd" + field.Name + "]");
                return { "Control": ctrl, "HiddenControl": hdCtrl };
            case "MultipleEasySearch":
                ctrl = rowDiv.find("div[name=" + field.Name + "]");
                var dataSpans = ctrl.parent().prev().find("span.multi-item");
                return { "Control": ctrl, "DataControl": dataSpans };
            case "RadioGroup":
                ctrl = rowDiv.find("div[name=" + field.Name + "]");
                var checkedCtrl = ctrl.find("input[name=" + field.Name + "]:checked");
                return { "Control": ctrl, "Checked": checkedCtrl };
            case "Custom":
                ctrl = row.field("[name=" + field.Name + "]");
                var result = { "Control": ctrl };
                var resultRef = { value: result };
                Toolkit.data._getCustomCtrl(field, ctrl, resultRef);
                result = resultRef.value;
                return result;
            default:
                break;
        }
    };

    Toolkit.data._getElementValue = function(elm, type, checkError) {
        var value;
        switch (type) {
            case "Detail":
                return elm.Control.attr("data-value");
            case "EasySearch":
                value = elm.Control.val();
                var esResult = { "Hidden": elm.HiddenControl.val(), "Text": value };
                if (checkError)
                    Toolkit.data.checkInputValid(elm.Control, type, value);
                return esResult;
            case "MultipleEasySearch":
                var spanData = elm.DataControl;
                var dataId = [];
                spanData.each(function() {
                    value = '"' + $(this).attr("data-id") + '"';
                    dataId.push(value);
                });
                return dataId.join(",");
            case "RadioGroup":
                return elm.Checked.val();
            case "CheckBox":
                if (elm.Control.prop("checked"))
                    return elm.Control.val();
                else
                    return elm.Control.attr("data-uncheck-value");
            case "CheckBoxList":
                var checkedList = elm.Control.find("input:checked");
                var checkIds = [];
                checkedList.each(function() {
                    value = '"' + $(this).val() + '"';
                    checkIds.push(value);
                });
                return checkIds.join(",");
            case "Upload":
                var ctrl = elm.Control;
                var uploadData = ctrl.data("upload");
                var fields = {
                    "ServerPath": ctrl.attr("data-serverPath"),
                    "FileSize": ctrl.attr("data-fileSize"),
                    "ContentType": ctrl.attr("data-contentType")
                };
                if (checkError)
                    Toolkit.data.checkInputValid(ctrl, type, uploadData == undefined ? "" : uploadData.FileName);
                return { "Fields": fields, "Data": uploadData };
            case "Custom":
                var func = Toolkit.data.getFunction(elm.GetValue);
                if (func !== null)
                    return func(elm);
                else
                    return elm.Control.val();
            default:
                value = elm.Control.val();
                if (checkError)
                    Toolkit.data.checkInputValid(elm.Control, type, value);
                return value;
        }
    };

    Toolkit.data._setElementValue = function(data, field, value, elm) {
        switch (field.Type) {
            case "EasySearch":
                data[field.Name] = value.Hidden;
                data["hd" + field.Name] = value.Text;
                break;
            case "Upload":
                var uploadData = value.Data;
                var fields = value.Fields;
                data[field.Name] = uploadData == undefined ? "" : uploadData.FileName;
                data[fields.ServerPath] = uploadData == undefined ? "" : uploadData.ServerPath;
                data[fields.FileSize] = uploadData == undefined ? "" : uploadData.FileSize;
                data[fields.ContentType] = uploadData == undefined ? "" : uploadData.ContentType;
                break;
            case "Custom":
                var func = Toolkit.data.getFunction(elm.SetValue);
                if (func !== null)
                    func(data, field, value);
                else
                    data[field.Name] = value;
                break;
            default:
                data[field.Name] = value;
                break;
        }
    };

    Toolkit.data._getCtrl = function(ctrl, refName, refType) {
        var rowDiv = ctrl.closest(".table-row");
        return Toolkit.data._getElementByName(rowDiv, { "Name": refName, "Type": refType });
    };

    Toolkit.data._getForm = function(frmId, currentObj, defaultName) {
        var formId = defaultName;
        if (typeof(frmId) == "string" && frmId != null)
            formId = frmId;
        else if (typeof(frmId) == "object") {
            var dataForm = currentObj.attr("data-form");
            if (dataForm)
                formId = dataForm;
        }
        var frm = $("#" + formId);
        return frm;
    };

    Toolkit.data.getFunction = function(funcName) {
        if (funcName !== undefined && funcName !== "") {
            var funcArr = funcName.split(".");
            var resp;
            if (funcArr.length === 1)
                resp = window[funcName];
            else {
                resp = window[funcArr[0]];
                for (var i = 1; i < funcArr.length; i++)
                    resp = resp[funcArr[i]];
            }
            if ($.isFunction(resp))
                return resp;
        }
        return null;
    };

    Toolkit.data.getRowData = function(table, checkError, rowDiv) {
        var data = {};
        for (var i = 0; i < table.Fields.length; i++) {
            var field = table.Fields[i];
            var elm;
            if (table.SearchMethod === "Id")
                elm = Toolkit.data._getElementById(field);
            else
                elm = Toolkit.data._getElementByName(rowDiv, field);
            var value = Toolkit.data._getElementValue(elm, field.Type, checkError);
            Toolkit.data._setElementValue(data, field, value, elm);
        }
        return data;
    };

    Toolkit.data.getTableData = function(table, checkError) {
        if (table.JsonType === "Object")
            return Toolkit.data.getRowData(table, checkError);
        else {
            var arr = [];
            if (table.SearchMethod === "Id") {
                var data = Toolkit.data.getRowData(table, checkError);
                arr.push(data);
            } else {
                var tableDiv = $("#" + table.Table);
                var divs;
                if (tableDiv.hasClass("table-row"))
                    divs = tableDiv;
                else
                    divs = tableDiv.find(".table-row");
                divs.each(function() {
                    var rowData = Toolkit.data.getRowData(table, checkError, $(this));
                    arr.push(rowData);
                });
            }
            return arr;
        }
    };

    Toolkit.data.getJson = function(frm) {
        var postObj = frm.attr("data-post");
        if (!postObj)
            return undefined;

        var postTables = postObj.parseJSON().Tables;
        var postData = {};
        for (var i = 0; i < postTables.length; i++) {
            var table = postTables[i];
            var tableData = Toolkit.data.getTableData(table, true);
            postData[table.Table] = tableData;
        }
        return postData;
    };

    Toolkit.data.setTableProp = function() {
        var forms = $("form.tk-dataform");
        forms.each(function() {
            var frm = $(this);
            var postObj = frm.attr("data-post");
            if (!postObj)
                return;

            var postTables = postObj.parseJSON().Tables;
            for (var i = 0; i < postTables.length; i++) {
                var table = postTables[i];
                var tableDiv = $("#" + table.Table);
                tableDiv.attr("data-searchMethod", table.SearchMethod);
            }
        });
    }

    Toolkit.data.getPostUrl = function(frm) {
        var url = frm.attr("action");
        if (!url || url === "." || url === "")
            url = window.location.href;
        return url;
    };

    Toolkit.data.post = function(frmId) {
        var currentObj = $(this);
        var frm = Toolkit.data._getForm(frmId, currentObj, "PostForm");
        if (!frm)
            return;
        Toolkit.data._clearError(frm);

        var postData = Toolkit.data.getJson(frm);
        if (frm.find("div.form-group.has-error").length > 0)
            return;
        Toolkit.ui.enableButton(false);
        var url = Toolkit.data.getPostUrl(frm);
        var response = currentObj.attr("data-response");
        var func = response ? window[response] : _successCommit;
        var postFunc = function(req) {
            Toolkit.ui.enableButton(true);
            func(req);
        };
        if (func)
            if (postData) {
                //alert(Toolkit.json.stringify(postData));
                //Toolkit.ui.enableButton(true);
                $.ajax({
                    type: 'post',
                    dataType: 'text',
                    url: url,
                    data: Toolkit.json.stringify(postData),
                    success: postFunc,
                    error: function() {
                        Toolkit.ui.enableButton(true);
                        Toolkit.page.showError('数据提交失败！请稍候重试。');
                    }
                });
            }
        return false;
    };

    Toolkit.data._getErrorCtrl = function(error) {
        var table = $("#" + error.TableName);
        var method = table.data("searchmethod");
        if (method === "Id")
            return $("#" + error.NickName);
        else {
            var selector = "tr.table-row:eq(" + error.Position + ")";
            var rowDiv = table.find(selector);
            selector = "[name='" + error.NickName + "']";
            return rowDiv.find(selector);
        }
    };

    Toolkit.data.updateInputsValid = function(errors) {
        for (var i = 0; i < errors.length; i++) {
            var error = errors[i];
            //var field = error.NickName;
            var msg = error.Message;
            var input = Toolkit.data._getErrorCtrl(error);
            Toolkit.data._showError(input, msg);
            if (i === 0) {
                if (input.is(':visible'))
                    input.focus();
            }
        }
    };

    Toolkit.data._defaultProcess = function(message, newWindow) {
        var retValue;
        if (message === undefined || message === "") {} else if (message === "WeixinClose") {
            Toolkit.page.weixinClose();
        } else if (message === "Refresh") {
            document.location.reload();
        } else if (message === "ListRefresh") {
            retValue = $("body").triggerHandler("ListRefresh");
            if (!retValue)
                document.location.reload();
        } else if (message === "Back") {
            //history.back();
            window.location.href = document.referrer;
        } else if (message === "CloseDialog") {
            Toolkit.page.closeDialog();
        } else if (message === "CloseDialogAndRefresh") {
            if (Toolkit.page.isDlgWin) {
                var parentBody = window.parent.$("body");
                retValue = parentBody.triggerHandler("ListRefresh");
                if (!retValue)
                    parent.document.location.reload();
            } else {
                retValue = $("body").triggerHandler("ListRefresh");
                if (!retValue)
                    document.location.reload();
            }
            Toolkit.page.closeDialog();
        } else {
            if (newWindow)
                window.open(message)
            else
                document.location.href = message;
        }
    };

    _successCommit = function(req) {
        var data = req.parseJSON();
        if (data && data.Result) {
            var result = data.Result.Result;
            if (result === "Success") {
                var message = data.Result.Message;
                var needProcess = true;
                if (data.AlertMessage && data.AlertMessage != "") {
                    if (Toolkit.page.isDlgWin)
                        alert(data.AlertMessage);
                    else {
                        needProcess = false;
                        Toolkit.page.alert(data.AlertMessage, function() { Toolkit.data._defaultProcess(message, data.NewWindow); });
                    }
                }
                if (needProcess)
                    Toolkit.data._defaultProcess(message, data.NewWindow);
            } else if (result === "Error") {
                if (data.FieldInfo) {
                    Toolkit.data.updateInputsValid(data.FieldInfo);
                } else {
                    Toolkit.page.showError(data.Result.Message);
                }
            } else if (result === "Fail") {
                Toolkit.page.showError("数据提交失败！请在新弹出的窗口查看错误信息");
                window.open(data.Result.Message);
            }
        } else {
            Toolkit.page.showError('数据提交失败！请稍候重试。');
        }
    };

    Toolkit.data.getSelfUrl = function(elm) {
        var url = document.location.href;
        var uri = new URI(url);
        var params = uri.search(true);
        delete params["RetURL"];
        uri = uri.search(params);
        return uri.toString();
    };

    Toolkit.data.checkInputValid = function(elm, type, value) {
        switch (type) {
            case "Text":
            case "TextArea":
            case "Date":
            case "DateTime":
            case "Time":
            case "Password":
            case "Upload":
            case "EasySearch":
            case "HTML":
                if (elm.attr("required")) {
                    if (value === undefined || value == "") {
                        var msg = "请输入" + elm.attr("data-title");
                        Toolkit.data._showError(elm, msg);
                    }
                }
                break;
        }
    };

    $.fn.showInputValidStatus = function(error) {
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
                this.change(function() {
                    $(this).showInputValidStatus();
                });
                this.data('validBinded', true);
            }
        }
    };

    $(document).ready(function() {
        Toolkit.data._clearError($("body"));
    });
})(jQuery);