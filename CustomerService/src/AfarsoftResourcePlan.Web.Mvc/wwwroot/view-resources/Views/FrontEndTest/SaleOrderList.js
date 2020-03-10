(function () {
    $(function () {
        var _Service = abp.services.app.recovery;
        var tableId = "#SaleOrderTableList";
        initRegionSelector("#searchRecoveryRegionProvince", "#searchRecoveryRegionCity", "#searchRecoveryRegionRegion");

        $tabs(".querySearch").click(function () {
            var Search = $(this).parent().serializeObject();
            DG.setQueryParamsRequest($tabs(tableId), Search)
        })

        InitDatePicker($tabs('.datePicker'), 2, $tabs('.querySearch'));

        $tabs(tableId).datagrid
            ({
                fitColumns: true,
                width: '100%',
                fit: true,
                showFooter: true,
                //是否显示斑马线效果;同一行中显示数据;显示一个行号列;只允许选择一行
                striped: false, nowrap: true, rownumbers: true, singleSelect: true,
                queryParams: {
                    StartDate: $tabs(".StartDate").datebox('getValue'),
                    EndDate: $tabs(".EndDate").datebox('getValue')
                },
                url: "/FrontEndTest/GetIndex",
                //分页相关属性
                pagination: true, pageSize: 20, pageList: [20, 50, 100, 150, 200], pageNumber: 1, sortName: 'orderCode', sortOrder: 'desc',
                onClickCell: function (index, field) {
                    DG.onClickCell(index, field, this);
                    editRow = index;
                    $tabs(tableId).datagrid('beginEdit', index);
                    DG.selectClickCell(field);
                },
                rowStyler: function (index, row) { },
                onLoadSuccess: function (data) {
                    $tabs(tableId).datagrid("keyCtr");
                    tableShow(tableId);
                },

                onBeginEdit: function (index, row) { },
                onAfterEdit: function (index, row, changes) { },
                columns: [[
                    {
                        field: 'id', title: '操作', sortable: true, width: 150, align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            return renderGridButton([
                                addGridButton("编辑", SaleOrderInfoEdit, rowData.orderState == 1 || rowData.orderState == 2, ""),
                                addGridButton("审核", SaleOrderInfoVerify, rowData.orderState == 1, ""),
                                addGridButton("删除", SaleOrderInfoDelete, rowData.orderState == 1 || rowData.orderState == 2, ""),
                                addGridButton("出库", SaleOrderInfoOut, rowData.orderState == 3 || rowData.orderState == 4, ""),
                                addGridButton("详情", SaleOrderInfoDetail, true, "")
                            ], 4);
                        }
                    },
                    {
                        field: 'orderCode', title: '单据编号', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'orderStateName', title: '单据状态', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'unionName', title: '客户名称', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'salerName', title: '销售员', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'makerName', title: '制单人', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'makerTime', title: '制单时间', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'makerRemark', title: '制单备注', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'saleQuantity', title: '订单数量', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'outedQuantity', title: '出库数量', sortable: true, width: 150, align: 'center'
                    },
                ]],
            });
        function SaleOrderInfoEdit(value, rowData, rowIndex) {
            clickLeftNav("EditSaleOrderInfoDiv", "编辑订单", "/Sale/EditSaleOrder", { orderId: rowData.id }, true)
        }
        function SaleOrderInfoVerify(value, rowData, rowIndex) {
            clickLeftNav("SaleOrderInfoDiv", "审核订单", "/Sale/SaleOrderInfoDetail", { orderId: rowData.id, type: "Verify" }, true)
        }
        function SaleOrderInfoDelete(value, rowData, rowIndex) {
            clickLeftNav("SaleOrderInfoDiv", "删除订单", "/Sale/SaleOrderInfoDetail", { orderId: rowData.id, type: "Delete" }, true)
        }
        function SaleOrderInfoOut(value, rowData, rowIndex) {
            clickLeftNav("SaleOrderToOutDiv", "订单出库", "/Sale/SaleOrderToOutOrder", { orderId: rowData.id }, true)
        }
        function SaleOrderInfoDetail(value, rowData, rowIndex) {
            clickLeftNav("SaleOrderInfoDiv", "订单详情", "/Sale/SaleOrderInfoDetail", { orderId: rowData.id, type: "" }, true)
        }
    });
})();