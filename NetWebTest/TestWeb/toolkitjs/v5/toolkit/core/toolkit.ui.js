﻿﻿// --------------------------------
// Toolkit.ui
// --------------------------------
(function($) {
	if (!$ || !window.Toolkit) return;
	// ----------------------------
	Toolkit.namespace('Toolkit.ui');
	// ----------------------------
	// IE6hack: 增加hover样式
	Toolkit.ui.ElementHover = function(elm) {
		elm.bind('mouseover', function() { $(this).addClass('hover'); });
		elm.bind('mouseout', function() { $(this).removeClass('hover'); });
	};
	// ----------------------------
	Toolkit.ui.ToggleTabElement = function(elm, options) {
		elm.each(function(i) {
			$(this).bind('click', i, function(e) {
				if ($(this).hasClass('on')) return;
				$(this).siblings('.on').removeClass('on');
				$(this).addClass('on');
				$(options.child+'.on').removeClass('on');
				$(options.child+':eq('+e.data+')').addClass('on');
				if (options.onToggle) {
					options.onToggle($(this), $(options.child+':eq('+e.data+')'));
				}
			});
		});
	};
	// IE6hack: 显示/隐藏没被遮盖对象 select/object/iframe
	Toolkit.ui.ShowOverElements = function(elm, element) {
		$(element).each(function() {
			if (this._hide) {
				this.style.visibility = 'visible';
				this._hide = false;
			}
		});
	};
	Toolkit.ui.HideOverElements = function(elm, element) {
		var ox = elm.offset().left;
		var oy = elm.offset().top;
		var ow = elm.outerWidth() || elm.width();
		var oh = elm.outerHeight() || elm.height();
		$(element).each(function() {
			var pos = $(this).offset();
			if (!(ox>(pos.left+$(this).width()) || pos.left>(ox+ow) || pos.top>(oy+oh) || oy>(pos.top+$(this).height()))) {
				this.style.visibility = 'hidden';
				this._hide = true;
			}
		});
	};
	// ----------------------------
	// 定时关闭(隐藏)div图层
	Toolkit.ui.HideByTimer = function(elm, param) {
		elm.each(function() {
			var element = $(this);
			setTimeout(function() {
				if (param.animate == 'slide') {
					element.slideUp();
				} else {
					element.hide();
				}
			}, param.timer*1000);
		});
	};
	// ----------------------------
	// 页面Control相关处理
	// ----------------------------
	// 绑定日历方法
	Toolkit.ui.DatePicker = function(elm) {
		Toolkit.load('lhgcalendar', function() {
			elm.each(function() {
				var param = {
					format: ($(this).attr('dataControl')=='Date')?'yyyy-MM-dd':'yyyy-MM-dd HH:mm'
				};
				var input = $(this).find('input[type=text]');
				if (input.attr('dataMinDate')) param.minDate = input.attr('dataMinDate');
				if (input.attr('dataMaxDate')) param.maxDate = input.attr('dataMaxDate');
				input.calendar(param);
			});
		});
	};
	// ----------------------------
	// 绑定EasySearch处理
	Toolkit.ui.EasySearch = function(elm) {
		Toolkit.load('suggest', function() {
			elm.each(function() {
				var element = $(this);
				var inputElement = element.find('input[type=text]');
				var fieldStr = element.attr('dataRefField') || '';
				var refFields = null;
				if (fieldStr!='') {
					var fields = fieldStr.split(';');
					refFields = {};
					for (var i=0; i<fields.length; i++) {
						if (fields[i].indexOf(',')<1) continue;
						var field_name = fields[i].split(',')[0];
						var element_name = fields[i].split(',')[1];
						var refElement = Toolkit.data.getRefElement(element, element_name);
						refElement.bind('change', function() {
							inputElement.bindToolkitUI('EasySearchEmpty');
						});
						refFields[field_name] = refElement;
					}
				}
				inputElement.bindToolkitUI('suggest', {
					source: element.attr('dataUrl'),
					refFields: refFields,
					onBeforeSuggest: function() {
						inputElement.trigger('BeforeEasySearch');
					},
					onSelect: function(input, id, text) {
						input.bindToolkitUI('EasySearchChecked', {id:id, text:text});
					}
				});
				inputElement.bindToolkitUI('EasySearchChecked');
				element.find('span.ico').click(function() {
					inputElement.trigger('BeforeEasySearch');
					inputElement.bindToolkitUI('EasySearchPopWin', {
						title: inputElement.attr('dataTitle') || '',
						source: element.attr('dataPage') || '',
						exId: inputElement.siblings('[name=hd'+inputElement.attr('name')+']').val() || '',
						refFields: refFields
					});
				});
			});
		});
	};
	Toolkit.ui.EasySearchEmpty = function(input) {
		var hdinput = input.siblings('[name=hd'+input.attr('name')+']');
		input.siblings('.checked').remove();
		hdinput.val('').trigger('change');
		input.val('').show();
	};
	// EasySearch选中后赋值
	Toolkit.ui.EasySearchChecked = function(input, data) {
		var hdinput = input.siblings('[name=hd'+input.attr('name')+']');
		if (data) {
			hdinput.val(data.id).trigger('change');
			input.val(data.text).blur();
			input.showInputValidStatus();
		}
		if (input.val()=='') return;
		input.hide();
		input.before('<span class="checked rad3">'+ input.val() +' <del title="删除">×</del></span>');
		input.siblings('.checked').find('del').click(function() {							
			$(this).parent().remove();
			hdinput.val('').trigger('change');
			input.val('').show();
		});
		input.trigger('AfterItemSelected', data);
	};
	// 打开EasySearch弹窗
	Toolkit.ui.EasySearchPopWin = function(input, data) {
		Toolkit.page.currentEasySearchElement = input;
		var url = data.source.substitute(data);
		if (data.refFields)	{
			var refdata = {};
			refdata['REF'] = [];
			for (var item in data.refFields) {
				var itemValue = Toolkit.data.getElementValue(data.refFields[item]);
				refdata['REF'].push({Field:item,RefValue:itemValue});
			}
			url += '&RefValue='+encodeURIComponent(Toolkit.json.stringify(refdata))+'&Format=Json';
		}
		Toolkit.page.dialog.pop({
			title: '选择'+ data.title,
			url: url,
			width: 250,
			height: 242
		});
	};
	// 弹窗中返回EasySearch所选数据
	Toolkit.ui.setEasySearchData = function(data) {
		var input = Toolkit.page.currentEasySearchElement;
		var controlType = input.parents('.tk-control').attr('dataControl');
		if (controlType == 'MultiEasySearch') {
			input.bindToolkitUI('MultiEasySearchChecked', data);
		} else {
			input.siblings('span.checked').remove();
			input.bindToolkitUI('EasySearchChecked', data);
		}
	};
	// ----------------------------
	// 绑定MultiEasySearch处理
	Toolkit.ui.MultiEasySearch = function(elm) {
		Toolkit.load('suggest', function() {
			elm.each(function() {
				var element = $(this);
				var inputElement = element.find('input[type=text]');
				var fieldStr = element.attr('dataRefField') || '';
				var refFields = null;
				if (fieldStr!='') {
					var fields = fieldStr.split(';');
					refFields = {};
					for (var i=0; i<fields.length; i++) {
						if (fields[i].indexOf(',')<1) continue;
						var field_name = fields[i].split(',')[0];
						var element_name = fields[i].split(',')[1];
						var refElement = Toolkit.data.getRefElement(element, element_name);
						refElement.bind('change', function() {
							inputElement.bindToolkitUI('MultiEasySearchEmpty');
						});
						refFields[field_name] = refElement;
					}
				}
				inputElement.bindToolkitUI('suggest', {
					source: element.attr('dataUrl'),
					refFields: refFields,
					onBeforeSuggest: function() {
						inputElement.trigger('BeforeEasySearch');
					},
					onSelect: function(input, id, text) {
						input.bindToolkitUI('MultiEasySearchChecked', {id:id, text:text});
					}
				});
				inputElement.bind('keypress', function(e) {
					try {
						if (e.shiftKey) return false;
						var code=e.which||e.keyCode||0;
						if (this.value=='' && code==8) {
							if (!inputElement.prev().hasClass('hidden')) {
								inputElement.prev().find('del').click();
							}
						}
					} catch(err) {}
				});
				inputElement.bindToolkitUI('MultiEasySearchChecked');
				element.find('span.mr20').click(function() {
					inputElement.focus();
				});
				element.find('span.ico').click(function() {
					inputElement.trigger('BeforeEasySearch');
					inputElement.bindToolkitUI('EasySearchPopWin', {
						title: inputElement.attr('dataTitle') || '',
						source: element.attr('dataPage') || '',						
						refFields: refFields
					});
				});
			});
		});
	};
	// MultiEasySearch选中后赋值
	Toolkit.ui.MultiEasySearchChecked = function(input, data) {
		if (data) {
			if (input.parent().find('input[value='+ data.id +']').size()>0) return;
			var elm = input.siblings('span.checked:first').clone();
			elm.removeClass('hidden');
			elm.find('input[name='+ input.attr('retId') +']').val(data.id);
			elm.find('input[name='+ input.attr('retText') +']').val(data.text);
			elm.find('em').html(data.text);
			input.before(elm);
			input.siblings('span.hidden').remove();
			input.bindToolkitUI('MultiEasySearchRemove');
			input.bindToolkitUI('MultiEasySearchResize');
			input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
			input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
			input.trigger('AfterItemSelected', data);
		} else {
			input.bindToolkitUI('MultiEasySearchRemove', 'all');
			input.bindToolkitUI('MultiEasySearchResize');
		}
	};
	Toolkit.ui.MultiEasySearchEmpty = function(input) {
		input.siblings('span.checked:gt(0)').remove();
		input.siblings('span.checked').addClass('hidden');
		input.siblings('span.checked').find('input').val('');
		input.siblings('span.checked').find('em').html('');
		input.bindToolkitUI('MultiEasySearchResize');
		input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
		input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
	};
	Toolkit.ui.MultiEasySearchRemove = function(input, type) {
		var element = (type)?input.siblings('span.checked'):input.prev();
		element.find('del').click(function() {
			if (input.siblings('span.checked').size()>1) {
				$(this).parent().remove();
			} else {
				$(this).parent().addClass('hidden');
				$(this).siblings('input').val('');
				$(this).siblings('em').html('');
			}
			input.bindToolkitUI('MultiEasySearchResize');
			input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retId'));
			input.parents('dd,td').bindToolkitData('UpdateDataSetRowElement', input.attr('retText'));
		});
	};
	Toolkit.ui.MultiEasySearchResize = function(input) {
		var w = 0;
		input.siblings().each(function() {
			w += $(this).outerWidth() + 2;
		});
		input.css('width', (input.parent().width() - w - 10) +'px').val('');
	};
	// ----------------------------
	// 绑定HTML编辑器
	Toolkit.ui.HtmlEditor = function(elm) {
		Toolkit.load('xheditor', function() {
			elm.each(function(i) {
				var container = $(this);
				var textarea = container.find('textarea');
				if (!textarea.attr('id')) {
					textarea.attr('id', textarea.attr('name')+'_XH_'+i);
				}
				var editor = textarea.xheditor({tools:'simple'});
				if ($(this).attr('dataControl')=='DetailHTML') {
					if($.browser.msie) {
						editor.doc.body.contentEditable = 'false';
					} else {
						editor.doc.designMode = 'Off';
					}
					container.find('table.xheLayout tbody tr:eq(0)').remove();
					container.find('table.xheLayout').css({width:'100%', height:'auto'});
					container.find('td.xheIframeArea').css('height', 'auto');
					container.find('table.xheLayout iframe').css('height', $(editor.doc.body).height());
				} else {
					container.find('table.xheLayout').css('width', '100%');
				}
			});
		});
	};

    Toolkit.ui._setElementValues = function(retElm, values)
    {
        values = values || {
                    'image':'', 
                    'path':'',
                    'type':'',
                    'size':''
                };
        retElm.image.val(values['image']).showInputValidStatus(false);
        retElm.path.val(values['path']).showInputValidStatus(false);
        retElm.type.val(values['type']).showInputValidStatus(false);
        retElm.size.val(values['size']).showInputValidStatus(false);
    };
	// ----------------------------
	// 绑定上传处理
	Toolkit.ui.UploadFile = function(elm) {
	    Toolkit.load('uploadify', function() {
	        elm.each(function(i) {
	            var element = $(this);
	            element.append('<span class="upload-frame"><span class="uploadplaceholder"></span><span class="upload-result"></span></span>');
				element.find('.uploadplaceholder').append(element.find('input.upload-content'));
	            element.find('.uploadplaceholder').bindToolkitUI('Uploadify', {
	                uploadUrl: element.attr('dataUploadUrl'),
	                dataXml: element.attr('dataXml'),
                    xmlVersion: element.attr('xmlVersion')|| '',
	                fieldName: element.attr('fieldName'),
	                isView: (element.attr('isView') == 'true') || false,
	                fileExt: element.attr('fileExt') || '',
                    filePath: element.attr('filePath') || '',
	                retElm: {
	                    image: element.find('input.upload-fileName'),
	                    path: element.find('input.upload-path'),
	                    type: element.find('input.upload-type'),
	                    size: element.find('input.upload-size')
	                }
	            });
	        });
	    });
	};
	Toolkit.ui.UploadIndex = 0;
	Toolkit.ui.Uploadify = function(elm, param) {
	    var fileExt = [], temp = param.fileExt.split('.') || [];
	    $.each(temp, function(name, value) {
	        if (value) fileExt.push('*.' + value);
	    });
	    fileExt = fileExt.join(';');
	    var _showDel = function(fileObj) {
	        var htmlstr = '<a href="{url}" target="_blank" class="{filetype}" title="{filename}"><font>{title}</font>{fileExtra}</a><a href="#" class="close">删除</a>';
	        var data = { url: fileObj.filePath, filetype: fileObj.type, filename: fileObj.name, fileExtra: '' };
			data.title = data.filename.sliceBefore('.').subByte(8,'…');
	        if (param.isView) {
	            data.filetype = 'picture';
	            data.fileExtra = '<img src="' + fileObj.filePath + '" />';
	        }
	        elm.find('.uploadifyQueue').html('');
	        elm.siblings('.upload-result').html(htmlstr.substitute(data)).css('display', 'block');
	        elm.siblings('.upload-result').find('a.close').click(function() {
//	            param.retElm.image.val('');
//	            param.retElm.path.val('');
//	            param.retElm.type.val('');
//	            param.retElm.size.val('');
                Toolkit.ui._setElementValues(param.retElm);
	            elm.show();
	            elm.siblings('.upload-result').html('').hide();
	            return false;
	        });
	    };
		Toolkit.ui.UploadIndex ++;
		elm.find("input.upload-content").attr('id', 'uploadify_'+ Toolkit.ui.UploadIndex);
	    elm.find("input.upload-content").uploadify({
	        'uploader': Toolkit.loader.serviceBase + Toolkit.loader.serviceLibs.uploadify.swf,
	        'buttonImg': Toolkit.loader.serviceBase + Toolkit.loader.serviceLibs.uploadify.uploadBtn,
	        'cancelImg': Toolkit.loader.serviceBase + Toolkit.loader.serviceLibs.uploadify.cancelBtn,
	        'script': param.uploadUrl,
	        //'fileDataName': 'FileData',
	        //'multi': false,
	        'folder': 'Upload',
	        'fileExt': fileExt,
	        'fileDesc': fileExt, //没有天理，fileExt需要fileDesc才起效
	        'scriptData': { DataXml: param.dataXml, FieldName: param.fieldName, Version: param.xmlVersion },
	        'auto': true,
	        'width': 70,
	        'height': 20,
	        'onSelect': function() {
				elm.siblings('.upload-result').html('<span class="ico-l-loading">正在上传……</span>').css('display','block');
	        },
	        'onComplete': function(event, queueId, fileObj, response) {
	            response = response.parseJSON();
	            if (response.Error) {
	                Toolkit.page.showError(response.Error[0].Error);
					elm.show();
					elm.siblings('.upload-result').html('').hide();
	                return false;
				}
	            response = response.Upload[0];
                Toolkit.ui._setElementValues(param.retElm, 
                {
                    'image':response.FileName, 
                    'path':response.ServerPath,
                    'type':response.FileType,
                    'size':response.FileSize
                });
//	            param.retElm.image.val(response.FileName);
//	            param.retElm.path.val(response.ServerPath);
//	            param.retElm.type.val(response.FileType);
//	            param.retElm.size.val(response.FileSize);
	            _showDel({
	                name: response.FileName,
	                filePath: response.ServerPath,
	                type: response.FileType,
	                size: response.FileSize
	            });
	        },
	        'onError': function(event, queueId, fileObj, errorObj) {
	            Toolkit.page.showError('上传图片错误:' + errorObj.type + '<br>' + errorObj.info);
				elm.show();
				elm.siblings('.upload-result').html('').hide();
	            return false;
	        }
	    });
	    if (param.retElm.path.val()) {
	        _showDel({
	            name: param.retElm.image.val(),
	            filePath: param.filePath,
	            type: param.retElm.type.val(),
	            size: param.retElm.size.val()
	        });
	    }
	};
	// ----------------------------
	// 页面容器相关的处理
	// ----------------------------
	Toolkit.ui.DataSearchExecute = function(elm) {
		elm.bindToolkitUI('DataFormElementExecute', 'detail');
		elm.find('form').bindToolkitData('LoadDataSet');
		// 提交搜索
		elm.find('.btn-search').click(function() {
			$(this).parents('form').bindToolkitData('SearchSubmit');
		});
	};
	// ----------------------------
	Toolkit.ui.DataTabExecute = function(elm) {
		elm.find('li[dataTab]').click(function() {
			if ($(this).hasClass('on')) return;
			$(this).siblings('.on').removeClass('on');
			$(this).addClass('on').trigger('loadData');
		});
		elm.find('li[dataTab]').bind('loadData', function() {
			$('.ui-list-frame').bindToolkitData('LoadDataListByTab', $(this).attr('dataTab'));
		});
        elm.find('li.on[dataTab]').first().trigger('loadData');
	};
	// ----------------------------
	// 详细页内容处理
	Toolkit.ui.DataDetailExecute = function(elm) {
		// 标题点击处理：折叠/展开
		elm.find('.mod-header').bindToolkitUI('DataHeaderCollapse', 'detail');
		// DetailElement显示处理
		elm.find('.tk-datatable').bindToolkitUI('DataDetailElementExecute');
	};
	Toolkit.ui.DataHeaderCollapse = function(elm, type) {
		elm.has('.btn-collapse').addClass('pointer').attr('title', '点击标题栏收起内容').click(function() {
			var element = $(this);
			var btnElm = element.find('.btn-collapse');
			if (btnElm.hasClass('expand')) {
				btnElm.removeClass('expand');
				element.attr('title', '点击标题栏收起内容');
				if (type=='detail')	{
					element.siblings('.mod-content,.mod-footer').show();
				} else {
					element.next().show();
				}
			} else {
				btnElm.addClass('expand');
				element.attr('title', '点击标题栏展开内容');
				if (type=='detail')	{
					element.siblings('.mod-content,.mod-footer').hide();
				} else {
					element.next().hide();
				}
			}
			if (type=='form') {
				Toolkit.ui.UpdateDataFormHeight();
			}
		});
		elm.has('.btn-collapse').each(function() {
			var element = $(this);
			var btnElm = element.find('.btn-collapse');
			if (btnElm.hasClass('expand')) {
				btnElm.addClass('expand');
				element.attr('title', '点击标题栏展开内容');
				if (type=='detail')	{
					element.siblings('.mod-content,.mod-footer').hide();
				} else {
					element.next().hide();
				}
				if (type=='form') {
					Toolkit.ui.UpdateDataFormHeight();
				}
			}
		});
	};
	Toolkit.ui.DataDetailElementExecute = function(elm) {
		elm.find('[dataControl=DetailHTML]').bindToolkitUI('HtmlEditor');
	};
	// ----------------------------
	// DetailList的tab处理
	Toolkit.ui.DataListLoad = function(elm) {
		// 自定义列表加载
		elm.find('.ui-list-dataload').each(function() {
			var element = $(this);
			element.bindToolkitData('LoadDataListByCustom', element.attr('dataUrl'));
			element.removeClass('ui-list-dataload').removeAttr('dataUrl');
		});
		// Detail页Tab从表加载
		if (elm.find('.list-tabs li').size()>0) {
			elm.find('.list-tabs li').bindToolkitUI('ToggleTabElement', {
				child:'.list-tabdatas li', 
				onToggle:function(tab, element) {
					if (tab.attr('dataUrl')) {
						element.bindToolkitData('LoadDataListByCustom', tab.attr('dataUrl'));
						tab.removeAttr('dataUrl');
					}
				}
			});
			elm.find('.list-tabs li:first').click();
		}
	};
	// ----------------------------
	// 数据表格显示处理
	Toolkit.ui.DataListExecute = function(elm) {
		// 标题栏点击处理：折叠/展开
		elm.find('.mod-header').bindToolkitUI('DataHeaderCollapse', 'list');
		// 数据表格显示处理
		$('.ui-list-frame').has('.ui-list-table').each(function() {
			$(this).bindToolkitUI('DataTableExecute');
		});
		// 数据
	};
	// 自动调整DataTable
	Toolkit.ui.DataTableExecute = function(elm) {
		if (elm.data('resize')) return;
		elm.data('resize', true);
		// 
		var autoheight = elm.attr('dataAutoHeight') || '';
		var table = elm.find('table');
		var tableframe = elm.find('.list-table');
		// 去除第一个th的左边框
		table.find('thead th:first').addClass('first');
		// 去除最后一行的底边
		table.find('tbody tr:last').addClass('last');
		// 计算表格宽度
		var tablewidth = 0;
		table.find('colgroup col').each(function() {
			if ($(this).attr('width')) {
				tablewidth += parseInt($(this).attr('width'),10);
			}
		});
		table.css('width', tablewidth+'px');
		// 表格宽度自适应处理
		var autowidth = false;
		if (table.width() <= tableframe.width()) {
			table.css({'width':'100%','min-width':tablewidth+'px'});
			if (table.find('colgroup col:last').size()>0) {
				table.find('colgroup col:last')[0].setAttribute('width', 'auto');
			}
			// IE8下bug： attr('width', 'auto')会出错
			// table.find('colgroup:last').attr('width', 'auto');
			autowidth = true;
		} else if ($.browser.msie && parseInt($.browser.version)==9) {
			// IE9下父元素overflow-x:auto与子元素hover的bug
			tableframe.css('overflow-x', 'scroll');
		}
		// 表格高度自适应处理：固定表头，自适应表格高度
		if (autoheight == 'true') {
			if ($(document).height() > $(window).height()) {
				var height = tableframe.height() - $(document).height() + $(window).height() - table.find('thead').height();
				if (height>100) {
					tableframe.css({height:height+'px'});
					// 复制并固定表头
					var overtable = table.clone();
					overtable.find('tbody').remove();
					tableframe.before('<div class="list-table list-table-over"></div>');
					elm.find('.list-table-over').append(overtable);
					table.find('thead').remove();
					if (!autowidth) {
						// 横向滚动时固定表头位置调整
						tableframe.scroll(function() {
							overtable.css('margin-left', (0-this.scrollLeft)+'px');
						});
					}
				}
			}
		}
		// 全选处理
		table.find('tbody input[type=checkbox]').bindToolkitUI('AddDataRowCheck');
		elm.find('thead input.e-checkall').click(function() {
			$(this).parents('.ui-list-frame').find('tbody input[type=checkbox]').bindToolkitData('CheckAll', this);
		});
		// 排序处理
		elm.find('thead th[dataSort]').click(function() {
			var sort = $(this).attr('dataSort') || '';
			var order = $(this).attr('dataOrder') || '';
			elm.bindToolkitData('LoadDataListByOrder',{sort:sort, order:order});
		});
		// 分页处理
		elm.find('.list-page a[dataPage]').click(function() {
			elm.bindToolkitData('LoadDataListByPage', $(this).attr('dataPage'));
			return false;
		});
		// 分页跳转处理
		elm.find('.list-page input[type=text]').keydown(function(e) {
			try {
				var code=e.which||e.keyCode||0;
				if (code==13) {
					$(this).blur();
					$(this).next().click();
				}
			} catch(err) {}
		});
		elm.find('.list-page a.btn').click(function() {
			var txtpage = $(this).prev();
			var p = txtpage.val();
			if (p == '' || !Toolkit.string.isPositiveInteger(p)) {
				Toolkit.page.alert('请输入要跳转的页数', function(){
					txtpage.focus();
				});
				return false;
			}
			elm.bindToolkitData('LoadDataListByPage', parseInt(p) - 1);
			return false;
		});
	};
	// ----------------------------
	// Insert/Update页的表单处理
	Toolkit.ui.DataFormExecute = function(elm) {
		// 折叠/展开处理
		elm.find('.mod-header').bindToolkitUI('DataHeaderCollapse', 'form');
		elm.bindToolkitUI('DataFormDetailExecute');
		elm.bindToolkitUI('DataFormListExecute');
		Toolkit.ui.UpdateDataFormHeight();
		//
		elm.find('form').bindToolkitData('LoadDataSet');
		// 提交搜索
		elm.find('.btn-submit').click(function() {
			var url = $(this).attr('href') || '#';
			if (url =="#" || url =="##" || url =="###" ) url= null;
			$(this).parents('form').bindToolkitData('UpdateSubmit', url);
			return false;
		});

        // 关闭对话框
		elm.find('.btn-close-dialog').click(function () {
		    parent.Toolkit.util.dialog.close();
		    return false;
		});
	    // 对话框提交搜索
		elm.find('.btn-submit-dialog').click(function () {
		    var url = $(this).attr('href') || '#';
		    if (url == "#" || url == "##" || url == "###") url = null;
		    $(this).parents('form').bindToolkitData('UpdateSubmitDialog', url);
		    return false;
		});
	};
	Toolkit.ui.DataFormDetailExecute = function(elm) {
		// 避免list-table下的隐藏模板里的tk-datatable被解析
		elm.find('.tk-datatable').each(function() {
			if ($(this).parent().tagName()=='td') return;
			$(this).bindToolkitUI('DataDetailElementExecute');
			$(this).bindToolkitUI('DataFormElementExecute', 'detail');
		});
	};
	Toolkit.ui.DataFormListExecute = function(elm) {
		elm.find('.list-table table').bindToolkitUI('DataFormListTableExecute');
		elm.find('.list-table table').bindToolkitUI('AddRowFormHeaderCollapse');
		elm.find('.list-table tbody.list>tr').bindToolkitUI('DataDetailElementExecute');
		elm.find('.list-table tbody.list>tr').bindToolkitUI('DataFormElementExecute', 'list');
		elm.find('.list-table tbody.list>tr').bindToolkitUI('UpdateFormRadioGroup');
		elm.find('.ui-newrow').bindToolkitUI('DataFormElementExecute');
		elm.bindToolkitUI('DataFormListActionExecute');
	};
	// form下的list table宽度自适应处理
	Toolkit.ui.DataFormListTableExecute = function(table) {
		if (table.size()==0 || table.hasClass('w100p')) return;
		var tablewidth = 0;
		table.find('colgroup col').each(function() {
			if ($(this).attr('width')) {
				tablewidth += parseInt($(this).attr('width'),10);
			} else {
				$(this).attr('width', '150');
				tablewidth += 150;
			}
		});
		if (tablewidth < table.parent().width()) {
			table.find('colgroup').append('<col />');
			table.find('thead tr').append('<th><div>&nbsp;</div></th>');
			table.find('tbody tr').append('<td><span>&nbsp;</span></td>');
			table.addClass('w100p');
		} else {
			table.attr('width', tablewidth);
		}
	};
	Toolkit.ui.DataFormElementExecute = function(elm, type) {
		// 基本的UI处理
		elm.find('[dataControl=Date], [dataControl=DateTime]').bindToolkitUI('DatePicker');
		elm.find('[dataControl=EasySearch]').bindToolkitUI('EasySearch');
		elm.find('[dataControl=MultiEasySearch]').bindToolkitUI('MultiEasySearch');
		elm.find('[dataControl=Upload]').bindToolkitUI('UploadFile');
		elm.find('[dataControl=HTML]').bindToolkitUI('HtmlEditor');
		// 基本的Data处理
		elm.find('input[dataHint],textarea[dataHint]').bindToolkitData('InputHint');
		elm.find('input[dataLimitType],textarea[dataLimitType]').bindToolkitData('InputLimit');
		elm.find('input[dataValidType],textarea[dataValidType],input[dataRequired],textarea[dataRequired],select[dataRequired]').bindToolkitData('InputValid');
		// KeyBoard相关处理
		var type = type || '';
		if (type=='detail') {
			elm.find('.tk-control input[type=text], .tk-control input[type=password], .tk-control textarea').bindToolkitUI('KeyBoardExecute', 'dl');
		} else if (type=='list') {
			elm.find('.tk-control input[type=text], .tk-control input[type=password], .tk-control textarea').bindToolkitUI('KeyBoardExecute', 'table');
			// 添加折叠和展开
			elm.find('.tk-datatable').bindToolkitUI('AddRowFormCollapse');
		}
	};
	// 当有RowForm时，给head增加折叠和展开按钮
	Toolkit.ui.AddRowFormHeaderCollapse = function(elm) {
		elm.bind('createCollapseExpandButton', function() {
			if (elm.data('collapseExpandButtonCreated')) return;
			var collapse = $('<a href="#" class="ico-l-collapse mr5 blue">全部收起</a>');
			collapse.click(function() {
				$(this).closest('table').find('tbody.list .tk-datatable').trigger('collapseRowForm');
				return false;
			});
			var expand = $('<a href="#" class="ico-l-expand mr5 blue">全部展开</a>');
			expand.click(function() {
				$(this).closest('table').find('tbody.list .tk-datatable').trigger('expandRowForm');
				return false;
			});
			var btnContainer = $('<div class="fr"></div>');
			btnContainer.append(collapse);
			btnContainer.append(expand);
			elm.find('thead th:last').prepend(btnContainer);
			elm.data('collapseExpandButtonCreated', true);
		});
	}; 
	Toolkit.ui.AddRowFormCollapse = function(elm) {
		// 高度小于50时，不添加折叠内容
		if (elm.height()<50) return;
		//
		elm.closest('table').trigger('createCollapseExpandButton');
		//
		elm.after('<div class="tk-datatable-more"><a href="#" class="blue ico-l-collapse">隐藏部分表单内容</a></div>');
		elm.bind('collapseRowForm', function() {
			$(this).parent().addClass('collapse');
			$(this).next().children('a').attr('class', 'ico-l-expand').html('显示完整表单内容');
		});
		elm.bind('expandRowForm', function() {
			$(this).parent().removeClass('collapse');
			$(this).next().children('a').attr('class', 'ico-l-collapse').html('隐藏部分表单内容');
		});
		elm.next().children('a').click(function() {
			if ($(this).hasClass('ico-l-collapse')) {
				$(this).parent().parent().addClass('collapse');
				$(this).attr('class', 'ico-l-expand').html('显示完整表单内容');
			} else {
				$(this).parent().parent().removeClass('collapse');
				$(this).attr('class', 'ico-l-collapse').html('隐藏部分表单内容');
			}
			return false;
		});
	};
	// 对RadioGroup做特殊处理，因为name要用于radio分组。
	Toolkit.ui.UpdateFormRadioGroup = function(elm) {
		elm.each(function() {
			var row = $(this);
			var rowIndex = '_'+ row.find('input.row-index').val() +'_'+ (new Date()).getTime();
			row.find('input[type=radio]').each(function() {
				var id = $(this).attr('name');
				$(this).attr('id', id);
				$(this).attr('name', id + rowIndex);
			});
		});
	};
	// 方向键切换输入对象
	Toolkit.ui.KeyBoardExecute = function(elm, type) {
//		elm.keydown(function(e) {
//			try {
//				var code=e.which||e.keyCode||0;
//				var data = Toolkit.data.getTextInputsIndex(this, type);
//				switch(code) {
//					case 37://left
//						if (data.left.size()>0 && data.left.is(':visible')) {
//							this.blur();
//							data.left.focus();
//						}
//						break;
//					case 38://up
//						if ($(this).data('suggestStatus')) return;
//						if (data.up.size()>0 && data.up.is(':visible')) {
//							this.blur();
//							data.up.focus();
//						}
//						break;
//					case 39://right
//						if (data.right.size()>0 && data.right.is(':visible')) {
//							this.blur();
//							data.right.focus();
//						}
//						break;
//					case 40://down
//						if ($(this).data('suggestStatus')) return;
//						if (data.down.size()>0 && data.down.is(':visible')) {
//							this.blur();
//							data.down.focus();
//						}
//						break;
//				}
//			} catch(err) {}
//		});
	};
	//
	Toolkit.ui.DataFormListActionExecute = function(elm) {
		// 选中状态修改
		elm.find('tbody.list input.row-index').bindToolkitUI('AddDataRowCheck');
		// 全选处理
		elm.find('thead input.e-checkall').click(function() {
			$(this).parents('table').find('tbody.list input.row-index').bindToolkitData('CheckAll', this);
		});
		// 选择按钮处理
		elm.find('.ui-btn-checkall').click(function() {
			elm.find('thead input.e-checkall').attr('checked', true);
			elm.find('tbody.list input.row-index').bindToolkitData('CheckAll', true);
			return false;
		});
		elm.find('.ui-btn-checknone').click(function() {
			elm.find('thead input.e-checkall').attr('checked', false);
			elm.find('tbody.list input.row-index').bindToolkitData('CheckAll', false);
			return false;
		});
		elm.find('.ui-btn-checkreverse').click(function() {
			elm.find('tbody.list input.row-index').bindToolkitData('CheckReverse');
			return false;
		});
		// 删除处理
		elm.find('.ui-btn-delrow').click(function() {
			var rowlist = Toolkit.data.getCheckedValue(elm.find('tbody.list input.row-index'));
			if (rowlist.length == 0) {
				Toolkit.page.alert('请选择要删除的数据行');
				return false;
			}
			var element = $(this);
			Toolkit.page.confirm('确认删除所选择的数据行？', function() {
				Toolkit.stat.addFuncStat('Toolit.ui.DeleteDataRow-'+rowlist.length);
				var table = element.parents('.ui-list-frame').find('table');
				if (rowlist.length == table.find('tbody.list tr').size()) {
					table.find('tbody.list>tr').remove();
					table.bindToolkitData('DeleteAllDataSetRow');
				} else {
					for (var i=rowlist.length-1; i>=0; i--) {
						table.find('tbody.list>tr:eq('+(rowlist[i]-1)+')').remove();
					}
					table.find('tbody.list>tr').each(function(i) {
						$(this).find('input.row-index').val(i + 1);
						$(this).find('span.row-index').html(i + 1);
					});
					table.bindToolkitData('DeleteDataSetRow', rowlist);
				}
				Toolkit.ui.UpdateDataFormHeight();
				Toolkit.stat.addFuncStat('Toolit.ui.DeleteDataRow-'+rowlist.length);
			});
			return false;
		});
		elm.find('.ui-btn-delall').click(function() {
			var element = $(this);
			Toolkit.page.confirm('确认删除全部数据行？', function() {
				var table = element.parents('.ui-list-frame').find('table');
				table.find('tbody.list tr').remove();
				table.bindToolkitData('DeleteAllDataSetRow');
				Toolkit.ui.UpdateDataFormHeight();
			});
			return false;
		});
		// 新建列表
		elm.find('.ui-newrow input[type=text]').keydown(function(e) {
			try {
				var code=e.which||e.keyCode||0;
				if (code==13) {
					$(this).blur();
					$(this).next().click();
				}
			} catch(err) {}
		});
		elm.find('.ui-newrow a.btn').click(function() {
			var element = $(this);
			var txtrow = element.prev();
			var count = txtrow.val();
			if (count == '' || !Toolkit.string.isPositiveInteger(count)) {
				Toolkit.page.alert('请输入要新建的行数', function() {
					txtrow.focus();
				});
				return false;
			}
			if (count>100) {
				Toolkit.page.alert('一次最多只能新建100行，请修改行数', function() {
					txtrow.focus();
				});
				return false;
			}
			if (count>10) Toolkit.page.showLoading('正在创建数据行……');
			setTimeout(function() {
				element.parents('.ui-list-frame').find('table').bindToolkitUI('CreateDataRow', count);
				if (count>10) Toolkit.page.hideLoading();
			}, 100);
			return false;
		});
	}; 
	//
	// 创建数据列表
	Toolkit.ui.CreateDataRow = function(elm, count) {
		Toolkit.stat.addFuncStat('Toolit.ui.CreateDataRow-'+count);
		elm.find('tbody.list .tk-datatable').trigger('collapseRowForm');
		var row = elm.find('tbody.template>tr');
		var rowCount = elm.find('tbody.list>tr').size();
		for (var i=0; i<count; i++) {
			var newrow = row.clone();
			elm.find('tbody.list').append(newrow);
		}
		var rows = (rowCount>0)?elm.find('tbody.list>tr:gt('+(rowCount-1)+')'):elm.find('tbody.list>tr');
		rows.each(function(i) {
			$(this).find('input.row-index').val(rowCount + i + 1);
			$(this).find('span.row-index').html(rowCount + i + 1);
			$(this).bindToolkitData('InsertDataSetRow');
		});
		rows.find('input.row-index').bindToolkitUI('AddDataRowCheck');
		rows.bindToolkitUI('DataDetailElementExecute');
		rows.bindToolkitUI('DataFormElementExecute', 'list');
		rows.bindToolkitUI('UpdateFormRadioGroup');
		Toolkit.ui.UpdateDataFormHeight();
		Toolkit.stat.addFuncStat('Toolit.ui.CreateDataRow-'+count);
	};
	Toolkit.ui.AddDataRowCheck = function(elm) {
		elm.click(function() {
			$(this).bindToolkitData('CheckDataRow');
		});		
	};
	Toolkit.ui.UpdateDataFormHeight = function() {
		if ($('.tk-module .tk-dataform').size()>0) return;
		if ($(document).height()>$(window).height()+10 && !$.browser.isIPad) {
			$('.tk-dataform').has('.mod-footer').addClass('tk-dataform-long');
		} else {
			$('.tk-dataform').has('.mod-footer').removeClass('tk-dataform-long');
		}
	};
	// ----------------------------
	// 页面链接处理
    Toolkit.ui.DataLinksExecute = function(elm) {
		elm.find('a[href]').click(function() {
			if ($(this).attr('href')=='') return false;
			if ($(this).attr('href').startWith('#')) return false;
			if ($(this).attr('href').startWith('javascript')) return false;
			if ($(this).attr('data-Confirm')) return false;
			if ($(this).attr('target')) return true;

			Toolkit.page.showLoading('正在加载页面……');
			if (Toolkit.page.isSubWin) {
				top.Toolkit.page.setLoadingTimer();
			}			
			return true;
		});
		elm.find('a[data-Confirm]').click(function() {
			var element = $(this);
			Toolkit.page.confirm(element.attr('data-Confirm'), function() {
				if (element.attr('targetContainer')) {
					Toolkit.page.showLoading('正在'+element.text()+'……');
					$.ajax({url:element.attr('href'), dataType:'text', success:function(req) {
						if (req=='OK') {
							element.closest(element.attr('targetContainer')).bindToolkitData('DataListRefresh');
						} else {
							Toolkit.page.showError(element.text()+'失败，请稍候重试');
						}
					}, error:function() {
						Toolkit.page.showError(element.text()+'失败，请稍候重试');
					}});
				} else {
					//Toolkit.page.showLoading('正在'+element.text()+'……');
					document.location.href = element.attr('href');
				}
			});
			return false;
		});
		elm.find('a[dialogUrl]').click(function() {
			var param = { title:$(this).attr('title')||'详细信息', url:$(this).attr('dialogUrl'), width:700, height:300, ajaxComplete:function(elm) {
				elm.find('.tk-datadetail').bindToolkitUI('DataDetailExecute');
				elm.find('.tk-dataform').bindToolkitUI('DataFormExecute');
			} };
			$.extend(param, $(this).attrJSON('dialogParam'));
			top.Toolkit.page.dataList.current = ($('.tk-datalist').size()>0) ? $(this).closest('.ui-list-frame') : $('.tk-page');
			top.Toolkit.page.dialog.ajax(param);
			return false;
		});
		elm.find('a[dialogPage]').click(function() {
			var param = { title:$(this).attr('title')||'详细信息', url:$(this).attr('dialogPage'), width:700, height:300 };
			$.extend(param, $(this).attrJSON('dialogParam'));
			top.Toolkit.page.dataList.current = ($('.tk-datalist').size()>0) ? $(this).closest('.ui-list-frame') : $('.tk-page');
			top.Toolkit.page.dialog.pop(param);
			return false;
		});
		elm.find('a[targetSection]').click(function() {
			$($(this).attr('targetSection')).bindToolkitData('LoadDataListByCustom', $(this).attr('href'));
			return false;
		});
	};
	// 模块高度自适应
	Toolkit.ui.SectionHeightAutoFit = function(elm, fixheight) {
		var fixheight = fixheight || 0;
		elm.bind('heightAutoFit', function(e, fix) {
			$(this).css('height', ($(this).height() - fix) +'px');
		});
		elm.trigger('heightAutoFit', $('body').height() + fixheight - $(window).height());
		$(window).resize(function() {
			elm.trigger('heightAutoFit', $('body').height() + fixheight - $(window).height());
		});
	};
	// 加载Section片段内容
	Toolkit.ui.LoadDataSection = function(elm) {
		elm.find('.ui-load-section').each(function() {
			var element = $(this);
			element.bindToolkitData('LoadDataListByCustom', element.attr('dataUrl'));
			element.removeClass('ui-load-section').removeAttr('dataUrl');
		});
	};
	// ----------------------------
	// 页面内容加载后的初始化
	Toolkit.ui.PageContentExecute = function(elm) {
		if (elm.tagName()=='body' || elm.hasClass('tk-page')) {
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
	// 页面链接处理【框架页使用】
	Toolkit.ui.PageLinksExecute = function(elm) {
		elm.find('a[targetTab]').click(function() {
			if ($(this).attr('href')=='' || $(this).attr('href').startWith('#')) return false;
			Toolkit.page.showTab('page', $(this).attr('targetTab'), $(this).text(), $(this).attr('href'));
			return false;
		});
		elm.find('a[targetFrame]').click(function() {
			if ($(this).attr('href')=='' || $(this).attr('href').startWith('#')) return false;
			Toolkit.page.showTab('frame', $(this).attr('targetFrame'), $(this).text(), $(this).attr('href'));
			return false;
		});
	};
	// ----------------------------
})(jQuery);