// --------------------------------
// Toolkit.ui.suggest
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.base');
    Toolkit.namespace('Toolkit.ui');
    // ----------------------------
    if (Toolkit.ui.suggest) return;
    // ----------------------------
    Toolkit.ui.suggest = function (elm, options) {
        if (!options || !options.source) return;
        elm.each(function () {
            new Toolkit.base.suggest(this, options);
        });
    };
    Toolkit.base.suggest = function (input, options) {
        options.delay = options.delay || 100;
        options.selectClass = options.selectClass || 'active';
        options.minchars = options.minchars || 0;
        options.onSelect = options.onSelect || false;
        options.charLengthCheck = options.charLengthCheck || false;
        //var postdata = {"SELECT":[{"TYPE":options.source.getQueryJson()['Source'],"VALUE":""}]};
        var $input = $(input).attr("autocomplete", "off");
        var postdata = { "RegName": $input.attr("data-regName"), "Text": "", "RefField": [] };
        if ($('#tk-easysearch').size() == 0) {
            $('body').append('<ul id="tk-easysearch" class="list-group"></ul>');
        }
        var $results = $('#tk-easysearch');
        var timeout = false;		// hold timeout ID for suggestion results to appear
        var prevLength = -1;			// last recorded length of $input.val()
        $input.blur(function () {
            setTimeout(function () { $results.hide() }, 200);
        });
        //
        //	if ($.browser.mozilla) {
        //		// onkeypress repeats arrow keys in Mozilla/Opera
        //		$input.keypress(processKey2);	
        //	} else {
        //		// onkeydown repeats arrow keys in IE/Safari*/
        //		$input.keydown(processKey2);
        //	}
        $input.focus(processKey2);
        $input.keyup(processKey2);
        //
        function processKey(e) {
            // handling up/down/escape requires results to be visible
            // handling enter/tab requires that AND a result to be selected
            if (/^32$|^9$/.test(e.keyCode) && getCurrent()) {
                if (e.preventDefault) e.preventDefault();
                if (e.stopPropagation) e.stopPropagation();
                e.cancelBubble = true;
                e.returnValue = false;
                selectCurrent();
                //	} else if ($input.val().length != prevLength) {
            } else {
                if (timeout) clearTimeout(timeout);
                timeout = setTimeout(suggest, options.delay);
                prevLength = $input.val().length;
            }
        }
        //此处针对上面介绍的修改增加的函数
        function processKey2(e) {
            // handling up/down/escape requires results to be visible
            // handling enter/tab requires that AND a result to be selected
            if (/13$|27$|38$|40$/.test(e.keyCode) && $results.is(':visible')) {
                if (e.preventDefault) e.preventDefault();
                if (e.stopPropagation) e.stopPropagation();
                e.cancelBubble = true;
                e.returnValue = false;
                switch (e.keyCode) {
                    case 13: // enter
                        selectCurrent();
                        break;
                    case 38: // up
                        selectPrev();
                        break;
                    case 40: // down
                        selectNext();
                        break;
                    case 27: //	escape
                        hideSuggest();
                        break;
                }
                //	} else if ($input.val().length != prevLength) {
            } else {
                if (timeout) clearTimeout(timeout);
                timeout = setTimeout(suggest, options.delay);
                prevLength = $input.val().length;
            }
        }
        //
        function suggest() {
            var q = $.trim($input.val());
            if (q.length >= options.minchars) {
                if (options.onBeforeSuggest) {
                    options.onBeforeSuggest();
                }
                postdata.Text = q;
                postdata.RefField = [];
                //var refFields = $input.attr("data-refFields");
                if (options.refFields && options.refFields != "") {
                    var refs = options.refFields.parseJSON();
                    var len = refs.length;
                    for (var i = 0; i < len; ++i) {
                        var item = refs[i];
                        //var itemValue = Toolkit.data.getElementValue(options.refFields[item]);
                        var ctrl = Toolkit.data._getCtrl($input, item.RefField, item.CtrlType);
                        var refValue;
                        if (ctrl.HiddenControl)
                            refValue = ctrl.HiddenControl.val();
                        else
                            refValue = ctrl.Control.val();
                        postdata.RefField.push({ "NickName": item.Field, "Value": refValue });
                    }
                }
                $.ajax({
                    type: 'post',
                    url: options.source,
                    data: Toolkit.json.stringify(postdata),
                    dataType: 'json',
                    success: function (data) {
                        hideSuggest();
                        if (data) {
                            displayItems(data);
                        }
                    }
                });
            } else {
                hideSuggest();
            }
        }
        //
        function displayItems(items) {
            if (!items) return;
            if (!items.length) {
                hideSuggest();
                return;
            }
            var htmlstr = '';
            var htmlTemplate = '<li class="list-group-item" data-id="{Value}" data-text="{Name}"><span>{DisplayName}</span></li>';
            for (var i = 0; i < items.length; i++) {
                var $str = $(htmlTemplate.substitute(items[i]));
                
                if ($input.parents('tr').length > 0) {
                    $str.attr({ 'data-trindex': $input.closest('.table-row')[0].rowIndex, 'data-panelid': $input.closest('.tk-datatable').attr('id')});
                }

                if ($input.data('addition') != '' && typeof $input.data('addition') != "undefined") {
                    $str.attr('data-addition', JSON.stringify(items[i].Data));
                }
                htmlstr += $str[0].outerHTML;
            }
            $results.html(htmlstr);
            $results.children('li').mouseover(function () {
                $results.children('li').removeClass(options.selectClass);
                $(this).addClass(options.selectClass);
            }).click(function (e) {
                Toolkit.ui.linkChange(this, $input.data('addition'), $(this).data('addition'));
                e.preventDefault();
                e.stopPropagation();
                selectCurrent();
            });
            showSuggest();
            if ($results.children('li').size() == 1 && $input.val().trim() != '') {
                $results.children('li').addClass(options.selectClass);
                //selectCurrent();
            }
        }
        function showSuggest() {
            var control = $input.closest('.tk-control');
            var ctrlWidth = control.width();
            if (ctrlWidth === 0) // Chrome中ctrlWidth是0
                ctrlWidth = $input.width() + 15;
            $results.show().css({
                width: ctrlWidth,
                top: 0,
                left: control.offset().left
            });
            if ($(window).height() - $input.offset().top - $input.height() > $results.height() || $input.offset().top - $results.height() < 0) {
                $results.css('top', ($input.offset().top + $input.outerHeight() - 1) + 'px');
            } else {
                $results.css('top', ($input.offset().top - $results.height()) + 'px');
            }
            $input.data('suggestStatus', true);
        }
        function hideSuggest() {
            $results.hide();
            $input.data('suggestStatus', false);
        }
        function getCurrent() {
            if (!$results.is(':visible')) return false;
            var $current = $results.children('li.' + options.selectClass);
            if (!$current.length) $current = false;
            return $current;
        }
        function selectCurrent() {
            $current = getCurrent();
            if ($current) {
                var id = $current.attr('data-id');
                var text = $current.attr('data-text');
                if (options.onSelect) options.onSelect($input, id, text);
                hideSuggest();
            }
        }
        function selectNext() {
            $current = getCurrent();
            if ($current) {
                $current.removeClass(options.selectClass).next().addClass(options.selectClass);
            } else {
                $results.children('li:first').addClass(options.selectClass);
            }
        }
        function selectPrev() {
            $current = getCurrent();
            if ($current) {
                $current.removeClass(options.selectClass).prev().addClass(options.selectClass);
            } else {
                $results.children('li:last').addClass(options.selectClass);
            }
        }
    };
    // ----------------------------
    Toolkit.debug('suggest.js', '初始化成功');
})(jQuery);