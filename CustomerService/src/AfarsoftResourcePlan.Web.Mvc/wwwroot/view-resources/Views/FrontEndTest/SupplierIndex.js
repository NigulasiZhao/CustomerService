(function () {
    $(function () {
        var _saleOrderService = abp.services.app.saleOrder;
        var _serialNumberService = abp.services.app.serialNumber;
        var _$form = $tabs('form[name=form1]');
        var editRow = 0;
        var AddProcessFlag = false;
        var tableId = "#NewSaleOrderList";

        //_serialNumberService.getOrderCode({ orderType: 5, organizationId: $("[name='organizationId']").val() }).done(function (data) {
        //    $tabs('#NewSaleOrderCode').text(data);
        //    $('#NewSaleOrderCode').text(data);
        //}).always(function () {
        //}).fail(function (data) {
        //});

        $tabs('.NewSaleOrderSaveButton').click(function (e) {
            if (AddProcessFlag) {
                abp.message.error("单据已提交", '提示');
                return;
            }
            var FormInfo = _$form.serializeFormToObject();
            if (FormInfo.customerId == "" || FormInfo.customerName == "") {
                abp.message.error("请选择客户", '提示');
                return;
            }
            if (FormInfo.SalerId == "" || FormInfo.SalerName == "") {
                abp.message.error("请选择销售员", '提示');
                return;
            }
            if (FormInfo.organizationId == "" || FormInfo.organizationName == "") {
                abp.message.error("请选择机构", '提示');
                return;
            }
            if (FormInfo.warehouseId == "" || FormInfo.warehouseName == "") {
                abp.message.error("请选择仓库", '提示');
                return;
            }
            if (FormInfo.orderDate == "") {
                abp.message.error("请选择单据日期", '提示');
                return;
            }
            if (FormInfo.orderDeliveryDate == "") {
                abp.message.error("请选择预计交货日期", '提示');
                return;
            }
            var saleOrderModel = new Object();
            saleOrderModel = FormInfo;
            saleOrderModel.unionName = FormInfo.customerName;
            saleOrderModel.unionId = FormInfo.customerId
            saleOrderModel.orderDate = new Date(FormInfo.orderDate);
            saleOrderModel.orderDeliveryDate = new Date(FormInfo.orderDeliveryDate);

            $tabs(tableId).datagrid('endEdit', editRow);
            var ConRows = [];
            var Rows = $tabs(tableId).datagrid('getRows');
            for (var i = 0; i < Rows.length; i++) {
                if (typeof Rows[i].id != "undefined") {
                    if (Rows[i].goodsQuantity != "" || Rows[i].giveQuantity != "") {
                        if (Rows[i].goodsQuantity == "") {
                            Rows[i].goodsQuantity = 0;
                        }
                        if (Rows[i].giveQuantity == "") {
                            Rows[i].giveQuantity = 0;
                        }
                        Rows[i].goodsId = Rows[i].id;
                        ConRows.push(Rows[i]);
                    } else {
                        abp.message.error("销售数量和赠送数量不能同时为0", '提示');
                        return;
                    }

                }
            }
            if (ConRows.length == 0) {
                abp.message.error("请选择分类明细", '提示');
                return;
            }
            saleOrderModel.SaleOrderDetailList = ConRows;
            AddProcessFlag = true;
            _saleOrderService.newSaleOrder(saleOrderModel).done(function (data) {
                if (data.code == 1) {
                    AddProcessFlag = false;
                    abp.message.error(data.message, '提示');
                } else {
                    abp.message.success("操作成功", '提示');
                }
            }).always(function () {
            }).fail(function (data) {
                AddProcessFlag = false;
                abp.message.error("操作失败", '提示');
            });
        });

        var activeId = $("#tab-content-tabitem > .active").attr("id");

        $tabs(".querySearch").click(function () {
            console.log($(this).parent().serialize())
        })
        $tabs(".customer-search").click(function () {
            var Search = $(this).parent().serializeObject();
            DG.setQueryParamsRequest($tabs("#customerForSaleList"), Search)
        })

        $tabs(".selectCustomer").each(function () {
            $(this).textbox($.extend({}, __customerOption, {
                OnSelectedCustomer: function (data) {
                    $tabs('form[name=form1]').form('load', { payWay: data.payWay, accountDate: data.accountDate });
                }
            }));
        });

        $tabs(tableId).datagrid("keyCtr");
        $tabs(tableId).datagrid
            ({
                fitColumns: true,
                width: '100%',
                fit: true,
                showFooter: true,
                //是否显示斑马线效果;同一行中显示数据;显示一个行号列;只允许选择一行
                striped: false, nowrap: true, rownumbers: true, singleSelect: true,
                url: "",
                //分页相关属性
                pagination: true, pageSize: 20, pageList: [20, 50, 100, 150, 200], pageNumber: 1, sortName: 'GoodsCode', sortOrder: 'asc',
                onClickCell: function (index, field) {
                    DG.onClickCell(index, field, this);
                    editRow = index;
                    $tabs(tableId).datagrid('beginEdit', index);
                    DG.selectClickCell(field);
                },
                rowStyler: function (index, row) { },
                onLoadSuccess: function (data) {
                    tableShow(tableId);
                    DG.countColumns(tableId, "goodsQuantity", "goodsAmount", "amountNoTaxRate");
                },
                onBeginEdit: function (index, row) { },
                onAfterEdit: function (index, row, changes) {
                    DG.calculationAmount(tableId, index, row, changes);
                    var num = DG.bottomAmount(tableId, "goodsAmount");
                    $("#" + activeId + " .spanAmount").text(num);
                    //if (changes.goodsQuantity == '' || row.id == undefined) {
                    //    return;
                    //}
                    //if (row.goodsPrice) {
                    //    $('#NewSaleOrderList').datagrid('updateRow', {
                    //        index: index,
                    //        row: {
                    //            priceNoTaxRate: DG.CutFour(row.goodsPrice / ((100 + parseInt(row.taxRate)) / 100))
                    //        }
                    //    })
                    //    if (row.goodsQuantity) {
                    //        $('#NewSaleOrderList').datagrid('updateRow', {
                    //            index: index,
                    //            row: {
                    //                goodsAmount: DG.CutFour(row.goodsPrice * parseInt(row.goodsQuantity)),
                    //                amountNoTaxRate: DG.CutFour(DG.CutFour(row.goodsPrice / ((100 + parseInt(row.taxRate)) / 100)) * parseInt(row.goodsQuantity))
                    //            }
                    //        });
                    //        DG.countColumns("#NewSaleOrderList", "goodsQuantity", "goodsAmount", "amountNoTaxRate");
                    //    }
                    //}
                },
                columns: [[
                    {
                        field: 'id', title: '', sortable: true, width: 150, align: 'center', hidden: true,
                    },
                    {
                        field: 'warehouseId', title: '', sortable: true, width: 150, align: 'center', hidden: true,
                    },
                    {
                        field: 'warehouseName', title: '', sortable: true, width: 150, align: 'center', hidden: true,
                    },
                    {
                        field: 'goodsName', title: '分类名称', sortable: true, width: 150, align: 'center', editor: {
                            type: 'goods', options: {
                                getGoodsQueryParam: function () {
                                    //var activeId = $('#tab-content-tabitem .active').eq(0).attr('id');
                                    //if ($('#' + activeId + ' input[name=customerId]').val() == "" || $('#' + activeId+' input[name=warehouseId]').val() == "") {
                                    //    abp.message.error("请先选择客户和仓库", "提示");
                                    //    return false;
                                    //}
                                    return { unionId: $("#" + activeId + " [name='customerId']").val(), warehousesId: $("#" + activeId+" [name='warehouseId']").val(), GoodsPriceType: 2 };
                                },
                                OnSelectedGoods: function (goods) {
                                    DG.goodsRowsWindow(tableId, goods, false);
                                }
                            }
                        }
                    },
                    {
                        field: 'goodsCode', title: '分类编码', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsArticleNumber', title: '分类货号', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsSpecification', title: '分类规格', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsBarcode', title: '分类条码', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'discountRate', title: '扣率', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsUnit', title: '分类单位', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'taxRate', title: '税率', sortable: true, width: 150, align: 'center', editor: {
                            type: 'numberbox',
                            options: {
                                align: 'right',
                                precision: 0,
                                min: 0
                            },
                        }
                    },
                    {
                        field: 'goodsQuantity', title: '数量', sortable: true, width: 150, align: 'center', editor: {
                            type: 'numberbox',
                            options: {
                                align: 'right',
                                precision: 3,
                                min: 0
                            },
                        }
                    },
                    {
                        field: 'giveQuantity', title: '赠送数量', sortable: true, width: 150, align: 'center', editor: {
                            type: 'numberbox',
                            options: {
                                align: 'right',
                                precision: 3,
                                min: 0
                            },
                        }
                    },
                    {
                        field: 'auxiliaryQuantity', title: '辅助数量', sortable: true, width: 150, align: 'center', editor: {
                            type: 'numberbox',
                            options: {
                                align: 'right',
                                precision: 3,
                                min: 0
                            },
                        }
                    },
                    {
                        field: 'auxiliaryUnit', title: '辅助单位', sortable: true, width: 150, align: 'center',
                    },
                    {
                        field: 'goodsPrice', title: '含税单价', sortable: true, width: 150, align: 'center', editor: {
                            type: 'numberbox',
                            options: {
                                align: 'right',
                                precision: 3,
                                min: 0
                            },
                        }
                    },
                    {
                        field: 'priceNoTaxRate', title: '不含税单价', sortable: true, width: 150, align: 'center', formatter: function (value, row, index) {
                            if (row.goodsPrice) {
                                return DG.CutFour(row.goodsPrice / ((100 + parseInt(row.taxRate)) / 100));
                            }
                        }
                    },
                    {
                        field: 'goodsAmount', title: '含税金额', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'amountNoTaxRate', title: '不含税金额', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'remark', title: '备注', sortable: true, width: 150, align: 'center', editor: {
                            type: 'textbox',
                            options: {
                                align: 'left',
                            },
                        }
                    },
                ]],
                toolbar: intToolbar(tableId, "add", "insert", "del", "search")
            });
        for (var i = 0; i < 10; i++) {
            //添加空行
            DG.insertRow($tabs(tableId));
        }
        $tabs(tableId).datagrid('selectRow', 0);
    });
})();