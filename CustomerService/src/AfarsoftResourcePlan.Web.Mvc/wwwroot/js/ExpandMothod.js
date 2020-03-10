function Guid() {
    return 'axxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function commaTrim(x) {
    return x.replace(/^,+|,+$/gm, '');
};

function initMyComponent() {
    $(".easyui-textbox-user").each(function () {
        $(this).textbox($.extend({}, __userOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-goods").each(function () {
        $(this).textbox($.extend({}, __goodsGeneralOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-org").each(function () {
        $(this).textbox($.extend({}, __organizationOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-warehouse").each(function () {
        $(this).textbox($.extend({}, __wareHouseOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-map").each(function () {
        $(this).textbox($.extend({}, __mapOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-servicearea").each(function () {
        $(this).textbox($.extend({}, __serviceAreaOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-member").each(function () {
        $(this).textbox($.extend({}, __memberOption, $.fn.textbox.parseOptions(this)));
    });
    $(".easyui-textbox-customer").each(function () {
        $(this).textbox($.extend({}, __customerOption, $.fn.textbox.parseOptions(this)));
    });

    //$("region").each(function () {
    //    var guid = Guid();
    //    $(this).wrap("<div id='" + guid + "'></div>");
    //    var vueInstance = new Vue({
    //        el: "#" + guid
    //    });
    //});
}
function initRegionSelector(province, city, region, path) {
    var inited = false;
    $(province).combobox({
        onSelect: function (record) {
            if (inited) {
                $.getJSON("/api/services/app/AreaInfo/GetAreaInfoListByUser", { level: 1, fatherId: record.id }, function (data) {
                    $(city).combobox("clear").combobox("loadData", data.result.data).combobox("setValue", data.result.data[0].id);
                });
            }
        }
    });
    $(city).combobox({
        onSelect: function (record) {
            if (inited) {
                $.getJSON("/api/services/app/AreaInfo/GetAreaInfoListByUser", { level: 2, fatherId: record.id }, function (data) {
                    $(region).combobox("clear").combobox("loadData", data.result.data).combobox("setValue", data.result.data[0].id);
                });
            }
        }
    });

    $.getJSON("/api/services/app/AreaInfo/GetAreaInfoListByUser", { level: 0 }, function (data1) {
        $(province).combobox("clear").combobox("loadData", data1.result.data);
        var pathArr = path ? commaTrim(path).split(',') : null;
        if (pathArr) {
            $(province).combobox("select", pathArr[0]);
        }

        $.getJSON("/api/services/app/AreaInfo/GetAreaInfoListByUser", { level: 1, fatherId: pathArr ? pathArr[0] : data1.result.data[0].id }, function (data2) {
            $(city).combobox("clear").combobox("loadData", data2.result.data);
            if (pathArr) {
                $(city).combobox("select", pathArr[1]);
            }

            $.getJSON("/api/services/app/AreaInfo/GetAreaInfoListByUser", { level: 2, fatherId: pathArr ? pathArr[1] : data2.result.data[0].id }, function (data3) {
                $(region).combobox("clear").combobox("loadData", data3.result.data);
                if (pathArr) {
                    $(region).combobox("select", pathArr[2]);
                }
                inited = true;
            });
        });
    });
}

//var abp = {
//    auth: {
//        isGranted: function (v) {
//            return true;
//        }
//    }
//};

function renderGridButton(buttons, maxcount, separator) {
    buttons = buttons.filter(function (s) {
        return s && s.trim();
    });
    var overstep = buttons.length - (maxcount || buttons.length);
    if (overstep > 0) {
        var guid = Guid();
        var menu = $("<div id='" + guid + "'></div>");
        var subButtons = buttons.splice(maxcount - 1);
        for (var i = 0; i < subButtons.length; i++) {
            var dom = $(subButtons[i]).wrap("<div></div>");
            menu.append("<div>" + subButtons[i] + "</div>")
        }
        menu.appendTo("body");
        buttons.push('<a id="btn-edit" href="#" class="easyui-menubutton" data-options="menu:\'#' + guid + '\'">更多</a>');
    }
    return buttons.join(separator || '&nbsp;');
}
function addGridButton(title, callback, condition, authority) {
    var href = '#';
    var guid = Guid();

    if ((typeof callback) == 'string') {
        href = callback;
    } else {
        $(document).delegate("#" + guid, "click", function () { callback(value, rowData, rowIndex); });
    }

    var authorityCheck = !authority || (typeof abp) != "undefined" && abp.auth.isGranted(authority);
    var value = arguments.callee.caller.arguments[0], rowData = arguments.callee.caller.arguments[1], rowIndex = arguments.callee.caller.arguments[2];
    if (JSON.stringify(rowData) != "{}") {
        return ((condition || condition == undefined) && authorityCheck) ? ("<a href='" + href + "' id='" + guid + "'>" + title + "</a>") : "";
    }

}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
$.extend($.fn.combobox.defaults, {
    editable: false,
    value: 0
});
$.extend($.fn.datebox.defaults, {
    editable: false
});
$.extend($.fn.datagrid.defaults, {
    fitColumns: true,
    width: '100%',
    fit: true,
    showFooter: true,
    //是否显示斑马线效果;同一行中显示数据;显示一个行号列;只允许选择一行
    striped: false, nowrap: true, rownumbers: true, singleSelect: true,
    pagination: true, pageSize: 20, pageList: [20, 50, 100, 150, 200], pageNumber: 1,
    loadFilter: function (data) {
        if (data instanceof Array) {
            return data;
        }
        var finalData = {};
        if (!data.rows && data.Result && data.Result.Rows) {
            finalData.rows = data.Result.Rows;
        }
        if (!data.footer && data.Result && data.Result.Footer) {
            var arr = [{}];
            arr.push(data.result.Footer);
            finalData.footer = arr;
        }
        if (!data.total && data.Result && data.Result.Total) {
            finalData.total = data.Result.Total;
        }
        return finalData;
    },
    method: "get"
});
$.extend($.fn.treegrid.defaults, {
    loadFilter: function (data) {
        if (data.result != undefined) {
            if (data.result.total == undefined && data.result.totalCount != undefined) {
                data.result.total = data.result.totalCount;
            }
            if (data.result.rows == undefined && data.result.items != undefined) {
                data.result.rows = data.result.items;
            }
            return data.result;
        }
        return data;
    },
    method: "get"
});
$.extend($.fn.datagrid.methods, {
    keyCtr: function (jq) {
        return jq.each(function () {
            var grid = $(this);
            grid.datagrid('getPanel').panel('panel').attr('tabindex', 1).bind('keydown', function (e) {
                switch (e.keyCode) {
                    case 37:// left
                        //if (!CE.hasMask()) {
                        var selected = grid.datagrid('getSelected');
                        var leftSelectIndex = 0;

                        if (!$(".datagrid-editable-input").length) {
                            return;
                        }

                        for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                            if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                leftSelectIndex = i;
                            }
                        }
                        var oldselect = getSelection($('.datagrid-editable-input').eq(leftSelectIndex).next().children('input').eq(0).get(0)).length;
                        var EditingRows = grid.datagrid('getEditingRowIndexs');
                        var arr = Object.keys(EditingRows);
                        for (var i = 0; i < arr.length; i++) {
                            grid.datagrid('endEdit', EditingRows[i]);
                        }
                        grid.datagrid('beginEdit', EditingRows[0]);
                        grid.datagrid('selectRow', EditingRows[0]);
                        if (!grid.datagrid('options').singleSelect) {
                            if (grid.datagrid('getEditingRowIndexs').length) {
                                var index = grid.datagrid('getEditingRowIndexs')[0];
                                var editors = grid.datagrid('getEditors', index);
                                if (editors[leftSelectIndex].target.val().length == 0 || (e.target.selectionStart == 0 && oldselect != editors[leftSelectIndex].target.val().length) || $('.datagrid-editable-input').eq(leftSelectIndex).next().children('span').length) {
                                    if (leftSelectIndex == 0) {
                                        leftSelectIndex = editors.length;
                                    }
                                    var delIndex = leftSelectIndex - 1;
                                    for (var i = 0; i < editors.length; i++) {
                                        if ($('.datagrid-editable-input').eq(delIndex).parent().parent().parent().parent().parent().parent().css("display") == 'none') {
                                            delIndex--;
                                        }
                                    }
                                    //$('.datagrid-editable-input').eq(leftSelectIndex - 1).next().children('input').eq(0).focus();
                                    //$('.datagrid-editable-input').eq(leftSelectIndex - 1).next().children('input').eq(0).select();
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus(function () {
                                        setTimeout(function () {
                                            $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).select();
                                        }, 0)

                                    });
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus();
                                } else if (oldselect == editors[rightSelectIndex].target.val().length) {
                                    set_text_value_position($('.datagrid-editable-input').eq(leftSelectIndex - 1).next().children('input').eq(0).attr("id"), 0)
                                } else {
                                    set_text_value_position($('.datagrid-editable-input').eq(leftSelectIndex).next().children('input').eq(0).attr("id"), getCursortPosition(e.target) - 1)
                                }
                            }
                        } else {
                            if (selected) {
                                var index = grid.datagrid('getRowIndex', selected);
                                var editors = grid.datagrid('getEditors', index);
                                if (editors[leftSelectIndex].target.val().length == 0 || (e.target.selectionStart == 0 && oldselect != editors[leftSelectIndex].target.val().length) || $('.datagrid-editable-input').eq(leftSelectIndex).next().children('span').length) {
                                    if (leftSelectIndex == 0) {
                                        leftSelectIndex = editors.length;
                                    }
                                    //for (var i = 0; i < editors.length; i++) {
                                    //    if (editors[i].target.parent().parent().parent().parent().parent().parent().css("display") == 'none') {
                                    //        delete editors[i]
                                    //    }
                                    //}
                                    var delIndex = leftSelectIndex - 1;
                                    for (var i = 0; i < editors.length; i++) {
                                        if ($('.datagrid-editable-input').eq(delIndex).parent().parent().parent().parent().parent().parent().css("display") == 'none') {
                                            delIndex--;
                                        }
                                    }
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus(function () {
                                        setTimeout(function () {
                                            $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).select();
                                        }, 0)

                                    });
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus();
                                } else if (oldselect == editors[leftSelectIndex].target.val().length) {
                                    set_text_value_position($('.datagrid-editable-input').eq(leftSelectIndex).next().children('input').eq(0).attr("id"), 0)
                                } else {
                                    set_text_value_position($('.datagrid-editable-input').eq(leftSelectIndex).next().children('input').eq(0).attr("id"), getCursortPosition(e.target) - 1)
                                }
                            }
                        }
                        //}
                        break;
                    case 38: // up
                        //if (!CE.hasMask()) {
                        var selected = grid.datagrid('getSelected');
                        if (!grid.datagrid('options').singleSelect) {
                            if (grid.datagrid('getEditingRowIndexs').length) {
                                var index = grid.datagrid('getEditingRowIndexs')[0];
                                var prevSelectIndex = 0;
                                for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                                    if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                        prevSelectIndex = i;
                                    }
                                }
                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);
                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }
                                var data = grid.datagrid('getData');
                                if (index == 0) {
                                    grid.datagrid('selectRow', data.rows.length - 1);
                                    grid.datagrid('beginEdit', data.rows.length - 1);
                                } else {
                                    grid.datagrid('selectRow', index - 1);
                                    grid.datagrid('beginEdit', index - 1);
                                }
                                $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).focus(function () {
                                    setTimeout(function () {
                                        $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).select();
                                    }, 0)

                                });
                                $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).focus();
                                editRow = index - 1;
                                if (editRow < 0) {
                                    editRow = data.rows.length - 1
                                }
                            } else {
                                return;
                            }
                        } else {
                            if (selected) {

                                var prevSelectIndex = 0;
                                var index = grid.datagrid('getRowIndex', selected);
                                for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                                    if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                        prevSelectIndex = i;
                                    }
                                }
                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);
                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }
                                var data = grid.datagrid('getData');
                                if (index == 0) {
                                    grid.datagrid('selectRow', data.rows.length - 1);
                                    grid.datagrid('beginEdit', data.rows.length - 1);
                                } else {
                                    grid.datagrid('selectRow', index - 1);
                                    grid.datagrid('beginEdit', index - 1);
                                }
                                //$('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).focus();
                                //$('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).select();
                                $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).focus(function () {
                                    setTimeout(function () {
                                        $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).select();
                                    }, 0)

                                });
                                $('.datagrid-editable-input').eq(prevSelectIndex).next().children('input').eq(0).focus();
                                editRow = index - 1;
                                if (editRow < 0) {
                                    editRow = data.rows.length - 1
                                }
                            } else {
                                var rows = grid.datagrid('getRows');
                                grid.datagrid('selectRow', rows.length - 1);

                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);

                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }

                                grid.datagrid('beginEdit', index - 1);

                            }
                        }
                        //}
                        break;
                    case 39:// right
                        //if (!CE.hasMask()) {
                        var selected = grid.datagrid('getSelected');
                        if (!grid.datagrid('options').singleSelect) {
                            if (grid.datagrid('getEditingRowIndexs').length) {
                                var index = grid.datagrid('getEditingRowIndexs')[0];
                            }
                        } else {

                            var index = grid.datagrid('getRowIndex', selected);
                        }
                        var rightSelectIndex = 0;

                        if (!$(".datagrid-editable-input").length) {
                            return;
                        }

                        for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                            if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                rightSelectIndex = i;
                            }
                        }
                        var oldSpace = e.target.selectionStart;
                        var oldselect = getSelection($('.datagrid-editable-input').eq(rightSelectIndex).next().children('input').eq(0).get(0)).length;
                        var EditingRows = grid.datagrid('getEditingRowIndexs');
                        var arr = Object.keys(EditingRows);
                        for (var i = 0; i < arr.length; i++) {
                            grid.datagrid('endEdit', EditingRows[i]);
                        }
                        grid.datagrid('beginEdit', EditingRows[0]);
                        grid.datagrid('selectRow', EditingRows[0]);
                        if (!grid.datagrid('options').singleSelect) {
                            if (grid.datagrid('getEditingRowIndexs').length) {
                                var editors = grid.datagrid('getEditors', index);     //获得当前行的编辑对象
                                if (e.target.selectionStart == editors[rightSelectIndex].target.val().length || $('.datagrid-editable-input').eq(rightSelectIndex).next().children('span').length || editors[rightSelectIndex].target.val().length == 0) {
                                    if (rightSelectIndex == editors.length - 1) {
                                        rightSelectIndex = -1;
                                    }
                                    var delIndex = rightSelectIndex + 1;
                                    for (var i = 0; i < editors.length; i++) {
                                        if ($('.datagrid-editable-input').eq(delIndex).parent().parent().parent().parent().parent().parent().css("display") == 'none') {
                                            if (delIndex == editors.length - 1) {
                                                delIndex = 0;
                                            } else {
                                                delIndex++;
                                            }
                                        }
                                    }
                                    //$('.datagrid-editable-input').eq(rightSelectIndex + 1).next().children('input').eq(0).focus();
                                    //$('.datagrid-editable-input').eq(rightSelectIndex + 1).next().children('input').eq(0).select();
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus(function () {
                                        setTimeout(function () {
                                            $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).select();
                                        }, 0)

                                    });
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus();
                                } else if (oldselect == editors[rightSelectIndex].target.val().length) {
                                    set_text_value_position($('.datagrid-editable-input').eq(rightSelectIndex).next().children('input').eq(0).attr("id"), oldselect)
                                } else {
                                    set_text_value_position($('.datagrid-editable-input').eq(rightSelectIndex).next().children('input').eq(0).attr("id"), oldSpace + 1)
                                }
                            }
                        } else {
                            if (selected) {
                                var editors = grid.datagrid('getEditors', index);     //获得当前行的编辑对象
                                if (e.target.selectionStart == editors[rightSelectIndex].target.val().length || $('.datagrid-editable-input').eq(rightSelectIndex).next().children('span').length || editors[rightSelectIndex].target.val().length == 0) {
                                    if (rightSelectIndex == editors.length - 1) {
                                        rightSelectIndex = -1;
                                    }
                                    var delIndex = rightSelectIndex + 1;
                                    for (var i = 0; i < editors.length; i++) {
                                        if ($('.datagrid-editable-input').eq(delIndex).parent().parent().parent().parent().parent().parent().css("display") == 'none') {
                                            if (delIndex == editors.length - 1) {
                                                delIndex = 0;
                                            } else {
                                                delIndex++;
                                            }

                                        }
                                    }
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus(function () {
                                        setTimeout(function () {
                                            $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).select();
                                        }, 0)

                                    });
                                    $('.datagrid-editable-input').eq(delIndex).next().children('input').eq(0).focus();
                                } else if (oldselect == editors[rightSelectIndex].target.val().length) {
                                    set_text_value_position($('.datagrid-editable-input').eq(rightSelectIndex).next().children('input').eq(0).attr("id"), oldselect)
                                } else {
                                    set_text_value_position($('.datagrid-editable-input').eq(rightSelectIndex).next().children('input').eq(0).attr("id"), oldSpace + 1)
                                }
                            }
                        }
                        //}
                        break;
                    case 40: // down
                        //if (!CE.hasMask()) {
                        var selected = grid.datagrid('getSelected');
                        if (!grid.datagrid('options').singleSelect) {
                            if (grid.datagrid('getEditingRowIndexs').length) {
                                var index = grid.datagrid('getEditingRowIndexs')[0];
                                var nextSelectIndex = 0;
                                for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                                    if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                        nextSelectIndex = i;
                                    }
                                }
                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);
                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }
                                var data = grid.datagrid('getData');
                                if (index == data.rows.length - 1) {
                                    grid.datagrid('selectRow', 0);
                                    grid.datagrid('beginEdit', 0);
                                } else {
                                    grid.datagrid('selectRow', index + 1);
                                    grid.datagrid('beginEdit', index + 1);
                                }
                                //$('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus();
                                //$('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).select();
                                $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus(function () {
                                    setTimeout(function () {
                                        $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).select();
                                    }, 0)

                                });
                                $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus();
                                editRow = index + 1;
                                if (editRow > data.rows.length - 1) {
                                    editRow = 0
                                }
                            } else {
                                return;
                            }
                        } else {
                            if (selected) {
                                var nextSelectIndex = 0;
                                var index = grid.datagrid('getRowIndex', selected);
                                for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
                                    if ($(".datagrid-editable-input").eq(i).next().hasClass("textbox-focused")) {
                                        nextSelectIndex = i;
                                    }
                                }
                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);
                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }
                                var data = grid.datagrid('getData');
                                if (index == data.rows.length - 1) {
                                    grid.datagrid('selectRow', 0);
                                    grid.datagrid('beginEdit', 0);
                                } else {
                                    grid.datagrid('selectRow', index + 1);
                                    grid.datagrid('beginEdit', index + 1);
                                }
                                $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus(function () {
                                    setTimeout(function () {
                                        $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).select();
                                    }, 0)

                                });
                                $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus();
                                editRow = index + 1;
                                if (editRow > data.rows.length - 1) {
                                    editRow = 0
                                }
                            } else {
                                grid.datagrid('selectRow', 0);

                                var EditingRows = grid.datagrid('getEditingRowIndexs');
                                var arr = Object.keys(EditingRows);
                                for (var i = 0; i < arr.length; i++) {
                                    grid.datagrid('endEdit', EditingRows[i]);
                                }

                                grid.datagrid('beginEdit', index + 1);
                            }
                        }
                        //}
                        break;
                }
            });
        });
    },
    getEditingRowIndexs: function (jq) {
        var rows = $.data(jq[0], "datagrid").panel.find('.datagrid-row-editing');
        var indexs = [];
        rows.each(function (i, row) {
            var index = row.sectionRowIndex;
            if (indexs.indexOf(index) == -1) {
                indexs.push(index);
            }
        });
        return indexs;
    }
});
//扩展load方法重新渲染easyui元素
$.fn.extend({
    Load: function (url, fun) {
        var $this = this;
        $(this).load(url, function (resp, status, xhr) {
            $.parser.parse($this);
            fun(resp, status, xhr);
        });
    }
});
//格式化日期
$.fn.datebox.defaults.parser = function (s) {
    var t = Date.parse(s);
    if (!isNaN(t)) {
        return new Date(t);
    } else {
        return new Date();
    }
}
$.fn.datebox.defaults.formatter = function (date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    month = month < 10 ? '0' + month : month;
    day = day < 10 ? '0' + day : day;

    return year + "-" + month + "-" + day;
};
function set_text_value_position(obj, spos) {
    var tobj = document.getElementById(obj);
    if (spos < 0)
        spos = tobj.value.length;
    if (tobj.setSelectionRange) { //兼容火狐,谷歌
        setTimeout(function () {
            tobj.setSelectionRange(spos, spos);
            tobj.focus();
        }
            , 0);
    } else if (tobj.createTextRange) { //兼容IE
        var rng = tobj.createTextRange();
        rng.move('character', spos);
        rng.select();
    }
}

//获取光标位置函数    
function getCursortPosition(ctrl) {
    var CaretPos = 0;   // IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart == '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}

//设置光标位置函数
function set_text_value_position(obj, spos) {
    var tobj = document.getElementById(obj);
    if (spos < 0)
        spos = tobj.value.length;
    if (tobj.setSelectionRange) { //兼容火狐,谷歌
        setTimeout(function () {
            tobj.setSelectionRange(spos, spos);
            tobj.focus();
        }
            , 0);
    } else if (tobj.createTextRange) { //兼容IE
        var rng = tobj.createTextRange();
        rng.move('character', spos);
        rng.select();
    }
}


//获取光标位置函数    
function getCursortPosition(ctrl) {
    var CaretPos = 0;   // IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart == '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}

function checkIpt(target) {
    var t = $(target);
    var options = {};

    var s = $.trim(t.attr('data-info'));
    if (s) {
        if (s.substring(0, 1) != '{') {
            s = '{' + s + '}';
        }
        options = (new Function('return ' + s))();
    }
    return options;
}

function TabsChange(modalOrDiv, index, isShow) {
    if (modalOrDiv) {
        var tabs = $tabs("." + modalOrDiv + " .tab-head span");
        var contents = $tabs("." + modalOrDiv + " .tab-content").children("div");
    } else {
        var tabs = $tabs(".tab-head span");
        var contents = $tabs(".tab-content").children("div");
    }


    tabs.eq(0).addClass('selected');
    tabs.eq(0).siblings().removeClass("selected");
    contents.eq(0).addClass('show');
    contents.eq(0).siblings().removeClass('show');
    if (modalOrDiv) {
        $("#" + modalOrDiv).modal("show");
    }


    (function changeTab(tab) {
        for (var i = 0, len = tabs.length; i < len; i++) {
            if (isShow) {
                tabs.eq(i).click(showTab);
            } else {
                if (i != index) {
                    tabs.eq(i).click(showTab);
                } else {
                    tabs.eq(i).unbind("click");
                }
            }
        }
    })();

    function showTab() {
        for (var i = 0, len = tabs.length; i < len; i++) {
            if (i == $(this).index()) {
                tabs.eq(i).addClass('selected');
                contents.eq(i).addClass('show');
            } else {
                tabs.eq(i).removeClass("selected");
                contents.eq(i).removeClass('show');
            }
        }
    }
}