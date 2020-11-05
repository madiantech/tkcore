// --------------------------------
// Toolkit.dataset
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------	
    Toolkit.namespace('Toolkit.dataset');
    // ----------------------------
    Toolkit.dataset.indexId = 0;
    Toolkit.dataset.getDataSetId = function () {
        this.indexId++;
        return 'DS' + this.indexId.padLeft(4, '0');
    };
    Toolkit.dataset.initForm = function (frm) {
        var dataSetId = this.getDataSetId();
        frm.attr('dataSet', dataSetId);
        var tables = (frm.attr('dataTables') || '').trim().split(',').removeEmpty();
        var fields = (frm.attr('dataFields') || '').trim().split(';').removeEmpty();
        this[dataSetId] = {};
        this[dataSetId].tables = tables;
        for (var i = 0; i < tables.length; i++) {
            var f = fields[i] || '';
            this.initTable(dataSetId, tables[i], fields[i].split(','));
        }
    };
    Toolkit.dataset.initTable = function (dataSetId, table, fields) {
        this[dataSetId][table] = {};
        this[dataSetId][table].fields = fields;
        this[dataSetId][table].elements = [];
        var tableElement = $('#' + table);
        if (tableElement.tagName() == 'table') {
            var that = this;
            tableElement.find('tbody.list tr').each(function () {
                that.addElement(dataSetId, $(this), table, fields);
            });
        } else {
            this.addElement(dataSetId, tableElement, table, fields);
        }
    };
    Toolkit.dataset.addElement = function (dataSetId, elm, table, fields) {
        var element = {};
        for (var i = 0; i < fields.length; i++) {
            var field = fields[i];
            var fieldElm = elm.find('[name=' + field + ']');
            if (fieldElm.size() == 0) fieldElm = elm.find('#' + field);
            element[field] = fieldElm;
        }
        this[dataSetId][table].elements.push(element);
    };
    Toolkit.dataset.removeElement = function (dataSetId, table, row) {
        this[dataSetId][table].elements.removeAt(row);
    };
    Toolkit.dataset.emptyElements = function (dataSetId, table) {
        this[dataSetId][table].elements = [];
    };
    Toolkit.dataset.getData = function (dataSetId) {
        Toolkit.stat.addFuncStat('Toolit.dataset.getData-' + dataSetId);
        var data = {};
        for (var i = 0; i < this[dataSetId].tables.length; i++) {
            var table = this[dataSetId].tables[i];
            var fields = this[dataSetId][table].fields;
            var temp = [];
            for (var j = 0; j < this[dataSetId][table].elements.length; j++) {
                var element = this[dataSetId][table].elements[j];
                var item = {};
                for (var k = 0; k < fields.length; k++) {
                    var field = fields[k];
                    var elm = element[field];
                    Toolkit.data.CheckInputValid(elm);
                    if (elm.data('valid') && elm.data('error')) {
                        Toolkit.page.showError(elm.closest('.tk-control').parent().attr('title'), function () {
                            if (elm.is(':visible')) {
                                elm.focus();
                            }
                        });
                        return false;
                    }
                    item[field] = Toolkit.data.getElementValue(elm);
                }
                temp.push(item);
            }
            if (temp.length > 0) data[table] = temp;
        }
        Toolkit.stat.addFuncStat('Toolit.dataset.getData-' + dataSetId);
        return data;
    };
    // ----------------------------
})(jQuery);