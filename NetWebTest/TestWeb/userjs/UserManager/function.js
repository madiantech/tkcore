function newStandard() {
    var element = $(this);
    var defaultValue = [
        { "NameId": "Insert", "Name": "新建", "Position": "Global", "Icon": "icon-plus", "Info": "Insert", "Page": "List", "OperOrder": "10" },
        { "NameId": "Update", "Name": "修改", "Position": "Row", "Icon": "icon-edit", "Info": "Update", "Page": "All", "OperOrder": "20" },
        { "NameId": "Delete", "Name": "删除", "Position": "Row", "Info": "Delete,AjaxUrl", "ConfirmData": "确认删除吗？", "Page": "List", "OperOrder": "30" }
    ];
    setTimeout(function () {
        Toolkit.ui._createDataRow(element.parents('.panel').find('table'), 3, defaultValue);
    }, 100);
}

function newStandardTree() {
    var element = $(this);
    var defaultValue = [
        { "NameId": "Update", "Name": "修改节点", "Position": "Global", "Icon": "icon-edit", "Info": "Update,Dialog", "Page": "Detail", "OperOrder": "10" },
        { "NameId": "Delete", "Name": "删除节点", "Position": "Global", "Icon": "icon-remove", "Info": "Delete,AjaxUrl", "ConfirmData": "确认删除吗？", "Page": "Detail", "OperOrder": "20" },
        { "NameId": "Insert", "Name": "新建子节点", "Position": "Global", "Icon": "icon-plus", "Content": "~/CNewChild/~{CcSource}.c", "UseKey": "1", "UseMarco": "1", "Info": "Dialog", "Page": "Detail", "OperOrder": "30" }
    ];
    setTimeout(function () {
        Toolkit.ui._createDataRow(element.parents('.panel').find('table'), 3, defaultValue);
    }, 100);
}

function newEmpty() {
    var element = $(this);
    var defaultValue = [
        { "NameId": "_Empty", "Name": "空操作", "Content": "避免角色在配置权限时，出现需要选中该功能，但不要所有子操作的情况无法实现的BUG", "OperOrder": "-1" }
    ];
    setTimeout(function () {
        Toolkit.ui._createDataRow(element.parents('.panel').find('table'), 1, defaultValue);
    }, 100);
}

$(document).ready(function () {
    $("#newStd").click(newStandard);
    $("#newTreeStd").click(newStandardTree);
    $("#newEmpty").click(newEmpty);
});
