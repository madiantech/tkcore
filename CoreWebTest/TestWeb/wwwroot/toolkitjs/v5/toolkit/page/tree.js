(function ($) {
    // 定义新建和修改后的重刷页面内容处理
    $('.tk-page').bind('refreshDataPage', function (e, url) {
        Toolkit.page.showLoading();
        document.location.href = url;
    });
    //
    Toolkit.load('treeview');
    Toolkit.namespace('Toolkit.page.tree');
    // 初始数据列表处理
    Toolkit.page.tree.loadTreeData = function (elm, param) {
        Toolkit.page.showLoading();
        var param = param || {};
        param.Id = param.Id || '';
        param.ExId = param.ExId || '';
        var url = elm.attr('dataUrl').substitute(param);
        elm.load(url, function () {
            Toolkit.page.hideLoading();
            if (param.callback) {
                param.callback(elm);
            }
        });
    };
    Toolkit.page.tree.initTreeItem = function (elm) {
        elm.find('a.folder').each(function () {
            if ($(this).siblings('ul').size() > 0) {
                $(this).siblings('ul').addClass('dataloaded');
                return;
            }
            $(this).parent().addClass('closed');
            $(this).after('<ul></ul>');
        });
        elm.find('a').click(function () {
            $('#treeview a.checked').removeClass('checked');
            $(this).addClass('checked');
            Toolkit.page.showLoading('加载节点内容……');
            $('#nodeDetail').load($(this).attr('detailPage'), function () {
                $('#nodeDetail').bindToolkitUI('DataDetailExecute');
                $('#nodeDetail').bindToolkitUI('DataLinksExecute');
                $('#nodeDetail').find('.tk-datalist').bindToolkitUI('DataListLoad').bindToolkitUI('DataListExecute');
                //$('#nodeDetail .mod-content').css('height', $('#nodeDetail .mod-content').height()+10);
                Toolkit.page.hideLoading();
            });
        });
    };
    Toolkit.page.tree.initTreeView = function (elm) {
        elm.treeview({
            persist: "location",
            toggle: function () {
                var element = $(this);
                if (element.children('a').hasClass('file') || element.children('ul').size() == 0) return;
                if (element.hasClass('collapsable') && !element.children('ul').hasClass('dataloaded')) {
                    Toolkit.page.tree.loadSubTree(element.children('ul'));
                }
            }
        });
    };
    Toolkit.page.tree.initTreeSortable = function () {
		$('#treeview li').bindJqueryUI('draggable', 'destroy');
		$('#treeview li>a').bindJqueryUI('droppable', 'destroy');
		$('#treeview li').bindJqueryUI('draggable', {
			helper: function(event) {
				return $('<div class="bgffc lh20 w100 bdc tac oh op5 move">'+ $(this).children('a').text() +'</div>');
			}
		});
		$('#treeview li>a').bindJqueryUI('droppable', {
			over: function(event, ui) {
				if (ui.draggable.parent().parent()[0]==$(this).parent()[0]) return;
				$(this).addClass('over');
			},
			out: function(event, ui) {
				if (ui.draggable.parent().parent()[0]==$(this).parent()[0]) return;
				$(this).removeClass('over');
			},
			drop: function(event, ui) {
				if (ui.draggable.parent().parent()[0]==$(this).parent()[0]) return;
				$(this).removeClass('over');
				$(this).addClass('dest');
				Toolkit.page.tree.moveTreeItem(ui.draggable, $(this).parent());
			}
		});
    };
    Toolkit.page.tree.moveTreeItem = function (source, dest) {
		var sourceNode = source.children('a');
		var destNode = dest.children('a');
		var sourceTitle = sourceNode.text();
		var destTitle = destNode.text();
		Toolkit.page.confirm('确认将<span class="bold">'+sourceTitle+'</span>移动<span class="bold">'+destTitle+'</span>下？', function() {
			if (destNode.hasClass('file')) {
				destNode.removeClass('file');
				destNode.addClass('folder');
				dest.append('<ul></ul>');
			}
			var apiUrl = $('#treeview').attr('dataMoveUrl').substitute({Oid:source.attr('dataId'), Nid:dest.attr('dataId')});
			Toolkit.page.showLoading('正在提交数据……');
			$.ajax({dataType:'text', url:apiUrl, success:function(req) {
				dest.children('a.dest').removeClass('dest');
				if (req.trim()=='') {
					dest.children('ul').append(source);
					Toolkit.page.hideLoading();
				} else {
					Toolkit.page.showError('移动失败，请重试');
				}
			}, error:function() {
				Toolkit.page.showError('移动失败，请重试');
				dest.children('a.dest').removeClass('dest');
			}});
		}, function() {
			dest.children('a.dest').removeClass('dest');
		});
    };
    Toolkit.page.tree.initTreeAction = function () {
        $('#nodeTree a.ico-l-up').click(function () {
            Toolkit.page.tree.sortTreeItem($('#treeview a.checked').parent(), 'up', $(this).attr('dataUrl'));
        });
        $('#nodeTree a.ico-l-down').click(function () {
            Toolkit.page.tree.sortTreeItem($('#treeview a.checked').parent(), 'down', $(this).attr('dataUrl'));
        });
    };
    Toolkit.page.tree.sortTreeItem = function (elm, direct, url) {
        var dest = (direct == 'up') ? elm.prev() : elm.next();
        if (dest.size() == 0) return;
        var dataId = elm.attr('dataId');
        var apiUrl = url.substitute({ Id: dataId });
        Toolkit.page.confirm('确认移动当前节点？', function () {
            Toolkit.page.showLoading('正在提交数据……');
            $.ajax({ dataType: 'text', url: apiUrl, success: function (req) {
                if (req.trim() == '') {
                    if (direct == 'up') {
                        elm.after(dest);
                    } else {
                        dest.after(elm);
                    }
                    Toolkit.page.hideLoading();
                } else {
                    Toolkit.page.showError('移动失败，请重试');
                }
            }, error: function () {
                Toolkit.page.showError('移动失败，请重试');
            }
            });
        });
    };
    Toolkit.page.tree.addTreeBranches = function (elm) {
        elm.treeview({
            add: elm.children('li'),
            toggle: function () {
                var element = $(this);
                if (element.children('a').hasClass('file') || element.children('ul').size() == 0) return;
                if (element.hasClass('collapsable') && !element.children('ul').hasClass('dataloaded')) {
                    Toolkit.page.tree.loadSubTree(element.children('ul'));
                }
            }
        });
    };
    // 读取SubTree数据
    Toolkit.page.tree.loadSubTree = function (elm) {
        elm.addClass('dataloaded');
        elm.siblings('.hitarea').addClass('placeholder');
        var parentId = elm.parent().attr('dataId') || '';
        var url = $('#treeview').attr('dataUrl').substitute({ Id: parentId, ExId: '' });
        elm.load(url, function () {
            Toolkit.page.tree.initTreeItem(elm);
            Toolkit.page.tree.addTreeBranches(elm);
            Toolkit.page.tree.initTreeSortable();
            elm.siblings('.hitarea').removeClass('placeholder');
        });
    };
    Toolkit.page.tree.init = function () {
        var exId = $('#treeview').attr('dataExID') || '';
        $('#treeview').parent().css('overflow', 'auto');
        Toolkit.page.tree.loadTreeData($('#treeview'), { ExId: exId, callback: function (elm) {
            if (elm.find('li>a').size() > 0) {
                $('#nodeTree').addClass('fl').addClass('w200').show();
                $('#nodeTree .mod-footer, #nodeDetail').show();
                Toolkit.page.tree.initTreeItem(elm);
                Toolkit.page.tree.initTreeView(elm);
                Toolkit.page.tree.initTreeSortable();
                Toolkit.page.tree.initTreeAction();
                if (exId != '') {
                    elm.find('li[dataId=' + exId + ']>a').click();
                } else if (elm.find('li').size() > 0) {
                    elm.find('li>a:first').click();
                }
                $('.mod-content:first').bindToolkitUI('SectionHeightAutoFit', 25);
			} else if (elm.find('li').size()>0) {
				$('#nodeTree').show();
				$('#treeview').bindToolkitUI('DataLinksExecute');
			} else {
				elm.html('<li><div class="p20 f14">列表为空。</div></li>')
				$('#nodeTree').show();
				$('#nodeTree .mod-footer').show();
				$('#nodeTree .mod-footer a.ico-l-up').hide();
				$('#nodeTree .mod-footer a.ico-l-down').hide();
			}
        }});
    };
    $(function () {
        Toolkit.load('treeview', function () {
            Toolkit.page.tree.init();
        });
    });
})(jQuery);