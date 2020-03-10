
    //var activeId = $("#tab-content-tabitem > .active").attr("id");
    //var $ = JQuery.noConflict();
    //var jq = jQuery.noConflict();
    //var $ = function (params) {
    //    var activeId = $("#tab-content-tabitem > .active").attr("id");
    //    //var $$ = jQuery.noConflict();
    //    return jq("#" + activeId + " " + params)
    //}

    var $tabs = function (params) {
        //var jq = jQuery.noConflict();
        var activeId = $("#tab-content-tabitem > .active").attr("id");
        return $("#" + activeId + " " + params)
    }

