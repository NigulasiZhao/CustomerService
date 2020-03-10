(function () {
    $(function () {
        var OrderId = "";//编辑页面时拿该ID
        var PrecisionOfPrice = 4;//后台配置小数位数，如果没有则暂为4
        //var _saleOrderService = abp.services.app.saleOrder;
        //var _serialNumberService = abp.services.app.serialNumber;
        //var _$form = $tabs('form[name=form1]');
        var editRow = 0;
        var AddProcessFlag = false;
        var tableId = "#ChanePriceList";

        //$tabs(".NewSaleOrderSaveContinueButton").click(function () {
        //    saveFn(true);
        //})
        //$tabs('.NewSaleOrderSaveButton').click(function (e) {
        //    saveFn();
        //});

        //$tabs(".querySearch").click(function () {
        //    console.log($(this).parent().serialize())
        //})
        //$tabs(".customer-search").click(function () {
        //    var Search = $(this).parent().serializeObject();
        //    DG.setQueryParamsRequest($tabs("#customerForSaleList"), Search)
        //})

        //$tabs(".selectCustomer").each(function () {
        //    $(this).textbox($.extend({}, __customerOption, {
        //        OnSelectedCustomer: function (data) {
        //            $tabs('form[name=form1]').form('load', { payWay: data.payWay, accountDate: data.accountDate });
        //        }
        //    }));
        //});

        $tabs(tableId).datagrid("keyCtr");
        $tabs(tableId).datagrid
            ({
                url: "",
                //分页相关属性
                sortName: 'GoodsCode', sortOrder: 'asc',
                onClickCell: function (index, field) {
                    DG.onClickCell(index, field, this);
                    editRow = index;
                    $tabs(tableId).datagrid('beginEdit', index);
                    DG.selectClickCell(field);
                },
                onLoadSuccess: function (data) {
                    tableShow(tableId);
                    editorCheck();
                },
                onBeginEdit: function (index, row) { },
                onAfterEdit: function (index, row, changes) {
                    DG.calculationAmount(tableId, index, row, changes);
                    var num = DG.bottomAmount(tableId, "goodsAmount");
                    $tabs(".spanAmount").text(num);
                },
                columns: [[
                    {
                        field: 'goodsName', title: '分类名称', sortable: true, width: 150, align: 'center', editor: {
                            type: 'goods', options: {
                                getGoodsQueryParam: function () {
                                    //var activeId = $('#tab-content-tabitem .active').eq(0).attr('id');
                                    //if ($('#' + activeId + ' input[name=customerId]').val() == "" || $('#' + activeId+' input[name=warehouseId]').val() == "") {
                                    //    abp.message.error("请先选择客户和仓库", "提示");
                                    //    return false;
                                    //}
                                    return { unionId: $tabs("[name='customerId']").val(), warehousesId: $tabs("[name='warehouseId']").val(), GoodsPriceType: 2 };
                                },
                                OnSelectedGoods: function (goods) {
                                    DG.goodsRowsWindow(tableId, goods, false);
                                }
                            }
                        }, styler: function (value, row, index) {
                            return { class: 'eaditableBackground' }
                        }
                    },
                    {
                        field: 'goodsCode', title: '分类编码', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsArticleNumber', title: '分类货号', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'BasicSalePrice', title: '源基础售价', sortable: true, width: 100, align: 'left',
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'NewBasicSalePrice', title: '调整基础售价', sortable: true, width: 100, align: 'left', editor: {},
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'Level1Price', title: '源一级价', sortable: true, width: 100, align: 'right',
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'NewLevel1Price', title: '调整一级价', sortable: true, width: 100, align: 'right', editor: {},
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'Level2Price', title: '源二级价', sortable: true, width: 100, align: 'right',
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'NewLevel2Price', title: '调整二级价', sortable: true, width: 100, align: 'right', editor: {},
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        field: 'Level3Price', title: '源三级价', sortable: true, width: 100, align: 'right',
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                    {
                        field: 'NewLevel3Price', title: '调整三级价', sortable: true, width: 100, align: 'right', editor: {},
                        formatter: function (value, rowData, rowIndex) {
                            if (value != undefined && value != '') {
                                return FormatDecima(value, 1);
                            }
                            else {
                                return '';
                            }
                        },
                    },
                ]],
                toolbar: intToolbar(tableId, "add", "insert", "del", "search")
            });
        for (var i = 0; i < 10; i++) {
            //添加空行
            DG.insertRow($tabs(tableId));
        }
        $tabs(tableId).datagrid('selectRow', 0);

        //全选
        $tabs(".LabRadAll input[type=checkbox]").click(function () { AllCheck(this) });
        function AllCheck(obj) {
            var colums = ['NewBasicSalePrice', 'NewLevel1Price', 'NewLevel2Price', 'NewLevel3Price'];
            if (obj.checked == false) {
                $tabs("input[type='checkbox']").each(function (index, item) {
                    item.checked = false;
                });
                for (var i = 0; i < colums.length; i++) {
                    var e = $tabs(tableId).datagrid('getColumnOption', colums[i]);
                    e.editor = {};
                    e.styler = function () {
                        return { };
                    }
                }
            }
            else {
                $tabs("input[type='checkbox']").each(function (index, item) {
                    item.checked = true;
                });
                for (var i = 0; i < colums.length; i++) {
                    var e = $tabs(tableId).datagrid('getColumnOption', colums[i]);
                    e.editor = {
                        type: 'numberbox',
                        options: {
                            align: 'right',
                            precision: PrecisionOfPrice,
                            value: 0,
                            min: 0,
                            onChange: function (newValue, oldValue) {
                                newValue = '';
                            },
                        }
                    };
                    e.styler = function () {
                        return { class: 'eaditableBackground' };
                    }
                }
            }
            var allRows = $tabs(tableId).datagrid("getRows");
            for (var i = 0; i < allRows.length; i++) {
                $tabs(tableId).datagrid('refreshRow', i);
            }
        }
        //选中时可编辑
        $tabs(".LabRad input[type=checkbox]").click(function () { checkEditor(this) });
        function checkEditor(obj, name) {
            $tabs(tableId).datagrid('endEdit', editRow);
            if (name == "undefine" || name == "" || name == null) {
                name = obj.name
            }
            var e = $tabs(tableId).datagrid('getColumnOption', name + 'Price');
            if (obj.checked == false) {
                e.editor = {};
                e.styler = function () {
                    return {};
                }
            } else {
                e.editor = {
                    type: 'numberbox',
                    options: {
                        align: 'right',
                        precision: PrecisionOfPrice,
                        value: 0,
                        min: 0,
                        onChange: function (newValue, oldValue) {
                            newValue = '';
                        },
                    }
                };
                e.styler = function () {
                    return { class: 'eaditableBackground'};
                }
            }
            var allRows = $tabs(tableId).datagrid("getRows");
            for (var i = 0; i < allRows.length; i++) {
                $tabs(tableId).datagrid('refreshRow',i);
            }
            
        }
        //编辑页面选中相应的checkbox
        
        function editorCheck() {
            if (OrderId) {
                var rows = $tabs(tableId).datagrid('getRows');
                var colums = ['NewBasicSalePrice', 'NewLevel1Price', 'NewLevel2Price', 'NewLevel3Price'];
                if (rows.length > 0) {
                    if (rows[0].NewBasicSalePrice != null) {
                        $tabs('.NewBasicSale').attr("checked", "checked");
                        checkEditor($tabs('.NewBasicSale'), 'NewBasicSale');
                    }
                    if (rows[0].NewLevel1Price != null) {
                        $tabs('.NewLevel1').attr("checked", "checked");
                        checkEditor($tabs('.NewLevel1'), 'NewLevel1');
                    }
                    if (rows[0].NewLevel2Price != null) {
                        $tabs('.NewLevel2').attr("checked", "checked");
                        checkEditor($tabs('.NewLevel2'), 'NewLevel2');
                    }
                    if (rows[0].NewLevel3Price != null) {
                        $tabs('.NewLevel3').attr("checked", "checked");
                        checkEditor($tabs('.NewLevel3'), 'NewLevel3');
                    }
                }
            }
        }
    });
})();