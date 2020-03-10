var closableTab = {
    //frame加载完成后设置父容器的高度，使iframe页面与父页面无缝对接
    frameLoad: function (frame) {

        var mainheight = $(frame).contents().find('body').height();
        $(frame).parent().height(mainheight);
    },

    //添加tab
    addTab: function (tabItem, refresh) { //tabItem = {id,name,url,closable}

        var id = "tab_seed_" + tabItem.id;
        var container = "tab_container_" + tabItem.id;
        if (!$('#' + id)[0]) {
            if ($('#nav-tabs-tabitem>li').length > 10) {
                abp.message.warn("最多可以同时打开10个菜单，请先关闭不需要的菜单!");
                $("#navContent-loading").css("display", "none");
                return;
            }
            var li_tab = '<li role="presentation" class="context" id="' + id + '"  data-toggle="context" data-target="#context-menu"><a href="#' + container + '"  role="tab" data-toggle="tab" style="position: relative;padding:6px 25px 6px 15px;border-radius:3px;">' + tabItem.name;
            if (tabItem.closable) {
                li_tab = li_tab + '<i class="glyphicon glyphicon-remove small" tabclose="' + id + '" style="position: absolute;right:10px;top: 9px;cursor:pointer;"  onclick="closableTab.closeTab(this)"></i></a></li> ';
            } else {
                li_tab = li_tab + '</a></li>';
            }

            var tabpanel = '<div role="tabpanel" class="tab-pane" id="' + container + '" style="width: 100%;height:100%;">' +
                '<div id="tab_frame_' + id + '" style="overflow-x: hidden; overflow-y: hidden;width:100%;height: 100%"></div>' +
                '</div>';
            //var tabpanel = '<div role="tabpanel" class="tab-pane" id="' + container + '" style="width: 100%;">' +
            //    '<iframe src="' + tabItem.url + '" id="tab_frame_2" frameborder="0" style="overflow-x: hidden; overflow-y: hidden;width:100%;height: 100%"  onload="closableTab.frameLoad(this)"></iframe>' +
            //    '</div>';


            $('#nav-tabs-tabitem').append(li_tab);
            $('#tab-content-tabitem').append(tabpanel);
            $("#tab_frame_" + id).Load(tabItem.url, function (response, status, xhr) {
                $("#navContent-loading").css("display", "none");

                if (xhr.status == 401) {
                    abp.message.confirm("登陆信息已失效，请重新登陆", "错误", function () {
                        location.href = "/Account/Login";
                    });
                } else {
                    initMyComponent();
                    Adaptation();
                }

            })
        } else {
            if (refresh) {
                $("#tab_frame_" + id).Load(tabItem.url, function (response, status, xhr) {
                    $("#navContent-loading").css("display", "none");

                    initMyComponent();
                    Adaptation();
                })
            }
            $("#navContent-loading").css("display", "none");
        }
        $("li[id^=tab_seed_]").removeClass("active");
        $("div[id^=tab_container_]").removeClass("active");
        $("#" + id).data("param", tabItem.data);
        $("#" + container).data("pageData", tabItem);
        $("#" + id).addClass("active");
        $("#" + container).addClass("active");
    },

    //关闭tab
    closeTab: function (item) {
        var val = $(item).attr('tabclose');
        var containerId = "tab_container_" + val.substring(9);

        if ($('#' + containerId).hasClass('active')) {
            $('#' + val).prev().addClass('active');
            $('#' + containerId).prev().addClass('active');
        }


        $("#" + val).remove();
        $("#" + containerId).remove();
        Adaptation();
    }
}