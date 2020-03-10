//Skin changer
function skinChanger() {
    $('.right-sidebar .demo-choose-skin li').on('click', function () {
        var currentTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
        $('.right-sidebar .demo-choose-skin li').removeClass('active');

        var $selected = $(this);
        $selected.addClass('active');
        var selectedTheme = $selected.data('theme');

        $('body')
            .removeClass('theme-' + currentTheme)
            .addClass('theme-' + selectedTheme);

        //Change theme settings on the server
        abp.services.app.configuration.changeUiTheme({
            theme: selectedTheme
        });
    });
}

//Skin tab content set height and show scroll
function setSkinListHeightAndScroll() {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.demo-choose-skin');

    $el.slimScroll({ destroy: true }).height('auto');
    $el.parent().find('.slimScrollBar, .slimScrollRail').remove();

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Setting tab content set height and show scroll
function setSettingListHeightAndScroll() {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.right-sidebar .demo-settings');

    $el.slimScroll({ destroy: true }).height('auto');
    $el.parent().find('.slimScrollBar, .slimScrollRail').remove();

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}
//nav tab
function clickLeftNav(ids, names, urls, data, refresh, that) {
    if (that) {
        if ($(that).children("a").text() == "首页") {
            $("#HomeLi").addClass("active");
            $("#Home").addClass("active");
            $("#HomeLi").siblings().removeClass("active");
            $("#Home").siblings().removeClass("active");
            return;
        }
    }
    $("#HomeLi").removeClass("active");
    $("#Home").removeClass("active");
    var item = {
        'id': ids, 'data': data, 'name': names, 'url': urls, 'closable': true
    };
    $("#navContent-loading").css("display", "block");
    closableTab.addTab(item, refresh);
    bindRightContextMenu("tab_seed_" + ids);
    $('#nav-tabs-tabitem li').click(function () {
        Adaptation();
    })
}
function bindRightContextMenu(id) {
    $('#' + id).contextmenu({
        target: '#context-menu',
        onItem: function (context, e) {
            if ($(e.target).text() == "关闭当前") {
                closableTab.closeTab($(context).find("i"));
            } else if ($(e.target).text() == "关闭其他") {
                for (var i = 1; i < $(context).siblings().length; i++) {
                    if ($(context).siblings().eq(i).attr("id") != "HomeLi") {
                        closableTab.closeTab($(context).siblings().eq(i).find("i"));
                        i--;
                    }
                }
            } else {
                for (var i = 1; i < $(context).siblings().length; i++) {
                    if ($(context).siblings().eq(i).attr("id") != "HomeLi") {
                        closableTab.closeTab($(context).siblings().eq(i).find("i"));
                        i--;
                    }
                }
                closableTab.closeTab($(context).find("i"));
            }
        }
    });
}

//Activate notification and task dropdown on top right menu
function activateNotificationAndTasksScroll() {
    $('.navbar-right .dropdown-menu .body .menu').slimscroll({
        height: '254px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

function Adaptation() {
    var activeId = $("#tab-content-tabitem > .active").attr("id");
    if (activeId == 'tab_container_base_Erp_GoodsInfo') {
        $("#GoodsInfoTableList").datagrid("resize")
    }
    var title = $("#" + activeId + " .header").height();
    var freamTop = parseInt($(".content_nav").css("marginTop").replace('px', ''));
    var tabsHeight = $("#nav-tabs-tabitem").height();
    var searchFormHeight = 0;
    if ($("#" + activeId).find(".SearchForm").css("display") != "none") {
        searchFormHeight = $("#" + activeId).find(".SearchForm").height()
    }
    var tableHeights = $(window).height() - searchFormHeight - 40 - title - tabsHeight - freamTop - 20;
    if ($("#" + activeId + " .input-note").length) {
        tableHeights = tableHeights - $("#" + activeId + " .input-note").height();
    }
    if ($("#" + activeId + " .flexTable").length) {
        tableHeights = tableHeights - $("#" + activeId + " .flexTable").height();
    }
    $("#" + activeId + " .TableAdaptation").css("height", tableHeights);


    $("#" + activeId + " .TableAdaptation .panel-body").css('height', tableHeights - 2);
    if ($("#" + activeId + " .TableAdaptation .datagrid-footer").eq(0).height() == 0) {
        if ($("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).length == 0) {
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(0).css('height', tableHeights - 3 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(0).css('height', tableHeights - 83 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(1).css('height', tableHeights - 3 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(1).css('height', tableHeights - 83 - searchFormHeight);
        } else {
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(0).css('height', tableHeights - 38);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(0).css('height', tableHeights - 118);
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(1).css('height', tableHeights - 38);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(1).css('height', tableHeights - 118);
        }
    } else {
        if ($("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).length == 0) {
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(0).css('height', tableHeights - 38 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(0).css('height', tableHeights - 118 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(1).css('height', tableHeights - 38 - searchFormHeight);
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(1).css('height', tableHeights - 118 - searchFormHeight);
        } else {
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(0).css('height', tableHeights - 40 - $("#" + activeId + " .TableAdaptation .datagrid-footer").eq(0).height() - $("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).height());
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(0).css('height', tableHeights - 3 - $("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).height());
            $('#' + activeId + ' .TableAdaptation .datagrid-body').eq(1).css('height', tableHeights - 40 - $("#" + activeId + " .TableAdaptation .datagrid-footer").eq(0).height() - $("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).height());
            $('#' + activeId + ' .TableAdaptation .datagrid-view').eq(1).css('height', tableHeights - 3 - $("#" + activeId + " .TableAdaptation .datagrid-pager").eq(0).height());
        }
    }

    $(".SearchForm .publicIpts").on('click', function (e) {
        e.stopPropagation();
        $(".SearchForm .publicIpts").removeClass("publicIptsHover");
        $(this).addClass("publicIptsHover");
    })
    $(document).on('click', function (e) {
        if (e.target != $('.SearchForm .publicIpts')) {
            $('.SearchForm .publicIpts').removeClass('publicIptsHover');
        }
    });
}

function intToolbar(tableId) {
    var pulicToolbar = [];
    var argArr = Array.prototype.slice.call(arguments, 1);
    var toolbarAdd = {
        text: '添加', iconCls: 'iconfont icontianjia', handler: function () {
            DG.insertRow(tableId);
        }
    };
    var toolbarInsert = {
        text: '插入', iconCls: 'iconfont iconcharu', handler: function () {
            DG.insertInSeleRow(tableId);
        }
    };
    var toolbarDel = {
        text: '删除', iconCls: 'iconfont iconshanchu', handler: function () {

            DG.DeleteRow(tableId);
        }
    };
    var toolbarSearch = {
        id: 'dgSearchHref', text: '<div class="dgSearch"><input id="iptSearch" onchange="DG.watchSearchInput(this)" type="text" autocomplete="off" placeholder="请输入关键字"><i class="iconfont iconsousuo" onclick="DG.searchPosition(\'' + tableId + '\')"></i></div>'
    };
    argArr.forEach(function (item) {
        if (item == "add") {
            pulicToolbar.push(toolbarAdd);
        } else if (item == "insert") {
            pulicToolbar.push(toolbarInsert);
        } else if (item == "del") {
            pulicToolbar.push(toolbarDel);
        } else if (item == "search") {
            pulicToolbar.push(toolbarSearch);
        }
    })
    return pulicToolbar;
}

function tableShow(ObjId, check) {
    var rows = $(ObjId).datagrid('getFooterRows');
    if (rows) {
        var keyArr = Object.keys(rows[1]);
        for (var i = 0; i < keyArr.length; i++) {
            rows[0][keyArr[i]] = DG.bottomAmount("#RecoverysTableList", keyArr[i]);
        }
        $(ObjId).datagrid('reloadFooter');
    }
    var activeId = $("#tab-content-tabitem > .active").attr("id");
    setTimeout(function () {
        $(ObjId).prev().prev().find('.datagrid-header-rownumber').html("<span class='iconfont icon-shezhi' style='font-size: 18px;width:18px;height:18px;' onclick='createColumnMenu(&quot;" + ObjId + "&quot;)'></span>");
    }, 10);


    setTimeout(function () {
        //$('.datagrid-ftable .datagrid-row .datagrid-td-rownumber ').html('<div class="datagrid-cell-rownumber" style="font-weight: bold;">合计</div>');
        if ($('#' + activeId + ' .datagrid-ftable .datagrid-row .datagrid-td-rownumber ').length == 1) {
            $('#' + activeId + ' .datagrid-ftable .datagrid-row .datagrid-td-rownumber:first ').html('<div class="datagrid-cell-rownumber" style="font-weight: bold;font-size:12px;">合计</div>');
        }
        if ($('#' + activeId + ' .datagrid-ftable .datagrid-row .datagrid-td-rownumber ').length > 1) {
            $('#' + activeId + ' .datagrid-ftable .datagrid-row .datagrid-td-rownumber:first ').html('<div class="datagrid-cell-rownumber" style="font-weight: bold;font-size:12px;">页合计</div>');
            $('#' + activeId + ' .datagrid-ftable .datagrid-row .datagrid-td-rownumber:last ').html('<div class="datagrid-cell-rownumber" style="font-weight: bold;font-size:12px;">总合计</div>');
        }
    }, 100);

    if (check) {
        $(ObjId).parent().find('.datagrid-header-check').children('input').attr('disabled', true);

    }
}

//表头设置标题事件
function createColumnMenu(dataGridObj) {
    cmenu = $('<div/>').appendTo('body');
    cmenu.menu({

        onClick: function (item) {
            if (item.iconCls == 'icon-ok') {
                $(dataGridObj).datagrid('hideColumn', item.name);
                cmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-empty'
                });
            } else {
                $(dataGridObj).datagrid('showColumn', item.name);
                cmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-ok'
                });
            }
            saveGridColumns(dataGridObj);
        }
    });

    var fields = $(dataGridObj).datagrid('getColumnFields');
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        // console.log(field);
        var col = $(dataGridObj).datagrid('getColumnOption', field);

        if (col.visiableOnMenu == undefined || col.visiableOnMenu) {
            if (!col.hidden) {
                cmenu.menu('appendItem', {
                    text: col.title,
                    name: field,
                    iconCls: 'icon-ok'
                });
            } else {
                cmenu.menu('appendItem', {
                    text: col.title,
                    name: field,
                    iconCls: 'icon-empty'
                });
            }
        }
    }
    //兼容IE9 IE10 改变表格点击 表头出现位置
    var theEvent = window.event || arguments.callee.caller.arguments[0];
    cmenu.menu('show', {
        left: theEvent.clientX + 41 + 'px',
        top: theEvent.clientY + 17 + 'px'
    });

}

function saveGridColumns(dataGridObj) {
    var columns = $(dataGridObj).datagrid('options').columns;
    localStorage[$(dataGridObj)[0].id] = JSON.stringify(columns);
}

function InitDatePicker(dPObj, index, fun, isCallBack) {
    dPObj.find("li:eq(" + index + ")").click();
    if (isCallBack == false) {
    }
    else
        fun.click();
    $(dPObj).find("li").click(function () {
        fun.click();
    })
}
function dateSelect() {
    $('.content_nav').delegate('.searchli-group li', "click", function () {
        var activeId = $("#tab-content-tabitem > .active").attr("id");
        $(this).addClass("addbackground").siblings().removeClass("addbackground");
        var di = $(this).attr("data-index");
        var day = new Date();
        var nowDay = day.getDate();//当前日
        var nowMonth = day.getMonth();           //当前月
        var nowYear = day.getFullYear();             //当前年
        var nowDayOfWeek = day.getDay();
        if (nowDayOfWeek == 0) {
            nowDayOfWeek = 7;
        }//今天本周的第几天
        var lastMonthDate = new Date();  //上月日期
        lastMonthDate.setDate(1);
        lastMonthDate.setMonth(lastMonthDate.getMonth() - 1);
        var lastYear = lastMonthDate.getYear();
        var lastMonth = lastMonthDate.getMonth();
        switch (di) {
            case "1":
                day.setDate(day.getDate() - 1)
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(day));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(day));
                break;
            case "2":
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(day));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(day));
                break;
            case "3":
                var getWeekStartDate = new Date(nowYear, nowMonth, nowDay - (nowDayOfWeek - 1));
                var getWeekEndDate = new Date(nowYear, nowMonth, nowDay + (7 - nowDayOfWeek));
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(getWeekStartDate));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(getWeekEndDate));
                break;
            case "4":
                var getUpWeekStartDate = new Date(nowYear, nowMonth, nowDay - nowDayOfWeek - 6);
                var getUpWeekEndDate = new Date(nowYear, nowMonth, nowDay + (7 - nowDayOfWeek - 7));
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(getUpWeekStartDate));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(getUpWeekEndDate));
                break;
            case "5":
                var getMonthStartDate = new Date(nowYear, nowMonth, 1);
                var getMonthEndDate = new Date(nowYear, nowMonth, getMonthDays(nowYear, nowMonth));
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(getMonthStartDate));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(getMonthEndDate));
                break;
            case "6":
                if (lastMonth == 11) {
                    nowYear = nowYear - 1
                }
                var getLastMonthStartDate = new Date(nowYear, lastMonth, 1);
                var getLastMonthEndDate = new Date(nowYear, lastMonth, getMonthDays(nowYear, lastMonth));
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(getLastMonthStartDate));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(getLastMonthEndDate));
                break;
            case "7":
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(day));
                day.setDate(day.getDate() - 91);
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(day));
                break;
            case "8":
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(new Date(nowYear, getQuarterStartMonth(nowMonth), 1)));
                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(day));
                break;
            case "9":
                $('#' + activeId + ' .StartDate').datebox('setValue', formatterDate(new Date(nowYear, 0, 1)));

                $('#' + activeId + ' .EndDate').datebox('setValue', formatterDate(day));
                break;
            default:
                $('#StartDate').datebox('setValue', '2017-04-01');
        }
    });
    $('.content_nav').delegate(".containerNode", "click", function () {
        var activeId = $("#tab-content-tabitem > .active").attr("id");
        if ($(this).attr('data-id') == 1) {
            $('.zhankai').show();
            $('.shouqi').hide();
            $('#' + activeId + ' .SearchForm').hide();

            $(this).attr('data-id', 0);
            Adaptation();
        } else {
            $('.zhankai').hide();
            $('.shouqi').show();
            $('#' + activeId + ' .SearchForm').show();

            $(this).attr('data-id', 1);
            Adaptation();
        }

    });
}
function formatterDate(date) {
    var myyear = date.getFullYear();
    var mymonth = date.getMonth() + 1;
    var myweekday = date.getDate();

    if (mymonth < 10) {
        mymonth = "0" + mymonth;
    }
    if (myweekday < 10) {
        myweekday = "0" + myweekday;
    }
    return (mymonth + "/" + myweekday + "/" + myyear);
}
//获得某月的天数
function getMonthDays(nowYear, myMonth) {
    var monthStartDate = new Date(nowYear, myMonth, 1);
    var monthEndDate = new Date(nowYear, myMonth + 1, 1);
    var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
    return days;
}
function getQuarterStartMonth(nowMonth) {
    var quarterStartMonth = 0;
    if (nowMonth < 3) {
        quarterStartMonth = 0;
    }
    if (2 < nowMonth && nowMonth < 6) {
        quarterStartMonth = 3;
    }
    if (5 < nowMonth && nowMonth < 9) {
        quarterStartMonth = 6;
    }
    if (nowMonth > 8) {
        quarterStartMonth = 9;
    }
    return quarterStartMonth;
}
function subindexMenu() {
    $("#js-menu").find("li").hover(function () {
        $(this).css("z-index", "9999");
        $(this).find("a").addClass("on");
        $(this).find(".listbox").show();
    }, function () {
        $(this).removeAttr("style");
        $(this).find("a").removeClass("on");
        $(this).find(".listbox").hide();
    });
    for (var i = 0; i < $('.listbox').length; i++) {
        $('.listbox').eq(i).css('width', parseInt($('.listbox').eq(i).children('dl').length * 188) + 'px');
        $('.listbox').eq(i).children('dl').css('width', (100 / $('.listbox').eq(i).children('dl').length) + '%');

        $('.listbox').eq(i).children('dl').css('height', parseInt($('.listbox').eq(i).height()) + 'px');
    }
}
//展示的数字处理,1表示价格，2表示数量
function FormatDecima(value, valueType) {
    if (!value) {
        value = 0;
    }
    else {
        value = parseFloat(value);
    }
    if (localStorage["LastPlace"] == 1) {
        if (valueType == 1) {
            return parseFloat(value.toFixed(localStorage["DecimalDigits"]));
        }
        else {
            return parseFloat(value.toFixed(localStorage["CountDigits"]));
        }
    } else if (localStorage["LastPlace"] == 0) {
        if (valueType == 1) {
            return value.toFixed(localStorage["DecimalDigits"]);
        } else {
            return value.toFixed(localStorage["CountDigits"]);
        }
    } else {
        return value;
    }
}

(function ($) {

    $(function () {
        subindexMenu();
        dateSelect();
        activateNotificationAndTasksScroll();
        setSkinListHeightAndScroll();
        setSettingListHeightAndScroll();
        $(window).resize(function () {
            setSkinListHeightAndScroll();
            setSettingListHeightAndScroll();
            Adaptation();
        });

    });

})(jQuery);