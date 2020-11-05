/*
* 有一个限制，不能在多个区块中共用同一个module
* */
(function($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.load.add('xml2json', {
        js: 'lib/jquery.xml2json.pack.js'
    });
    // ----------------------------
    //模块拖动处理
    Toolkit.ui.initDragdrop = function(elm) {
        var config = {
            opacity: .6,
            handle: '.sort-handle',
            placeholder: 'sort-helper',
            connectWith: '.tk-column',
            items: '.sort',
            scroll: true,
            tolerance: 'pointer',
            start: function(e, ui) {
                ui.placeholder[0].columnNode = ui.helper[0].parentNode;
                ui.placeholder.height(ui.helper.height() - 4);
                ui.placeholder.css('line-height', (ui.helper.height() - 4) + 'px');
                ui.placeholder.css('margin-top', '0');
                ui.placeholder.css('margin-right', ui.helper.css('margin-right'));
                ui.placeholder.css('margin-left', ui.helper.css('margin-left'));
                ui.placeholder.css('margin-bottom', ui.helper.css('margin-bottom'));
            },
            change: function(e, ui) {
                if (ui.item.hasClass('vonly') && ui.placeholder[0].columnNode !== ui.placeholder[0].parentNode) {
                    ui.placeholder.html('此模块不能拖动到此处').css({ 'color': '#f00', 'background-color': '#ffc' });
                } else {
                    ui.placeholder.html('').css({ 'color': '', 'background-color': '' });
                }
            },
            stop: function(e, ui) {
                if (ui.item.hasClass('vonly') && this !== ui.item[0].parentNode) {
                    $(this).sortable('cancel');
                    return false;
                }
                Toolkit.module.saveModules();
            }
        };
        elm.bindJqueryUI('sortable', config);
    };
    // 用户模块处理
    Toolkit.ui.UserModuleExecute = function(moduleJDOM, module) {
        var moduleDom = moduleJDOM[0];
        moduleDom.moduleData = module;
        moduleDom.moduleId = module.id;
        moduleDom.column = module.column;
        moduleDom.priority = module.priority;
        moduleDom.userPref = module.preference || {};
        moduleDom.status = {};
        if (module.status == 'new') {
            moduleDom.status.isnew = true;
        }
        moduleJDOM.bindToolkitUI('LoadModuleContent', module);
        moduleJDOM.bindToolkitUI('ModuleCollapseExecute');
        moduleJDOM.bindToolkitUI('ModuleSettingExecute', module);
    };
    // 加载模块内容（其中接口参数中需要传入moduleId，并在相应的接口内容中的Toolkit.module.define()
    Toolkit.ui.LoadModuleContent = function(moduleJDOM, module) {
        var moduleDom = moduleJDOM[0];
        var url = Toolkit.module.userData.portletUrl + "?Source=" + module.id;
        var pref = {}, temp = {};
        if (moduleDom.userPref && !$.isEmptyObject(moduleDom.userPref))
        {
        	$.each(moduleDom.userPref, function(key, value) {
            	temp[key] = value.toString();
        	});
            pref["_Preference"] = [temp];
        }
        $.ajax({ type: 'post', dataType: 'html', url: url, data: Toolkit.json.stringify(pref), success: function(content) {
            moduleJDOM.find('.mod-content').html(content);
			setTimeout(function() {
				moduleJDOM.trigger('ContentLoaded');
			}, 200);
        }});
    };
    // 模块折叠展开处理，删除处理
    Toolkit.ui.ModuleCollapseExecute = function(moduleJDOM) {
        moduleJDOM.find('.mod-header > .btn-collapse').bind('CollapseModule', function() {
            $(this).siblings('.btn-setting').trigger('CloseSetting');
            $(this).parent().siblings('.mod-content').hide();
            $(this).addClass('expand').attr('title', '展开内容');
        }).bind('ExpandModule', function() {
            $(this).parent().siblings('.mod-content').show();
            $(this).removeClass('expand').attr('title', '收起内容');
        }).click(function() {
            if ($(this).hasClass('expand')) {
                $(this).trigger('ExpandModule');
            } else {
                $(this).trigger('CollapseModule');
            }
        });
        moduleJDOM.find('.mod-header > .btn-close').click(function() {
            Toolkit.page.confirm('确认删除模块：“' + $(this).siblings('h3').text() + '”', function() {
                if (moduleJDOM[0].status.isnew) {
                    moduleJDOM.remove();
                } else {
                    moduleJDOM[0].status.deleted = true;
                    moduleJDOM.hide();
                    Toolkit.module.saveModules();
                }
            });
        });
    };
    // 模块设置处理，模块的设置参数项支持json和xml两个文本输出格式。
    Toolkit.ui.ModuleSettingExecute = function(moduleJDOM, module) {
        if (!module.gadgetPrefs || ($.isArray(module.gadgetPrefs)&& module.gadgetPrefs.length <=0) 
        	||$.isEmptyObject(module.gadgetPrefs)) {
            moduleJDOM.find('.mod-header > .btn-setting').remove();
            moduleJDOM.find('.mod-setting').remove();
            return;
        }
        moduleJDOM.find('.mod-header > .btn-setting').bind('OpenSetting', function() {
            $(this).parent().siblings('.mod-setting').show();
            $(this).html('关闭设置');
        }).bind('CloseSetting', function() {
            $(this).parent().siblings('.mod-setting').hide();
            $(this).html('设置');
        }).click(function() {
            if ($(this).text() == '设置') {
                $(this).trigger('OpenSetting');
            } else {
                $(this).trigger('CloseSetting');
            }
        });
        if (!$.isArray(module.gadgetPrefs) && typeof (module.gadgetPrefs) == 'string' && module.gadgetPrefs != '') {
            Toolkit.load('xml2json', function() {
                module.gadgetPrefs = $.xml2json('<xml>' + module.gadgetPrefs + '</xml>').pref || [];
                moduleJDOM.bindToolkitUI('CreateSettingView', module);
            });
        } else {
            moduleJDOM.bindToolkitUI('CreateSettingView', module);
        }
    };
    Toolkit.ui.CreateSettingView = function(moduleJDOM, module) {
        for (var i = 0; i < module.gadgetPrefs.length; i++) {
            var pref = module.gadgetPrefs[i];
            moduleJDOM.bindToolkitUI('CreateSettingElement', pref);
        }
        var updateElm = $('<a href="#" class="btn btn-mini"><em>保存</em></a>');
        updateElm.click(function() {
            Toolkit.module.savePreference(moduleJDOM, function() {
                moduleJDOM.bindToolkitUI('LoadModuleContent', module);
                moduleJDOM.find('.mod-header > .btn-setting').trigger('CloseSetting');
            });
            return false;
        });
        moduleJDOM.find('.mod-setting').append(updateElm);
    };
    Toolkit.ui.CreateSettingElement = function(moduleJDOM, pref) {
        var moduleDom = moduleJDOM[0];
        var settingJDOM = moduleJDOM.find('.mod-setting');
        var userPref = moduleDom.userPref[pref.name];
        pref.datatype = pref.datatype || 'String';
        pref.required = pref.required || 'false';
        switch (pref.datatype) {
            case 'String':
                pref.value = userPref || pref['default_value'] || '';
                settingJDOM.append('<p><span>' + pref['display_name'] + '：</span></p>');
                var txtElm = $('<input type="text style="width:60%;" name="{name}" required="{required}" value="{value}" />'.substitute(pref));
                txtElm.blur(function() {
                    moduleDom.userPref[this.name] = this.value;
                });
                settingJDOM.children('p:last').append(txtElm);
                moduleDom.userPref[pref.name] = pref.value;
                break;
            case 'Bool':
                pref.value = (typeof (userPref) != 'undefined') ? (userPref.toLowerCase() == "true") : (pref['default_value'] || false);
                settingJDOM.append('<p><label>' + pref['display_name'] + '</label></p>');
                var chkElm = $('<input type="checkbox" name="{name}" {} />'.substitute(pref));
                chkElm.attr('checked', pref.value);
                chkElm.click(function() {
                    moduleDom.userPref[this.name] = this.checked;
                });
                settingJDOM.children('p>label:last').prepend(chkElm);
                moduleDom.userPref[pref.name] = pref.value;
                break;
            case 'Enum':
                pref.value = userPref || pref['default_value'] || '';
                settingJDOM.append('<p><span>' + pref['display_name'] + '：</span></p>');
                var selElm = $('<select name="' + pref.name + '"></select>');
                var data = pref.EnumValue;
                for (var i = 0; i < data.length; i++) {
                    selElm.append('<option value="' + data[i].Value + '"' + (pref.value == data[i].Value ? ' selected' : '') + '>' + (data[i]['DisplayValue'] || data[i].Value) + '</option>');
                }
                selElm.change(function() {
                    moduleDom.userPref[this.name] = this.value;
                });
                settingJDOM.children('p:last').append(selElm);
                moduleDom.userPref[pref.name] = selElm.val();
                break;
            case 'Enums':
                var chk_arr = (userPref) ? userPref.split(',') : ((pref['default_value']) ? pref['default_value'].split('|') : []);
                settingJDOM.append('<p><span>' + pref['display_name'] + '：</span></p>');
                var data = pref.EnumValue;
                for (var i = 0; i < data.length; i++) {
                    settingJDOM.children('p:last').append('<label><input type="checkbox" name="' + pref.name + '" value="' + data[i].Value + '" />' + (data[i]['DisplayValue'] || data[i].Value) + '</label>');
                }
                settingJDOM.find('input[name=' + pref.name + ']').click(function() {
                    moduleDom.userPref[this.name] = Toolkit.data.getCheckedValue(settingJDOM.find('input[name=' + this.name + ']')).join(',');
                });
                moduleDom.userPref[pref.name] = Toolkit.data.getCheckedValue(settingJDOM.find('input[name=' + pref.name + ']')).join(',');
        }
    };
    // ----------------------------
    Toolkit.namespace('Toolkit.module');
    // ----------------------------
    Toolkit.module.data = {};
    Toolkit.module.data.layouts = {
        "101": { 'colnum': '1', 'width': ['99', '', '', ''], 'title': '一列' },
        "201": { 'colnum': '2', 'width': ['50', '49', '', ''], 'title': '两个对称列' },
        "202": { 'colnum': '2', 'width': ['33', '66', '', ''], 'title': '两列，左窄' },
        "203": { 'colnum': '2', 'width': ['66', '33', '', ''], 'title': '两列，右窄' },
        "301": { 'colnum': '3', 'width': ['33', '33', '33', ''], 'title': '三个对称列' },
        "302": { 'colnum': '3', 'width': ['25', '49', '25', ''], 'title': '三列，两边窄' },
        "303": { 'colnum': '3', 'width': ['25', '25', '49', ''], 'title': '三列，右宽' },
        "304": { 'colnum': '3', 'width': ['49', '25', '25', ''], 'title': '三列，左宽' }
    };
    Toolkit.module.userData = {};
    Toolkit.module.templates = {
        'column': '<div id="column_{id}" class="tk-column cw{width}"></div>',
        'module': '<div id="module_{id}" class="tk-module sort"><div class="mod-frame">' +
					'<div class="mod-header sort-handle"><b class="btn-collapse fl" title="收起"></b><b class="btn-close" title="删除模块"></b><a href="#" class="btn-setting">设置</a><h3>{title}</h3></div>' +
					'<div class="mod-setting dn"></div>' +
					'<div class="mod-content"><p class="ico-l-loading">正在加载……</p></div>' +
				  '</div></div>'
    };
    // ----------------------------
    Toolkit.module.define = function(moduleId, callback) {
        if (!callback) return;
        var moduleJDOM = $('#module_' + moduleId);
        moduleJDOM.bind('ContentLoaded', function() {
            callback(moduleJDOM);
            moduleJDOM.unbind('ContentLoaded');
        });
    };
    // ----------------------------
    //初始化生成页面
    Toolkit.module.init = function() {
        Toolkit.module.initColumns();
        Toolkit.module.initModules();
        Toolkit.module.initPageTool();
        $('.tk-column').bindToolkitUI('initDragdrop');
    };
    //生成column
    Toolkit.module.initColumns = function() {
        var layoutId = this.userData.layoutId;
        var column = this.data.layouts[layoutId];
        $('#pageContainer').attr('dataLayoutId', layoutId).html('');
        for (var i = 0; i < column.colnum; i++) {
            $('#pageContainer').append(this.templates.column.substitute({ id: (i + 1), width: column.width[i] }));
        }
    };
    //生成module
    Toolkit.module.initModules = function() {
        var modules = this.userData.regions;
        var column = this.data.layouts[this.userData.layoutId];
        for (var i = 0; i < modules.length; i++) {
            var module = modules[i];
            if (module.column > column.colnum) module.column = column.colnum;
            $('#column_' + module.column).append(this.templates.module.substitute(module));
            $('#module_' + module.id).bindToolkitUI('UserModuleExecute', module);
        }
    };
    Toolkit.module.initPageTool = function() {
        var column = this.data.layouts[this.userData.layoutId];
        $('#editLayout,#addModule').show();
        $('#editLayout').click(function() {
            Toolkit.page.dialog.show({ title: '修改布局', id: 'layouts', width: 300, height: 100 });
        });
        $('#addModule').click(function() {
            Toolkit.page.showLoading({ title: '添加模块', loadingText: '正在加载模块列表' });
            if ($('#gadgets li').size() > 0) {
				Toolkit.page.hideLoading();
                Toolkit.page.dialog.show({ title: '添加模块', id: 'gadgets', width: 300, height: 300 });
            }
            $.get(Toolkit.module.userData.portalUrl + "&Operation=GetModules", function(d) {
                Toolkit.load('xml2json', function() {
                	var temp = $.xml2json(d).Portlet|| [];
                	if(!$.isArray(temp))
                		temp = [temp];
                    Toolkit.module.data.gadgets = temp;
                    Toolkit.module.initGadgetsList();
					Toolkit.page.hideLoading();
                    Toolkit.page.dialog.show({ title: '添加模块', id: 'gadgets', width: 300, height: 300 });
                });
            }, "xml");
        });
        Toolkit.module.initLayoutsList();
    };
    Toolkit.module.initGadgetsList = function() {
        $('body').append('<div id="gadgets" class="dn"><p class="pt5 pl10">请选择您所想要的模块。</p><ul class="clearfix"></ul></div>');
        for (var i = 0; i < this.data.gadgets.length; i++) {
            $('#gadgets ul').append('<li class="clearfix" title="' + this.data.gadgets[i].Title + '" dataIndex="' + i + '">' + this.data.gadgets[i].Title + '</li>');
        }
        $('#gadgets li').click(function() {
            if ($(this).hasClass('on')) return;
            Toolkit.module.insertModule($(this).attr('dataIndex'));
            $(this).addClass('on');
        });
    };
    Toolkit.module.insertModule = function(idx) {
        var gadget = this.data.gadgets[idx];
        gadget.PreferenceValue = gadget.PreferenceValue || [];
        gadget.PreferenceField = gadget.PreferenceField || [];
        var preferences = {}, gadgetPrefs = [];
        $.each(gadget.PreferenceValue, function(i, item) {
            preferences[item.Name] = item.Value;
        });
        $.each(gadget.PreferenceField, function(i, item) {
            gadgetPrefs[i] = {
                "name": item.Name,
                "display_name": item.DisplayName,
                "required": item.Required,
                "datatype": item.DataType,
                "default_value": item.DefaultValue,
                "EnumValue":item.EnumValue
            };
        });
        var module = {
            status: 'new',
            id: gadget.Id,
            title: gadget.Title,
            column: $('.tk-column').size(),
            priority: 1,
            preference: preferences,
            gadgetPrefs: gadgetPrefs
        };
        $('#column_' + module.column).append(this.templates.module.substitute(module));
        $('#module_' + module.id).bindToolkitUI('UserModuleExecute', module);
        Toolkit.module.saveModules();
        Toolkit.util.dialog.close();
    };
    Toolkit.module.initLayoutsList = function() {
        $('body').append('<div id="layouts" class="dn"><p class="pt5 pl10">请选择您所想要的布局。</p><ul class="clearfix"></ul></div>');
        for (var item in this.data.layouts) {
            $('#layouts ul').append('<li class="clearfix" title="' + this.data.layouts[item].title + '" dataLayoutId="' + item + '"><div class="lpv lpv' + item + '"></div>' + this.data.layouts[item].title + '</li>');
        }
        $('#layouts li[dataLayoutId=' + this.userData.layoutId + ']').addClass('on');
        $('#layouts li').click(function() {
            Toolkit.module.changeColumn($(this).attr('dataLayoutId'));
            $(this).siblings('.on').removeClass('on');
            $(this).addClass('on');
            Toolkit.util.dialog.close();
        });
    };
    //改变分栏
    Toolkit.module.changeColumn = function(layoutId) {
        var oldcols = this.data.layouts[this.userData.layoutId].colnum;
        var column = this.data.layouts[layoutId];
        if (oldcols <= column.colnum) {
            for (i = 0; i < oldcols; i++) {
                $('#column_' + (i + 1)).attr('class', 'tk-column cw' + column.width[i]);
            };
            for (i = oldcols; i < column.colnum; i++) {
                $('#pageContainer').append(this.templates.column.substitute({ id: (i + 1), width: column.width[i] }));
            }
            if (oldcols < column.colnum) {
                $('.tk-column:gt(' + (oldcols - 1) + ')').bindToolkitUI('initDragdrop');
            }
            this.userData.layoutId = layoutId;
            Toolkit.module.saveLayout();
        } else {
            $('#column_' + (column.colnum)).append($('.tk-column:gt(' + (column.colnum - 1) + ') .tk-module'));
            $('.tk-column:gt(' + (column.colnum - 1) + ')').remove();
            for (i = 0; i < column.colnum; i++) {
                $('#column_' + (i + 1)).attr('class', 'tk-column cw' + column.width[i]);
            };
			Toolkit.util.overlayer.resize();
            this.userData.layoutId = layoutId;
            Toolkit.module.saveLayout();
            Toolkit.module.saveModules();
        }
    };
    // ----------------------------
    // 保存页面layout
    Toolkit.module.saveLayout = function() {
        this.saveData(this.userData.portalUrl
        , { "Operation": "SaveLayout", "LayoutId": this.userData.layoutId }, {});
    };
    // 保存模块信息（多个）
    Toolkit.module.saveModules = function() {
        var modules = [];
        $('.tk-column').each(function(i) {
            $(this).find('.tk-module').each(function(j) {
                this.column = i + 1;
                this.priority = j + 1;
            });
        });
        $('.tk-module').each(function() {
            if (this.status.isnew) {
                modules.push({
                    'action': 'insert',
                    'title': this.moduleData.title,
                    'moduleId': this.moduleId,
                    'column': this.column.toString(),
                    'priority': this.priority.toString()
                });
                delete this.status.isnew;
            } else if (this.status.deleted) {
                modules.push({
                    'action': 'delete',
                    'moduleId': this.moduleId
                });
                $(this).addClass('tk-module-deleted');
            } else {
                if (this.column != this.moduleData.column || this.priority != this.moduleData.priority) {
                    modules.push({
                        'action': 'update',
                        'moduleId': this.moduleId,
                        'column': this.column.toString(),
                        'priority': this.priority.toString()
                    });
                    this.moduleData.column = this.column;
                    this.moduleData.priority = this.priority;
                }
            }
        });
        this.saveData(this.userData.portalUrl, { "Operation": "SaveModules" }, { "_Regions": modules }
        , function() {
            $('.tk-module-deleted').remove();
        });
    };
    // 保存模块设置（单个）
    Toolkit.module.savePreference = function(moduleJDOM, callback) {
        var moduleDom = moduleJDOM[0];
        var userPref = {};
        $.each(moduleDom.userPref, function(key, value) {
            userPref[key] = value.toString();
        });
        this.saveData(this.userData.portalUrl, { "Operation": "SavePref", "ModuleId": moduleDom.moduleId }
        , { "_Preference": [userPref] }, callback);
    };
    // 统一的保存数据提交处理
    Toolkit.module.saveData = function(url, queryPart, param, callback) {
        var _path = url.getPathName();
        var _param = url.getQueryJson();
        $.extend(_param, queryPart);
        url = _path + '?' + $.param(_param);
        $.ajax({ type: 'post', url: url, data: Toolkit.json.stringify(param), success: function() {
            if (callback) callback();
        }
        });
    };
    // ----------------------------
    Toolkit.page.init = function() {
        Toolkit.module.init();
    };
})(jQuery);